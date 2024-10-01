using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using dotenv.net;
using Microsoft.IdentityModel.Tokens;

public class JwtHelper {

    public static readonly string _userId = "UserID";
    public static readonly string _authId = "AuthenticationID";
    public static readonly string _authKey = "AuthenticationKey";
    private static string? _key;
    private static readonly JwtSecurityTokenHandler? _tokenHandler = new JwtSecurityTokenHandler();


    public JwtHelper() {
        
        DotEnv.Load();

        _key = Environment.GetEnvironmentVariable("JWT_KEY")!;
    }

    
    public static string F_Tokenize(string userId, string authId, string key) => 
        _tokenHandler!.WriteToken(_tokenHandler.CreateToken(new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new Claim[] {
                new Claim(_userId, userId),
                new Claim(_authId, authId),
                new Claim(_authKey, key)
            }),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key!)),
                SecurityAlgorithms.HmacSha256Signature
            )
        }));
}