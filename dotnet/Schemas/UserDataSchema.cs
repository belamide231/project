using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


public class ConversationIdSchema {

    [BsonElement("ActiveTime")]
    public DateTime ActiveTime { get; set; }


    [BsonElement("ConversationId")]
    public string? ConversationId { get; set; }

    public ConversationIdSchema(string conversationId) {
        ActiveTime = DateTime.UtcNow;
        ConversationId = conversationId;
    }
}


public class UserDataSchema {

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("ConversationIds")]
    public List<ConversationIdSchema>? ConversationIds { get; set; }

    public UserDataSchema(string id, string conversationId) {
        Id = id;
        ConversationIds = new List<ConversationIdSchema>(new [] {
            new ConversationIdSchema(conversationId)
        });
    }
}