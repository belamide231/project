using Microsoft.AspNetCore.Authorization;


public class WebsocketAuthorization {


    private readonly RequestDelegate _next;
    private readonly IAuthorizationService _authorize;


    public WebsocketAuthorization(RequestDelegate next, IAuthorizationService authorize) {
        _next = next;
        _authorize = authorize;
    }


    public async Task InvokeAsync(HttpContext context) {

        if(context.WebSockets.IsWebSocketRequest && context.Request.Path == "/chat") {

            var authorization = await _authorize.AuthorizeAsync(context.User, null, KeyRequirement._policy);

            if(!authorization.Succeeded) {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }

        await _next(context);
    }
}