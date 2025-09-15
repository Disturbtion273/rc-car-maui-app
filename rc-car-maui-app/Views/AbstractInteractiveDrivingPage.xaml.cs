using rc_car_maui_app.Controls;
using rc_car_maui_app.Controls.Joystick;
using rc_car_maui_app.Controls.Slider;
using rc_car_maui_app.Services;
using rc_car_maui_app.Websocket;

namespace rc_car_maui_app.Views;

public abstract partial class AbstractInteractiveDrivingPage : AbstractDrivingPage
{
    private readonly GyroscopeService _gyroscopeService;
    protected CustomSlider Slider;
    
    protected AbstractInteractiveDrivingPage()
    {
        InitializeComponent();
        _gyroscopeService = new GyroscopeService();
        AddControls();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _gyroscopeService.StartGyroscope();
    }
    
    protected override void OnDisappearing()
    {
        _gyroscopeService.StopGyroscope();
        base.OnDisappearing();
    }

    private void AddControls()
    {
        AddToTopLeftLayout(new CameraResetButton());
        
        Slider = new CustomSlider();
        Slider.Minimum = -100;
        Slider.Maximum = 100;
        Slider.ValueChanged += Slider_OnValueChanged;
        Slider.DragCompleted += Slider_OnDragCompleted;
        AddToGrid(Slider, LayoutOptions.End, LayoutOptions.End, new Thickness(80, 20));
        
        Joystick joystick = new Joystick();
        joystick.ValueXChanged += Joystick_OnValueXChanged;
        joystick.ValueYChanged += Joystick_OnValueYChanged;
        joystick.DragCompleted += Joystick_OnDragCompleted;
        AddToGrid(joystick, LayoutOptions.Start, LayoutOptions.End, new Thickness(10));
    }
    
    private void Slider_OnValueChanged(object? sender, ValueChangedEventArgs e)
    {
        WebsocketClient.SetControlData("speed", e.NewValue);
        SpeedDisplayControl.UpdateControls((int)e.NewValue);
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