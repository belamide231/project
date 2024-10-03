using System.Text.Json.Serialization;

public class SendDTO {

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
    
    [JsonPropertyName("receivers")]
    public List<string> Receivers { get; set; } = new List<string>();
    }