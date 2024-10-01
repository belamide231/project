using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;

public class AuthorizationSchema {

    [BsonElement("AuthorizationID")]
    public string AuthorizationID { get; set; }

    [BsonElement("LoginDevice")]
    public string LoginDevice { get; set; } = string.Empty;

    [BsonElement("LoginLocation")]
    public string LoginLocation { get; set; } = string.Empty;

    [BsonElement("Created")]
    public DateTime Created { get; set; }

    [BsonElement("HashedAuthorizationKey")]
    public string HashedAuthorizationKey { get; set; }

    [BsonIgnore]
    public string AuthorizationKey { get; set; }

    public AuthorizationSchema(string loginDevice, string loginLocation) {

        AuthorizationID = Guid.NewGuid().ToString() + "-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        LoginDevice = loginDevice;
        LoginLocation = loginLocation;
        Created = DateTime.UtcNow;
        AuthorizationKey = BCryptHelper.Hash(AuthorizationID); 
        HashedAuthorizationKey = XxhashHelper.Hash(AuthorizationKey);
    }
}