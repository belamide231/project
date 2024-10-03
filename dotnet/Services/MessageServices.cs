using System.Collections.Concurrent;
using MongoDB.Driver;

public class MessageServices {


    private readonly Mongo _mongo;
    private readonly Redis _redis;
    private readonly ConcurrentDictionary<string, CancellationTokenSource> _cancellationTokenSources;
    public MessageServices(Mongo mongo, Redis redis) {
        _mongo = mongo;
        _redis = redis;
        _cancellationTokenSources = new ConcurrentDictionary<string, CancellationTokenSource>();
    }


    public async Task<StatusModel> SendAsync(SendDTO DTO, string userId) {


        var userIds = new List<string>(DTO.Receivers);
        var conversation = (await (await _mongo.F_ConversationsCollection().FindAsync(Builders<ConversationSchema>.Filter.All(f => f.UserIds, userIds))).ToListAsync()).FirstOrDefault();


        if(conversation == null) {
            conversation = new ConversationSchema(userIds);
            await _mongo.F_ConversationsCollection().InsertOneAsync(conversation);        
        }


        if(_cancellationTokenSources.TryGetValue(conversation.Id.ToString(), out var cancellationTokenSource)) {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }


        var newCancellationTokenSource = new CancellationTokenSource();
        _cancellationTokenSources.AddOrUpdate(conversation.Id.ToString(), newCancellationTokenSource, (_, _) => newCancellationTokenSource);


        await _redis.F_Conversations().ListRightPushAsync(conversation.Id.ToString(), Newtonsoft.Json.JsonConvert.SerializeObject(new MessageSchema(DTO.Receivers, userId, DTO.Message)));
        await NotifierWebsocket.F_MessageNotifier(conversation.Id.ToString(), DTO.Receivers);


        _ = Task.Run(async() => {


            await Task.Delay(TimeSpan.FromMinutes(int.TryParse(DotEnvHelper.CacheDuration, out var duration) ? duration : 60), newCancellationTokenSource.Token);

            await _mongo.F_ConversationsCollection().FindOneAndUpdateAsync(
                f => f.Id == conversation.Id,
                Builders<ConversationSchema>.Update.PushEach(f => f.Messages, 
                    (await _redis.F_Conversations().ListRangeAsync(conversation.Id.ToString())).Select(message => Newtonsoft.Json.JsonConvert.DeserializeObject<MessageSchema>(message!)).ToList()    
                )
            );
            await _redis.F_Conversations().KeyDeleteAsync(conversation.Id.ToString());
        });

        return new StatusModel(200);
    }
}