using System.Text.Json.Serialization;

public class SendDTO {

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
    
    [JsonPropertyName("receiver")]
    public string Receiver { get; set; } = string.Empty;
}