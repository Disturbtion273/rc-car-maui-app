using rc_car_maui_app.Websocket;

namespace rc_car_maui_app.Controls;

public partial class WebsocketIndicator : ContentView
{
    private readonly Dictionary<WebsocketClientState, Color> _websocketIndicatorColors = new()
    {
        { WebsocketClientState.Disconnected, Colors.Red },
        { WebsocketClientState.Connecting, Colors.DeepSkyBlue },
        { WebsocketClientState.Connected, (Color)Application.Current.Resources["Green"] },
        { WebsocketClientState.Disconnecting, Colors.Orange }
    };
    
    public WebsocketIndicator()
    {
        InitializeComponent();
        IndicatorBorder.BackgroundColor = _websocketIndicatorColors[WebsocketClient.GetState()];
        WebsocketClient.StateChanged += WebsocketClientOnStateChanged;
    }
    
    private void WebsocketClientOnStateChanged(WebsocketClientState state)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            IndicatorBorder.BackgroundColor = _websocketIndicatorColors[state];
        });
    }
}