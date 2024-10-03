using System.Text.Json.Serialization;

public class ReceiveDTO {

    [JsonPropertyName("conversationId")]
    public string? ConversationId { get; set; }
}