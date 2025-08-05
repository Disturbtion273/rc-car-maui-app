using System.Net.WebSockets;
using System.Text;

namespace rc_car_maui_app.Websocket;

public static class WebsocketClient
{
    private static Queue<string> queue = new Queue<string>();
    
    public static void Send(string message)
    {
        queue.Enqueue(message);
    }

    private static Task SendTask(ClientWebSocket client)
    {
        return Task.Run(async () =>
        {
            try
            {
                while (client.State == WebSocketState.Open)
                {
                    if (!queue.TryDequeue(out var queueItem))
                        continue;
                    
                    Console.Write("Send to server: ");
                    await client.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(queueItem)), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Send error: " + ex.Message);
            }
        });
    }

    private static Task ReceiveTask(ClientWebSocket client)
    {
        return Task.Run(async () =>
        {
            var buffer = new byte[1024];

            try {
                while (client.State == WebSocketState.Open) {
                    var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close) {
                        Console.WriteLine("Server requested close.");
                        break;
                    }

                    var response = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine("Received from server: " + response);
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Receive error: " + ex.Message);
            }
        });
    }

    public static async Task Connect(string uri)
    {
        using ClientWebSocket client = new ClientWebSocket();

        try {
            await client.ConnectAsync(new Uri(uri), CancellationToken.None);
            Console.WriteLine("Connected to WebSocket server.");

            await Task.WhenAny(ReceiveTask(client), SendTask(client));
        }
        catch (WebSocketException wse) {
            Console.WriteLine($"WebSocket error: {wse.Message}");
        }
        finally {
            if (client.State == WebSocketState.Open || client.State == WebSocketState.CloseReceived) {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closing", CancellationToken.None);
                Console.WriteLine("Connection closed.");
            }
        }
    }
}