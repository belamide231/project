using MongoDB.Bson;
using MongoDB.Driver;

public class Mongo {

    private readonly IMongoDatabase _mongo;
    public static readonly string _mongoUsers = "Users";
    public static readonly string _mongoRoles = "Roles";
    public static readonly string _mongoConversations = "Conversations"; 
    public static readonly string _mongoUsersData = "UsersData";

    public Mongo() {

        var ConnectionString = MongoUrl.Create(DotEnvHelper.MongoDbUrl);
        _mongo = new MongoClient(ConnectionString).GetDatabase(ConnectionString.DatabaseName);
    }


    public IMongoCollection<ApplicationUsers> F_UsersCollection() => _mongo.GetCollection<ApplicationUsers>(_mongoUsers);
    public IMongoCollection<ApplicationRoles> F_RolesCollection() => _mongo.GetCollection<ApplicationRoles>(_mongoRoles);
    public IMongoCollection<ConversationSchema> F_ConversationsCollection() => _mongo.GetCollection<ConversationSchema>(_mongoConversations);
    public IMongoCollection<UserDataSchema> F_UserDataCollection() => _mongo.GetCollection<UserDataSchema>(_mongoUsersData);
}