using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

public class NotifierWebsocket {

    public static async Task F_MessageNotifier(string conversationId, List<string> userIds) {

        foreach(var userId in userIds) {

            var websockets = Websocket.F_GetUserWebsockets(userId);
            if(websockets != null) {

                foreach(var websocket in websockets) 
                    await websocket.Value.SendAsync(Encoding.UTF8.GetBytes(conversationId), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}