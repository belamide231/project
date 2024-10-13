using Newtonsoft.Json;

public class RegisterDTO {

    [JsonProperty("deviceId")]
    public string DeviceId { get; set; } = string.Empty;

    [JsonProperty("verificationCode")]
    public string VerificationCode { get; set; } = string.Empty;

    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;
    
    [JsonProperty("password")]
    public string Password { get; set; } = string.Empty;
}