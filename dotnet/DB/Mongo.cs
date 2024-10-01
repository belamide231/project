using dotenv.net;
using MongoDB.Driver;

public class Mongo {

    private readonly IMongoDatabase _mongo;
    public static readonly string _mongoUsers = "Users";
    public static readonly string _mongoRoles = "Roles";

    public Mongo() {

        DotEnv.Load();

        var ConnectionString = MongoUrl.Create(Environment.GetEnvironmentVariable("MONGO"));
        _mongo = new MongoClient(ConnectionString).GetDatabase(ConnectionString.DatabaseName);
    }


    public IMongoCollection<ApplicationUsers> F_UsersCollection() => _mongo.GetCollection<ApplicationUsers>(_mongoUsers);
    public IMongoCollection<ApplicationRoles> F_RolesCollection() => _mongo.GetCollection<ApplicationRoles>(_mongoRoles);
}