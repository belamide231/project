using System.Collections.Concurrent;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Authorization;


public class Websocket : JwtHelper {


    protected readonly ConcurrentDictionary<string, ConcurrentDictionary<string, WebSocket>> _users;
    private readonly RequestDelegate _next;
    private readonly byte[] _buffer;


    public Websocket(RequestDelegate next) {
        _next = next;
        _users = new ConcurrentDictionary<string, ConcurrentDictionary<string, WebSocket>>();
        _buffer = new byte[1024 * 4];
    }


    public async Task ConnectAsync(string userId, string websocketId, WebSocket websocket) {

        var result = await websocket.ReceiveAsync(new ArraySegment<byte>(_buffer), CancellationToken.None);

        if(!result.CloseStatus.HasValue) {

            if(websocket.State != WebSocketState.Open && _users.TryGetValue(userId, out var websockets))
                websockets.TryRemove(websocketId, out _);                
        }

        await websocket.CloseAsync(result.CloseStatus!.Value, result.CloseStatusDescription, CancellationToken.None);
        foreach(var websockets in _users) {
            Console.WriteLine(websockets.Key);
        }
    }


    public async Task InvokeAsync(HttpContext context) {

        if(context.Request.Path == "/connect" && context.WebSockets.IsWebSocketRequest) {

            var userId = context.User.FindFirst(f => f.Type == _userId)?.Value;

            var websocket = await context.WebSockets.AcceptWebSocketAsync();
            if(websocket.State != WebSocketState.Open) {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await _next(context);
            }

            var websocketId = Guid.NewGuid().ToString() + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

            if(_users.TryGetValue(userId!, out var websockets)) {

                websockets.TryAdd(websocketId, websocket);

            } else {

                var newWebsocket = new ConcurrentDictionary<string, WebSocket>();
                newWebsocket.TryAdd(websocketId, websocket);
                
                _users.TryAdd(userId!, newWebsocket);
            }

            await ConnectAsync(userId!, websocketId, websocket);

        } else 
            await _next(context);
    }
}