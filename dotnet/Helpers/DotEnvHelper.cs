using dotenv.net;

public class DotEnvHelper {

    public static string? Url;
    public static string? MongoDbUrl;
    public static string? RedisDbUrl;
    public static string? JwtKey;
    public static string? CacheDuration;
    public static string? ConversationMigrationDuration;
    public static string? GmailUsername;
    public static string? GmailPassword;

    public DotEnvHelper() {

        DotEnv.Load();

        Url = Environment.GetEnvironmentVariable("URL");
        MongoDbUrl = Environment.GetEnvironmentVariable("MONGO");
        RedisDbUrl = Environment.GetEnvironmentVariable("REDIS");
        JwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
        CacheDuration = Environment.GetEnvironmentVariable("CACHE_DURATION");
        ConversationMigrationDuration = Environment.GetEnvironmentVariable("CONVERSATION_MIGRATION_DURATION");
        GmailUsername = Environment.GetEnvironmentVariable("GMAIL_USER");
        GmailPassword = Environment.GetEnvironmentVariable("GMAIL_PASS");
    }
}