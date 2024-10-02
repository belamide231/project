public class MessageServices {

    private readonly Mongo _mongo;
    private readonly Redis _redis;

    public MessageServices(Mongo mongo, Redis redis) {
        _mongo = mongo;
        _redis = redis;
    }

    public async Task<StatusModel> SendAsync(SendDTO DTO) {

        
        return new StatusModel(200);
    }
}