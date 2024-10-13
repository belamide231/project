using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
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


    public async Task<StatusModel> F_VerifyAsync(VerifyDTO DTO) {


        var taken = await _userManager.FindByEmailAsync(DTO.Email);
        if(taken != null) {
            return new VerifyResult.Conflict(VerifyResult.Taken);
        }


        var verificationCode = await _redis.F_Verifications().StringGetAsync(DTO.Email);
        if(!string.IsNullOrEmpty(verificationCode)) {


            if(Newtonsoft.Json.JsonConvert.DeserializeObject<VerificationModel>(verificationCode!)!.FailedAttempts >= 3)
                return new RegisterResults.VerificationCodeInvalid(RegisterResults.RegisteringThisEmailIsLock);

            return new VerifyResult.Success(VerifyResult.Exist);
        }

        var stored = await _redis.F_Verifications().StringSetAsync(DTO.Email, Newtonsoft.Json.JsonConvert.SerializeObject(new VerificationModel(DTO.VerificationCode, VerificationModel.Registration, 0)), TimeSpan.FromMinutes(5));
        if(!stored) {
            return new VerifyResult.Conflict(VerifyResult.Error);
        }
        
        await MailKitHelper.F_MailVerificationCode(DTO.Email, DTO.VerificationCode);
        return new VerifyResult.Success(VerifyResult.Mailed);
    }


    public async Task<StatusModel> F_RegisterAsync(RegisterDTO DTO) {


        var verificationCode = await _redis.F_Verifications().StringGetAsync(DTO.Email);
        if(string.IsNullOrEmpty(verificationCode)) 
            return new RegisterResults.VerificationCodeInvalid(RegisterResults.InvalidVerificationCode);
        

        var verificationObject = Newtonsoft.Json.JsonConvert.DeserializeObject<VerificationModel>(verificationCode!);
        if(verificationObject!.FailedAttempts >= 3) 
            return new RegisterResults.VerificationCodeInvalid(RegisterResults.RegisteringThisEmailIsLock);


        if(verificationObject!.VerificationCode != DTO.VerificationCode || verificationObject.VerificationType != VerificationModel.Registration) {
            verificationObject.FailedAttempts += 1;
            await _redis.F_Verifications().StringSetAsync(DTO.Email, Newtonsoft.Json.JsonConvert.SerializeObject(verificationObject), await _redis.F_Verifications().KeyTimeToLiveAsync(DTO.Email));
            return new RegisterResults.VerificationCodeConflict(RegisterResults.IncorrectVerificationCode, verificationObject.FailedAttempts);
        }


        var result = await _userManager.CreateAsync(new ApplicationUsers(DTO.Email), DTO.Password);
        if(!result.Succeeded) 
            return new RegisterResults.RegisterFail(result.Errors.FirstOrDefault()?.Description!);


        var deviceId = string.IsNullOrEmpty(DTO.DeviceId) ? Guid.NewGuid() + "-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString() : DTO.DeviceId;
        var user = await _mongo.F_UsersCollection().FindOneAndUpdateAsync(
            Builders<ApplicationUsers>.Filter.Eq(f => f.Email, DTO.Email),
            Builders<ApplicationUsers>.Update.Push(f => f.TrustedDevices, XxhashHelper.Hash(deviceId).ToString())
        );


        await _redis.F_Verifications().KeyDeleteAsync(DTO.Email);
        return new RegisterResults.RegisterSuccess(DTO.Email, DTO.Password, deviceId);
    }


    public async Task<StatusModel> F_LoginAsync(LoginDTO DTO) {


        var user = await _userManager.FindByEmailAsync(DTO.Email);
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


        var trustedDevicesId = await _mongo.F_UsersCollection().Find(
            Builders<ApplicationUsers>.Filter
                .Eq(f => f.Email, DTO.Email)
        ).Project<BsonDocument>(
            Builders<ApplicationUsers>.Projection
                .Include(f => f.TrustedDevices)
                .Include(f => f.Email)
                .Exclude(f => f.Id)
        ).FirstOrDefaultAsync();


        if(DTO.Trust) {

            if(string.IsNullOrEmpty(DTO.DeviceId))
                DTO.DeviceId = Guid.NewGuid().ToString() + "-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            if(!Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(trustedDevicesId["TrustedDevices"].ToString()!)!.Any(deviceId => XxhashHelper.Verify(DTO.DeviceId, deviceId)))
                await _mongo.F_UsersCollection().FindOneAndUpdateAsync(
                    Builders<ApplicationUsers>.Filter.Eq(f => f.Email, DTO.Email),
                    Builders<ApplicationUsers>.Update.Push(f => f.TrustedDevices, XxhashHelper.Hash(DTO.DeviceId))
                );
            else {
                
            }
        }

        await _userManager.ResetAccessFailedCountAsync(user);
        return new LoginResult.LoginSuccess(JwtHelper.F_Tokenize(user.Id.ToString(), DTO.DeviceId), DTO.DeviceId);
    }
}