using System.Net.WebSockets;
using System.Text;

namespace rc_car_maui_app.Websocket;

/**
 * WebsocketClient is a simple WebSocket client that connects to a server,
 * sends messages from a queue, and receives messages from the server.
 */
public static class WebsocketClient
{
    private static bool connected;
    private static Queue<string> queue = new Queue<string>();

    /**
    * This method connects to the WebSocket server at the specified URI.
    * It should only be called once, typically at application startup.
    */
    public static async Task Connect(string uri)
    {
        using var client = new ClientWebSocket();

        try
        {
            if (!connected)
            {
                await client.ConnectAsync(new Uri(uri), CancellationToken.None);
                Console.WriteLine("Connected to WebSocket server.");
                connected = true;

                await Task.WhenAny(ReceiveTask(client), SendTask(client));
            }
        }
        catch (WebSocketException wse) {
            Console.WriteLine($"WebSocket error: {wse.Message}");
        }
        finally {
            if (client.State is WebSocketState.Open or WebSocketState.CloseReceived) {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closing", CancellationToken.None);
                Console.WriteLine("Connection closed.");
                connected = false;
            }
        }
    }

    /**
     * This method sends a message to the WebSocket server.
     * It can be called multiple times to queue messages for sending.
     */
    public static void Send(string message)
    {
        if (connected) 
            queue.Enqueue(message);
    }

    /**
     * This method checks if the WebSocket client is currently connected to the server.
     */
    public static bool IsConnected()
    {
        return connected;
    }

    /**
     * Internal method that is handling sending messages to the WebSocket server.
     * It runs in a separate task and continuously checks the queue for messages to send.
     */
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

    /**
     * Internal method that is handling receiving messages from the WebSocket server.
     * It runs in a separate task and continuously listens for incoming messages.
     */
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
}