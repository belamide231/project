using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using dotenv.net;
using Microsoft.IdentityModel.Tokens;

public class JwtHelper {

    public static readonly string _userId = "UserID";
    public static readonly string _deviceId = "DeviceID";
    private static string? _key = DotEnvHelper.JwtKey;
    private static readonly JwtSecurityTokenHandler? _tokenHandler = new JwtSecurityTokenHandler();

    
    public static string F_Tokenize(string userId, string deviceId) => 
        _tokenHandler!.WriteToken(_tokenHandler.CreateToken(new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new Claim[] {
                new Claim(_userId, userId),
                new Claim(_deviceId, deviceId),
            }),
            Expires = DateTime.UtcNow.AddYears(100),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key!)),
                SecurityAlgorithms.HmacSha256Signature
            )
        }));
}