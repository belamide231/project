using AspNetCore.Identity.Mongo.Model;


public class ApplicationUsers : MongoUser {

    public List<AuthorizationSchema> Authorization { get; set; } = new List<AuthorizationSchema>();

    public ApplicationUsers(string email)  {
        Email = email;
        UserName = email;
    }

}