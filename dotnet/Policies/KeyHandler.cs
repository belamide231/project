using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;


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


        var userId = context.User.FindFirst(f => f.Type == JwtHelper._userId)?.Value;
        var authId = context.User.FindFirst(f => f.Type == JwtHelper._authId)?.Value;
        var authKey = context.User.FindFirst(f => f.Type == JwtHelper._authKey)?.Value;


        if(string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(authId) || string.IsNullOrEmpty(authKey)) {
            context.Fail();
            return;
        }


        var redisQuery = await _redis.F_Authorizations().StringGetAsync(authId);
        if(!string.IsNullOrEmpty(redisQuery) && XxhashHelper.Verify(authKey, redisQuery!)) {
            await _redis.F_Authorizations().KeyExpireAsync(authId, TimeSpan.FromMinutes(_duration));
            await _redis.F_UserInfo().KeyExpireAsync(userId, TimeSpan.FromMinutes(_duration));
            context.Succeed(requirement);
            return;
        }


        var mongoQuery = await _mongo.F_UsersCollection().Find(
            Builders<ApplicationUsers>.Filter.And(
                Builders<ApplicationUsers>.Filter.Eq(f => f.Id, new ObjectId(userId)),
                Builders<ApplicationUsers>.Filter.ElemMatch(f => f.Authorization,
                    Builders<AuthorizationSchema>.Filter.Eq(f => f.AuthorizationID, authId)
                )
            )
        ).Project<ApplicationUsers>(
            Builders<ApplicationUsers>.Projection.ElemMatch(f => f.Authorization, 
                Builders<AuthorizationSchema>.Filter.Eq(f => f.AuthorizationID, authId)
            )
            .Exclude(f => f.Id)
        ).FirstOrDefaultAsync();


        if(mongoQuery == null || !XxhashHelper.Verify(authKey, mongoQuery.Authorization.FirstOrDefault()!.HashedAuthorizationKey)) {
            context.Fail();
            return;
        }


        await _redis.F_UserInfo().StringSetAsync(userId, JsonConvert.SerializeObject(await _mongo.F_UsersCollection()
            .Find(f => f.Id == new ObjectId(userId))
            .Project<ApplicationUserEntity>(
                Builders<ApplicationUsers>.Projection
                    .Exclude(f => f.Id)
                    .Exclude(f => f.LockoutEnd)
                    .Exclude(f => f.Roles)
                    .Exclude(f => f.Claims)
                    .Exclude(f => f.Logins)
                    .Exclude(f => f.Tokens)
                    .Exclude(f => f.Authorization)
            )
            .FirstOrDefaultAsync()), TimeSpan.FromMinutes(_duration));
        await _redis.F_Authorizations().StringSetAsync(authId, mongoQuery.Authorization.FirstOrDefault()!.HashedAuthorizationKey, TimeSpan.FromMinutes(_duration));
        context.Succeed(requirement);
    }
}