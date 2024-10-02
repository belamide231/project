using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json;

public class MessageServices {

    private readonly Mongo _mongo;
    private readonly Redis _redis;

    public MessageServices(Mongo mongo, Redis redis) {
        _mongo = mongo;
        _redis = redis;
    }

    public async Task<StatusModel> SendAsync(SendDTO DTO, string userId) {

        // STORE THE MESSAGE IN THE REDIS, THE FUNCTIONALITY OF REDIS IS TO MIGRATE THE DATA IN MONGO IF THE CONVERSATION PASSES AN HOUR

        List<string> userIds = new List<string> { userId };
        userIds.AddRange(DTO.Receivers);

        var filter = Builders<ConversationSchema>.Filter.All(f => f.UserIds, userIds);

        var query = await _mongo.F_ConversationsCollection()
            .Find(filter)
            .FirstOrDefaultAsync();
        Console.WriteLine(query.Id);

        return new StatusModel(200);
    }
}