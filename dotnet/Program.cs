// dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
// dotnet add package AspNetCore.Identity.Mongo
// dotnet add package BCrypt.Net-Next
// dotnet add package System.IO.Hashing
// dotnet add package MongoDB.Driver
// dotnet add package StackExchange.Redis
// dotnet add package Newtonsoft.Json
// dotnet add package dotenv.net


using System.Text;
using AspNetCore.Identity.Mongo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;


new DotEnvHelper();


var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls(DotEnvHelper.Url!);


builder.Services.AddSingleton<Mongo>();
builder.Services.AddSingleton<Redis>();
builder.Services.AddSingleton<IAuthorizationHandler, KeyHandler>();
builder.Services.AddTransient<UserServices>();
builder.Services.AddSingleton<MessageServices>();


builder.Services.AddCors(option => {
    option.AddPolicy("*", policy => {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});
builder.Services.AddIdentityCore<ApplicationUsers>()
                .AddRoles<ApplicationRoles>()
                .AddMongoDbStores<ApplicationUsers, ApplicationRoles, ObjectId>(option => {
                    option.ConnectionString = DotEnvHelper.MongoDbUrl!;
                    option.UsersCollection = Mongo._mongoUsers;
                    option.RolesCollection = Mongo._mongoRoles;
                });
builder.Services.AddAuthorizationBuilder();
builder.Services.AddAuthentication(option => {
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option => {
    option.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(DotEnvHelper.JwtKey!)),
        ClockSkew = TimeSpan.Zero
    };
}).AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization(option => {
    option.AddPolicy(KeyRequirement._policy, policy => policy.AddRequirements(new KeyRequirement()));
});


var app = builder.Build();


if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("*");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseWebSockets();
app.UseMiddleware<WebsocketAuthorization>();
app.UseMiddleware<Websocket>();


app.Run();