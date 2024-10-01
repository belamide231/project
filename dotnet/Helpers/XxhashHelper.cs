using System.IO.Hashing;
using System.Text;

public class XxhashHelper {

    public static string Hash(string text) => XxHash64.HashToUInt64(Encoding.ASCII.GetBytes(text)).ToString();
    public static bool Verify(string text, string hash) =>
        ulong.TryParse(hash, out var hashedInt64) 
            && 
        hashedInt64 == XxHash64.HashToUInt64(Encoding.ASCII.GetBytes(text)) ? true : false; 
}