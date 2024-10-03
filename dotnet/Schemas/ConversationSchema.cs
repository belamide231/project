using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class MessageSchema {

    [BsonElement("Receiver")]
    public List<string> Receivers { get; set; }

    [BsonElement("Sender")]
    public string Sender { get; set; }

    [BsonElement("Created")]
    public DateTime Created { get; set; }

    [BsonElement("Message")]
    public string Message { get; set; }

    [BsonElement("Status")]
    public string Status { get; set; }

    public MessageSchema(List<string> receivers, string sender, string message) {
        Receivers = receivers;
        Sender = sender;
        Created = DateTime.UtcNow;
        Message = message;
        Status = "Sent";
    }
}


public class ConversationSchema {
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    [BsonElement("Messages")]
    public List<MessageSchema> Messages { get; set; } = new List<MessageSchema>();
    
    [BsonElement("UserIds")]
    public List<string> UserIds { get; set; } = new List<string>();
    public ConversationSchema(List<string> usersId) {
        Id = ObjectId.GenerateNewId();
        UserIds.AddRange(usersId);
    }
}