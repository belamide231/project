using dotenv.net;
using StackExchange.Redis;

public class Redis {


    private readonly IDatabase _authorizations;
    private readonly IDatabase _conversations;

    
    public Redis() {

        DotEnv.Load();

        var redis = ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("REDIS")!);
        _authorizations = redis.GetDatabase(0);
        _conversations = redis.GetDatabase(1);
    }

    
    public IDatabase F_Authorizations() => _authorizations;
    public IDatabase F_Conversations() => _conversations; 
}