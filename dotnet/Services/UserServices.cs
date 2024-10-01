using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;


public class UserServices {

    
    private readonly Mongo _mongo;
    private readonly UserManager<ApplicationUsers> _userManager;


    public UserServices(Mongo mongo, UserManager<ApplicationUsers> userManager) {
        _mongo = mongo;
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
        if(locked) 
            return new LoginResult.AccountLocked((user.LockoutEnd - DateTimeOffset.UtcNow).ToString()!);

        var match = await _userManager.CheckPasswordAsync(user, DTO.Password);
        if(!match) {
            await _userManager.AccessFailedAsync(user);
            return new LoginResult.LoginFail(LoginResult.IncorrectPassword);
        }

        var auth = new AuthorizationSchema(DTO.Device, DTO.Location);
        var query = await _mongo.F_UsersCollection().FindOneAndUpdateAsync(
            Builders<ApplicationUsers>.Filter.Eq(f => f.Id, user.Id), 
            Builders<ApplicationUsers>.Update.Push(f => f.Authorization, auth)
        );

        if(query == null) 
            return new LoginResult.InternalServerError(LoginResult.InternalServerIsError);

        await _userManager.ResetAccessFailedCountAsync(user);
        return new LoginResult.LoginSuccess(JwtHelper.F_Tokenize(user.Id.ToString(), auth.AuthorizationID, auth.AuthorizationKey));
    }
}