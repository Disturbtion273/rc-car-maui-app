using rc_car_maui_app.Helpers;

namespace rc_car_maui_app.Views;

public partial class ManualDrivingPage : AbstractInteractiveDrivingPage
{
    public ManualDrivingPage() 
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (Preferences.Get(SettingsKeys.SafeModeEnabled, false))
        {
            Slider.Maximum = 70;
            Slider.Minimum = -70;
        }
    }
}