using rc_car_maui_app.Controls.CustomPicker;
using rc_car_maui_app.Helpers;

namespace rc_car_maui_app.Views;

public partial class SettingsPage : ContentPage
{

    private readonly IThemeService _theme;
    private bool _initializing;
    private bool _initBattery;
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        _initBattery = true;
        var enabled = Preferences.Get(SettingsKeys.ShowBatteryIndicator, true);
        BatterySwitch.IsToggled = enabled;
        _initBattery = false;
        
        LanguagePicker.SelectedIndex = Localization.CurrentCode switch
        {
            "de" => 1,
            "es" => 2,
            _    => 0
        };
        
    }
    public SettingsPage()
    {
        InitializeComponent();
        
        _theme = ServiceHelper.GetService<IThemeService>();

        // Switch auf gespeicherten Zustand setzen, Event dabei ignorieren
        _initializing = true;
        DarkModeSwitch.IsToggled = _theme.GetSaved() == ThemeSetting.Dark;
        _initializing = false;
    }
    
    private void OnUnitToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set(SettingsKeys.UseKmH, e.Value);
    }
    
    private void OnBatteryToggled(object sender, ToggledEventArgs e)
    {
        if (_initBattery) return;
        Preferences.Set(SettingsKeys.ShowBatteryIndicator, e.Value);
        MessagingCenter.Send<object, bool>(this, SettingsKeys.ShowBatteryIndicatorChanged, e.Value);
    }
    
    private void OnSafeModeToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set(SettingsKeys.SafeModeEnabled, e.Value);
    }
    
    private async void SelectedLanguageChanged(object sender, EventArgs e)
    {
        var picker = (CustomPicker)sender;
        var selected = picker.Items[picker.SelectedIndex]; // "DE", "EN", "ES/SP"

        var code = selected switch
        {
            "DE" => "de",
            "ES" or "SP" => "es",
            _            => "en"
        };

        Preferences.Set("AppLanguage", code);
        Localization.SetCulture(code);  

    }

    private void OnDarkModeToggled(object sender, ToggledEventArgs e)
    {
        if (_initializing) return;
        var setting = e.Value ? ThemeSetting.Dark : ThemeSetting.Light;
        _theme.Apply(setting);   // sofort umschalten
        _theme.Save(setting);    // speichern
    }
}