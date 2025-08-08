using rc_car_maui_app.Views;

namespace rc_car_maui_app;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        CurrentItem = HomeTab;
        Routing.RegisterRoute(nameof(ManualDrivingPage), typeof(ManualDrivingPage));
    }
}