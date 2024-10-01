using Microsoft.AspNetCore.Authorization;

public class KeyRequirement : IAuthorizationRequirement {

    public static readonly string _policy = "CERTIFIED_USER";    
}