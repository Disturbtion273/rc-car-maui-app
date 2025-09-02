using rc_car_maui_app.Services;
using rc_car_maui_app.Helpers;
using rc_car_maui_app.Services;

namespace rc_car_maui_app.Controls;

public partial class BatteryIndicator : ContentView
{
    public BatteryIndicator()
    {
        InitializeComponent();
        UpdateVisibilityFromPreferences();
        UpdateBatteryUi();
        
        BatteryService.BatteryLevelChanged += UpdateBatteryUi;
        
        MessagingCenter.Subscribe<object, bool>(this,
            SettingsKeys.ShowBatteryIndicatorChanged,
            (sender, value) => { IsVisible = value; });
    }
    protected override void OnParentSet()
    {
        base.OnParentSet();
        // Jedes Mal wenn das Control in eine Seite kommt, sicherstellen,
        // dass die Sichtbarkeit mit den Preferences übereinstimmt
        UpdateVisibilityFromPreferences();
    }
    private void UpdateVisibilityFromPreferences()
    {
        // DEFAULT = true -> BatteryIndicator standardmäßig anzeigen
        var enabled = Preferences.Get(SettingsKeys.ShowBatteryIndicator, true);
        IsVisible = enabled;
    }
    private void UpdateBatteryUi()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            BatteryLabel.Text = $"{BatteryService.Level}%";
            BatteryBackground.WidthRequest = (double)BatteryService.Level * 16 / 100;
        });
    }
    
    ~BatteryIndicator()
    {
        // Aufräumen
        MessagingCenter.Unsubscribe<object, bool>(this, SettingsKeys.ShowBatteryIndicatorChanged);
        BatteryService.BatteryLevelChanged -= UpdateBatteryUi;
    }
}