using Microsoft.AspNetCore.Authorization;

public class KeyRequirement : IAuthorizationRequirement {

    public const string _policy = "CERTIFIED_USER";    
}