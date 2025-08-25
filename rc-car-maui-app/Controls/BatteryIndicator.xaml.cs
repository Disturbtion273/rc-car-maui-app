using rc_car_maui_app.Services;

namespace rc_car_maui_app.Controls;

public partial class BatteryIndicator : ContentView
{
    public BatteryIndicator()
    {
        InitializeComponent();
        UpdateBatteryUi();
        BatteryService.BatteryLevelChanged += UpdateBatteryUi;
    }

    private void UpdateBatteryUi()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            BatteryLabel.Text = $"{BatteryService.Level}%";
            BatteryBackground.WidthRequest = (double)BatteryService.Level * 16 / 100;
        });
    }
}