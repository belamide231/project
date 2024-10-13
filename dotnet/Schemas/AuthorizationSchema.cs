using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class AuthorizationSchema {

    [BsonElement("AuthorizationID")]
    public string AuthorizationID { get; set; }

    [BsonElement("Created")]
    public DateTime Created { get; set; }

    [BsonElement("HashedAuthorizationKey")]
    public string HashedAuthorizationKey { get; set; }

    [BsonIgnore]
    public string AuthorizationKey;

    public AuthorizationSchema() {

        AuthorizationID = Guid.NewGuid().ToString() + "-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        Created = DateTime.UtcNow;
        AuthorizationKey = BCryptHelper.Hash(AuthorizationID); 
        HashedAuthorizationKey = XxhashHelper.Hash(AuthorizationKey);
    }
}