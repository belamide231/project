using AspNetCore.Identity.Mongo.Model;


public class ApplicationUsers : MongoUser {

    public AuthorizationSchema Authorization { get; set; }

    public ApplicationUsers(string email)  {
        Email = email;
        UserName = email;
    }

}