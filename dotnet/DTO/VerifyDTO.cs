using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class VerifyDTO {

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    public string VerificationCode = (new Random().Next(100000, 1000000)).ToString();
}