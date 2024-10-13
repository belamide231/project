using dotenv.net;
using StackExchange.Redis;

public class Redis {


    private readonly IDatabase _authorizations;
    private readonly IDatabase _conversations;
    private readonly IDatabase _userinfo;
    private readonly IDatabase _verifications;

    
    public Redis() {

        DotEnv.Load();

        var redis = ConnectionMultiplexer.Connect(DotEnvHelper.RedisDbUrl!);
        _authorizations = redis.GetDatabase(0);
        _userinfo = redis.GetDatabase(1);
        _conversations = redis.GetDatabase(2);
        _verifications = redis.GetDatabase(3);
    }

    
    public IDatabase F_Authorizations() => _authorizations;
    public IDatabase F_UserInfo() => _userinfo;
    public IDatabase F_Conversations() => _conversations; 
    public IDatabase F_Verifications() => _verifications;
}