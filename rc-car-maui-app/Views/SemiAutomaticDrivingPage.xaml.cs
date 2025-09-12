using rc_car_maui_app.Controls;
using rc_car_maui_app.Controls.Joystick;
using rc_car_maui_app.Controls.Slider;
using rc_car_maui_app.Services;
using rc_car_maui_app.Websocket;

namespace rc_car_maui_app.Views;

public partial class SemiAutomaticDrivingPage : ContentPage
{
    public string WebViewUrl { get; }

    private readonly IDeviceOrientationService _orientationService;
    private readonly GyroscopeService _gyroscopeService;

    public SemiAutomaticDrivingPage()
    {
        InitializeComponent();
        _gyroscopeService = new GyroscopeService();
        _orientationService = DependencyService.Get<IDeviceOrientationService>();
        WebViewUrl = $"http://{WebsocketClient.GetHost()}:8080";
        BindingContext = this;
        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            MainGrid.Padding = new Thickness(50, 0);
        }
        WebsocketClient.NotificationReceived += WebsocketClientOnNotificationReceived;
        WebsocketClient.SpeedLimitReceived += WebsocketClientOnSpeedLimitReceived;
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
    
    private void WebsocketClientOnNotificationReceived(Notification? notification)
    {
        if (notification != null)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await notification.Show(MainGrid);
            });
        }
    }
    
    private void WebsocketClientOnSpeedLimitReceived(string value)
    {
        if (value == "dreissig")
        {
            Slider.Maximum = 30;
            Slider.Minimum = -30;
        } else if (value == "fuenfzig")
        {
            Slider.Maximum = 50;
            Slider.Minimum = -50;
        }
    }

    private void Slider_OnValueChanged(object? sender, ValueChangedEventArgs e)
    {
        WebsocketClient.SetControlData("speed", e.NewValue);
        var speed = (int)e.NewValue;
        SpeedDisplayName.Speed = speed;
        SpeedDisplayName.Direction = speed == 0 ? 0 : (speed > 0 ? 1 : -1);
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
}