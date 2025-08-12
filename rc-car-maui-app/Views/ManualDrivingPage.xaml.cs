using System.Text.Json;
using rc_car_maui_app.Services;
using rc_car_maui_app.Websocket;

namespace rc_car_maui_app.Views;

public partial class ManualDrivingPage
{
    private readonly IDeviceOrientationService _orientationService;
    private readonly GyroscopeService _gyroscopeService;

    public ManualDrivingPage()
    {
        InitializeComponent();
        _gyroscopeService = new GyroscopeService();
        _orientationService = DependencyService.Get<IDeviceOrientationService>();
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
        if (sender is Slider slider) slider.Value = 0;
    }
}