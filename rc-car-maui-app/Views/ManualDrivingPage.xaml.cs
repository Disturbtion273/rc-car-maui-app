using System.Text.Json;
using rc_car_maui_app.Controls.Joystick;
using rc_car_maui_app.Controls.Slider;
using rc_car_maui_app.Services;
using rc_car_maui_app.Websocket;
#if ANDROID
using AndroidX.Core.View;
#endif

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
        { WebsocketClientState.Connected, (Color)Application.Current.Resources["Green"] },
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
        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            MainGrid.Padding = new Thickness(50, 0);
        }
    }

    private void WebsocketClientOnStateChanged(WebsocketClientState state)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            WebsocketIndicator.BackgroundColor = _websocketIndicatorColors[state];
        });
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
        var oldSpeed = (int)e.OldValue;
        var speed = (int)e.NewValue;
        SpeedLabel.Text = Math.Abs(speed).ToString();
        UpdateUi(oldSpeed, speed);
    }

    private async void UpdateUi(int oldSpeed, int speed)
    {
        if (oldSpeed == 0 && speed > 0)
        {
            ArrowImage.Source = "arrow";
            ArrowImage.Rotation = 0;
        }
        else if (oldSpeed == 0 && speed < 0)
        {
            ArrowImage.Source = "arrow";
            ArrowImage.Rotation = 180;
        }
        else if (oldSpeed != 0 && speed == 0)
        {
            ArrowImage.Source = "minus";
            ArrowImage.Rotation = 0;
        }
        else if (oldSpeed > 0 && speed < 0)
        {
            await ArrowImage.RotateTo(180, 250, Easing.CubicInOut);
        }
        else if (oldSpeed < 0 && speed > 0)
        {
            await ArrowImage.RotateTo(0, 250, Easing.CubicInOut);
        }
    }

    private void Slider_OnDragCompleted(object? sender, EventArgs e)
    {
        if (sender is CustomSlider slider) slider.Value = 0;
    }

    private void Joystick_OnValueXChanged(object? sender, ValueChangedEventArgs e)
    {
        var tilt = e.NewValue > 0 ? 1 : e.NewValue < 0 ? -1 : 0;
        var speed = tilt == 0 ? 0 : Math.Abs(Math.Round(e.NewValue, 2)) * 100;

        WebsocketClient.SetControlData("tilt", tilt);
        WebsocketClient.SetControlData("tiltSpeed", speed);
    }

    private void Joystick_OnValueYChanged(object? sender, ValueChangedEventArgs e)
    {
        var pan = e.NewValue > 0 ? -1 : e.NewValue < 0 ? 1 : 0;
        var speed = pan == 0 ? 0 : Math.Abs(Math.Round(e.NewValue, 2)) * 100;

        WebsocketClient.SetControlData("pan", pan);
        WebsocketClient.SetControlData("panSpeed", speed);
    }

    private void Joystick_OnDragCompleted(object? sender, EventArgs e)
    {
        if (sender is not Joystick joystick) return;
        joystick.ValueX = 0;
        joystick.ValueY = 0;
    }

    private void CameraResetButton_OnClicked(object? sender, EventArgs e)
    {
        WebsocketClient.Send(JsonSerializer.Serialize(new { cameraReset = true }));
    }
}