using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

public class MessageWebsocket {

    private ConcurrentDictionary<string, CancellationTokenSource> _cancellationTokenSource = new ConcurrentDictionary<string, CancellationTokenSource>();

    public static async Task F_Send(string message, string receiver) {

        foreach(var websocket in Websocket.F_GetUserWebsockets(receiver)) 
            await websocket.Value.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, CancellationToken.None);
    }
}