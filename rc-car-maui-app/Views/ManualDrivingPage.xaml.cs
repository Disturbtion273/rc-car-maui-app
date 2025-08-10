using rc_car_maui_app.Services;

namespace rc_car_maui_app.Views;

public partial class ManualDrivingPage : ContentPage
{
    private readonly IDeviceOrientationService _orientationService;

    public ManualDrivingPage()
    {
        InitializeComponent();
        _orientationService = Application.Current.Windows[0].Page.Handler.MauiContext.Services
            .GetService<IDeviceOrientationService>();
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
        _orientationService.SetLandscape();
    }

    protected override void OnDisappearing()
    {
        _orientationService.SetPortrait();
        base.OnDisappearing();
    }

    private void Slider_OnValueChanged(object? sender, ValueChangedEventArgs e)
    {
        Console.WriteLine(e.NewValue);
    }

    private void Slider_OnDragCompleted(object? sender, EventArgs e)
    {
        if (sender is Slider slider) slider.Value = 0;
    }
}