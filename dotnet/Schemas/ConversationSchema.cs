using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


public class MessageSchema {

    [BsonElement("Receiver")]
    public string Receiver { get; set; }

    [BsonElement("Sender")]
    public string Sender { get; set; }

    [BsonElement("Created")]
    public DateTime Created { get; set; }

    [BsonElement("Message")]
    public string Message { get; set; }

    public MessageSchema(string receiver, string sender, string message) {
        Receiver = receiver;
        Sender = sender;
        Created = DateTime.UtcNow;
        Message = message;
    }
}


public class ConversationSchema {
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; } = ObjectId.Empty;

    [BsonElement("Messages")]
    public List<MessageSchema> Messages { get; set; } = new List<MessageSchema>();
    
    [BsonElement("UserIds")]
    public List<string> UserIds { get; set; } = new List<string>();
}