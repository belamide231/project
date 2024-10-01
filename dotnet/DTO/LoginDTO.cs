using MongoDB.Bson.Serialization.Attributes;

public class LoginDTO {

    [BsonElement("username")]
    public string Username { get; set; } = string.Empty;

    [BsonElement("password")]
    public string Password { get; set; } = string.Empty;

    [BsonElement("device")]
    public string Device { get; set; } = string.Empty;

    [BsonElement("location")]
    public string Location { get; set; } = string.Empty;
}