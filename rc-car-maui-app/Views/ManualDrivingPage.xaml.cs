using rc_car_maui_app.Controls.Slider;
using rc_car_maui_app.Services;
using rc_car_maui_app.Websocket;

namespace rc_car_maui_app.Views;

public partial class ManualDrivingPage
{
    public string WebViewUrl { get; }

    private readonly IDeviceOrientationService _orientationService;
    private readonly GyroscopeService _gyroscopeService;
    private readonly Dictionary<WebsocketClientState, Color> _websocketIndicatorColors = new()
    {
        { WebsocketClientState.Disconnected, Colors.Red },
        { WebsocketClientState.Connecting, Colors.DeepSkyBlue },
        { WebsocketClientState.Connected, (Color) Application.Current.Resources["Green"] },
        { WebsocketClientState.Disconnecting, Colors.Orange }
    };

    public ManualDrivingPage()
    {
        InitializeComponent();
        _gyroscopeService = new GyroscopeService();
        _orientationService = DependencyService.Get<IDeviceOrientationService>();
        WebViewUrl = $"http://{WebsocketClient.GetHost()}:8080";
        BindingContext = this;
        WebsocketClient.StateChanged += WebsocketClientOnStateChanged;
        WebsocketIndicator.BackgroundColor = _websocketIndicatorColors[WebsocketClient.GetState()];
    }

    private void WebsocketClientOnStateChanged(WebsocketClientState state)
    {
        WebsocketIndicator.BackgroundColor = _websocketIndicatorColors[state];
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
        _orientationService.SetLandscape();
        _gyroscopeService.StartGyroscope();
    }

    protected override void OnDisappearing()
    {
        _orientationService.SetPortrait();
        _gyroscopeService.StopGyroscope();
        base.OnDisappearing();
    }

    private void Slider_OnValueChanged(object? sender, ValueChangedEventArgs e)
    {
        WebsocketClient.SetControlData("speed", e.NewValue);
    }

    private void Slider_OnDragCompleted(object? sender, EventArgs e)
    {
        if (sender is CustomSlider slider) slider.Value = 0;
    }
}