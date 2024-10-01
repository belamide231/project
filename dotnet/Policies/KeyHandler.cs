using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;


public class KeyHandler : AuthorizationHandler<KeyRequirement> {


    private readonly Mongo _mongo;
    private readonly Redis _redis;


    public KeyHandler(Mongo mongo, Redis redis) {
        _mongo = mongo;
        _redis = redis;
    }


    protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, KeyRequirement requirement) {

        var userId = context.User.FindFirst(f => f.Type == JwtHelper._authId);
        var authId = context.User.FindFirst(f => f.Type == JwtHelper._authId);
        var authKey = context.User.FindFirst(f => f.Type == JwtHelper._authKey);

    }
}