using AspNetCore.Identity.Mongo.Model;


public class ApplicationUsers : MongoUser {

    public List<AuthorizationSchema> Authorization { get; set; } = new List<AuthorizationSchema>();
    public List<string> TrustedDevices { get; set; } = new List<string>();

    public ApplicationUsers(string email)  {
        Email = email;
        UserName = email;
    }

}