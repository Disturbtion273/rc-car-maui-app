using System.Globalization;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using rc_car_maui_app.Controls;
using rc_car_maui_app.Enums;
using rc_car_maui_app.Helpers;
using rc_car_maui_app.Services;

namespace rc_car_maui_app.Websocket;

/**
 * WebsocketClient is a simple WebSocket client that connects to a server,
 * sends messages from a queue, and receives messages from the server.
 */
public static class WebsocketClient
{
    private static ClientWebSocket? client;
    private static WebsocketClientState state = WebsocketClientState.Disconnected;
    private static Queue<string> queue = new Queue<string>();
    private static string host;

    public static event Action<WebsocketClientState>? StateChanged;
    public static event Action<string, Color>? ConnectionInfoChanged;
    public static event Action<Notification?>? NotificationReceived;
    public static event Action<string>? SpeedLimitReceived;
    public static event Action<int>? SpeedInfoChanged;

    private static Dictionary<string, int> controlData = new ();
    private static Dictionary<string, int> sentControlData = new ();

    public static void SetControlData(string key, int value)
    {
        controlData[key] = value;
    }

    public static void SetDrivingMode(DrivingMode mode)
    {
        Send(JsonSerializer.Serialize(new { drivingMode = mode.ToString().ToLower() }));
    }

    /**
     * This method connects to the WebSocket server at the specified URI.
     * It should only be called once, typically at application startup.
     * It triggers the connection failed event if the connection cannot be established.
     */
    public static async Task Connect(string host, int port = 9999)
    {
        client = new ClientWebSocket();
        try
        {
            if (IsDisconnected())
            {
                SetState(WebsocketClientState.Connecting);
                await client.ConnectAsync(new Uri($"ws://{host}:{port}"), CancellationToken.None);
                Console.WriteLine("Connected to WebSocket server.");
                SetState(WebsocketClientState.Connected);
                WebsocketClient.host = host;

                await Task.WhenAny(ReceiveTask(), SendTask(), SendControlData());
            }
        }
        catch (WebSocketException wse)
        {
            Console.WriteLine($"WebSocket error: {wse.Message}");
        }
        finally
        {
            if (client.State is WebSocketState.Open or WebSocketState.CloseReceived)
            {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closing", CancellationToken.None);
                Console.WriteLine("Connection closed.");
                SetState(WebsocketClientState.Disconnected);
                ConnectionInfoChanged?.Invoke("Successfully disconnected", Colors.Green);
            }

            if (IsConnecting())
            {
                SetState(WebsocketClientState.Disconnected);
                ConnectionInfoChanged?.Invoke("Unable to connect to the car", Colors.Red);
            }
        }
    }

    public static async Task Disconnect()
    {
        if (client?.State is WebSocketState.Open)
        {
            SetState(WebsocketClientState.Disconnecting);
            await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closing", CancellationToken.None);
            Console.WriteLine("Disconnected from WebSocket server.");
            SetState(WebsocketClientState.Disconnected);
            ConnectionInfoChanged?.Invoke("Successfully disconnected", Colors.Green);
        }
    }

    /**
     * This method sends a message to the WebSocket server.
     * It can be called multiple times to queue messages for sending.
     */
    public static void Send(string message)
    {
        if (IsConnected()) 
            queue.Enqueue(message);
    }

    /**
     * This method checks if the WebSocket client is currently connected to the server.
     */
    public static bool IsConnected()
    {
        return state == WebsocketClientState.Connected;
    }
    
    /**
     * This method checks if the WebSocket client is currently connecting to the server.
     */
    public static bool IsConnecting()
    {
        return state == WebsocketClientState.Connecting;
    }
    
    /**
     * This method checks if the WebSocket client is currently disconnected from the server.
     */
    public static bool IsDisconnected()
    {
        return state == WebsocketClientState.Disconnected;
    }

    /**
     * This method returns the host (IP-address) of the websocket connection
     */
    public static string GetHost()
    {
        return host;
    }

    /**
     * This method returns the current state of the WebSocket client.
     */
    public static WebsocketClientState GetState()
    {
        return state;
    }
    
    /**
     * This method sets the current state of the WebSocket client.
     * It triggers the StateChanged event if the state has changed.
     */
    public static void SetState(WebsocketClientState state)
    {
        if (WebsocketClient.state != state)
        {
            WebsocketClient.state = state;
            StateChanged?.Invoke(state);
        }
    }

    /**
     * Internal method that is handling sending messages to the WebSocket server.
     * It runs in a separate task and continuously checks the queue for messages to send.
     */
    private static Task SendTask()
    {
        return Task.Run(async () =>
        {
            try
            {
                while (client?.State == WebSocketState.Open)
                {
                    if (!queue.TryDequeue(out var queueItem) || queueItem == null)
                        continue;
                    
                    await client.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(queueItem)), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Send error: " + ex.Message);
                SetState(WebsocketClientState.Disconnected);
                ConnectionInfoChanged?.Invoke("Unexpectedly disconnected from the car", Colors.Red);
            }
        });
    }

    /**
     * Internal method that is handling receiving messages from the WebSocket server.
     * It runs in a separate task and continuously listens for incoming messages.
     */
    private static Task ReceiveTask()
    {
        return Task.Run(async () =>
        {
            var buffer = new byte[1024];

            try
            {
                while (client?.State == WebSocketState.Open)
                {
                    var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        Console.WriteLine("Server requested close.");
                        break;
                    }

                    var response = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine("Received from server: " + response);
                    try
                    {
                        var jsonData = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                        if (jsonData != null)
                        {
                            foreach (var keyValue in jsonData)
                            {
                                if (keyValue.Key == "battery")
                                {
                                    BatteryService.Level = ((JsonElement)keyValue.Value).GetInt32();
                                }
                                else if (keyValue.Key == "label")
                                {
                                    string value = ((JsonElement)keyValue.Value).ToString();
                                    NotificationReceived?.Invoke(Notifications.Of(value));
                                    if (Preferences.Get(SettingsKeys.SafeModeEnabled, false))
                                    {
                                        SpeedLimitReceived?.Invoke(value);
                                    }
                                }
                                else if (keyValue.Key == "speed")
                                {
                                    SpeedInfoChanged?.Invoke(((JsonElement)keyValue.Value).GetInt32());
                                }
                            }
                        }
                    } catch (JsonException)
                    {
                        Console.WriteLine("Received invalid JSON data: " + response);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Receive error: " + ex.Message);
                SetState(WebsocketClientState.Disconnected);
                ConnectionInfoChanged?.Invoke("Unexpectedly disconnected from the car", Colors.Red);
            }
        });
    }

    private static Task SendControlData()
    {
        return Task.Run(async () =>
        {
            try
            {
                while (client?.State == WebSocketState.Open)
                {
                    // Only send control data if it has changed
                    if (controlData.Any(kv => !sentControlData.TryGetValue(kv.Key, out var val) || val != kv.Value))
                    {
                        var json = JsonSerializer.Serialize(controlData);
                        Send(json);
                        sentControlData = new Dictionary<string, int>(controlData);
                    }

                    await Task.Delay(10);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Send Control Data error: " + ex.Message);
                SetState(WebsocketClientState.Disconnected);
                ConnectionInfoChanged?.Invoke("Unexpectedly disconnected from the car", Colors.Red);
            }
        });
    }
}