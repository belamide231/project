using MongoDB.Bson.Serialization.Attributes;

public class LoginDTO {

    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("password")]
    public string Password { get; set; } = string.Empty;

    [BsonElement("deviceId")]
    public string DeviceId { get; set; }
    
    [BsonElement("trust")]
    public bool Trust { get; set; } = false;
}