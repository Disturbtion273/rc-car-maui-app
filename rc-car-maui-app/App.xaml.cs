using rc_car_maui_app.Websocket;

namespace rc_car_maui_app;

public partial class App : Application {
    public App() {
        InitializeComponent();
    }

    protected override void OnStart()
    {
        base.OnStart();
        _ = ConnectWebSocket();
    }

    private static async Task ConnectWebSocket()
    {
        for (var counter = 1; counter <= 5 && !WebsocketClient.IsConnected(); counter++)
        {
            try
            {
                Console.WriteLine($"Attempting to connect to WebSocket server... Attempt {counter}");
                await WebsocketClient.Connect("ws://192.168.178.31:9999");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebSocket connection error: {ex.Message}");
            }
        }
    }

    protected override Window CreateWindow(IActivationState? activationState) {
        return new Window(new AppShell());
    }
}