using Microsoft.AspNetCore.Authorization;


public class KeyHandler : AuthorizationHandler<KeyRequirement> {


    private readonly int _duration;
    private readonly Mongo _mongo;
    private readonly Redis _redis;


    public KeyHandler(Mongo mongo, Redis redis) {

        _duration = int.TryParse(DotEnvHelper.CacheDuration, out var Parsed) ? Parsed : 60;
        _mongo = mongo;
        _redis = redis;
    }


    protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, KeyRequirement requirement) {

        // LOGIC HERE

       context.Succeed(requirement);
    }
}