using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

public class UserServices {

    
    private readonly Mongo _mongo;
    private readonly Redis _redis;
    private readonly UserManager<ApplicationUsers> _userManager;


    public UserServices(Mongo mongo, Redis redis, UserManager<ApplicationUsers> userManager) {
        _mongo = mongo;
        _redis = redis;
        _userManager = userManager;
    }


    public async Task<StatusModel> F_RegisterAsync(RegisterDTO DTO) {

        var result = await _userManager.CreateAsync(new ApplicationUsers(DTO.Email), DTO.Password);
        if(!result.Succeeded) 
            return new RegisterResults.RegisterFail(result.Errors.FirstOrDefault()?.Description!);

        return new RegisterResults.RegisterSuccess(DTO.Email, DTO.Password);
    }


    public async Task<StatusModel> F_LoginAsync(LoginDTO DTO) {

        var user = await _userManager.FindByNameAsync(DTO.Username);
        if(user == null) 
            return new LoginResult.LoginFail(LoginResult.InvalidUsername);
        
        var locked = await _userManager.IsLockedOutAsync(user);
        if(locked) {
            var remaining = await _userManager.GetLockoutEndDateAsync(user) - DateTimeOffset.UtcNow;
            return new LoginResult.AccountLocked(remaining.ToString()!);
        }

        var match = await _userManager.CheckPasswordAsync(user, DTO.Password);
        if(!match) {
            await _userManager.AccessFailedAsync(user);
            return new LoginResult.LoginFail(LoginResult.IncorrectPassword);
        }

        var auth = new AuthorizationSchema("", "");

        var update = Builders<ApplicationUsers>.Update.Push(f => f.Authorization, auth);
        //var stored = await _mongo.F_UsersCollection().FindOneAndUpdateAsync();        

        Console.WriteLine(JwtHelper.F_Tokenize(user.Id.ToString(), auth.AuthorizationID, auth.AuthorizationKey));
        return new StatusModel(StatusCodes.Status200OK);
    }
}