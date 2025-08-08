using rc_car_maui_app.Services;
using rc_car_maui_app.Views;

namespace rc_car_maui_app;

public partial class AppShell : Shell
{
    private readonly IDeviceOrientationService _orientationService;
    public AppShell()
    {
        InitializeComponent();
        CurrentItem = HomeTab;
        Routing.RegisterRoute(nameof(ManualDrivingPage), typeof(ManualDrivingPage));
        _orientationService = new DeviceOrientationService();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _orientationService.SetPortrait();
    }
}