using Newtonsoft.Json;

public class RegisterDTO {


    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;
    
    
    [JsonProperty("password")]
    public string Password { get; set; } = string.Empty;
}