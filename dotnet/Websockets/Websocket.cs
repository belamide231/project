using System.Collections.Concurrent;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;


public class Websocket : JwtHelper {


    private static ConcurrentDictionary<string, ConcurrentDictionary<string, WebSocket>>? _users;
    private readonly RequestDelegate _next;
    private readonly byte[] _buffer;


    public Websocket(RequestDelegate next) {
        _users = new ConcurrentDictionary<string, ConcurrentDictionary<string, WebSocket>>();
        _next = next;
        _buffer = new byte[1024 * 4];
    }


    public void LogUsersConnections() {
        Console.WriteLine("\nACTIVE USERS");
        foreach(var usersWebsocketList in _users!) {
            
            Console.WriteLine(usersWebsocketList.Key);
            foreach(var specificWebsocket in usersWebsocketList.Value) {
                
                Console.WriteLine("\t"+specificWebsocket.Key);
            }
        }
        Console.WriteLine("\n");
    }


    public async Task ConnectAsync(string userId, string websocketId, WebSocket websocket) {

        var connection = await websocket.ReceiveAsync(new ArraySegment<byte>(_buffer), CancellationToken.None);

        if(_users!.TryGetValue(userId, out var userWebsocketList) && userWebsocketList.TryRemove(websocketId, out _) && _users.TryGetValue(userId, out var ws) && string.IsNullOrEmpty(ws.FirstOrDefault().Key))
            _users.Remove(userId, out _);

        LogUsersConnections();

        await websocket.CloseAsync(connection.CloseStatus!.Value, connection.CloseStatusDescription, CancellationToken.None);
    }


    public async Task BadResponse(HttpContext context) {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await _next(context);
        return;
    }


    public async Task InvokeAsync(HttpContext context) {

        if(context.Request.Path == "/chat" && context.WebSockets.IsWebSocketRequest) {

            var userId = context.User.FindFirst(f => f.Type == _userId)?.Value;

            if(string.IsNullOrEmpty(userId))
                await BadResponse(context);

            var websocket = await context.WebSockets.AcceptWebSocketAsync();
            if(websocket.State != WebSocketState.Open) 
                await BadResponse(context);

            var websocketId = Guid.NewGuid().ToString() + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

            _users!.AddOrUpdate(userId!, new ConcurrentDictionary<string, WebSocket>(new [] {
                new KeyValuePair<string, WebSocket>(websocketId, websocket)
            }), (_, value) => {
                value.TryAdd(websocketId, websocket);
                return value;
            });

            LogUsersConnections();
            await ConnectAsync(userId!, websocketId, websocket);

        } else 
            await _next(context);
    }


    public static ConcurrentDictionary<string, WebSocket> F_GetUserWebsockets(string userId) =>
        _users!.TryGetValue(userId, out var userWebsockets) ? userWebsockets : null!;
        
}