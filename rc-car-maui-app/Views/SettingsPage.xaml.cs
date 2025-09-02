using rc_car_maui_app.Controls.CustomPicker;
using Microsoft.Maui.Storage;
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
        if (e.Value)
        {
            Console.WriteLine("Unit: KM/H");
        }
        else
        {
            Console.WriteLine("Unit: MP/H");
        }
    }
    
    private void OnBatteryToggled(object sender, ToggledEventArgs e)
    {
        if (_initBattery) return;
        Preferences.Set(SettingsKeys.ShowBatteryIndicator, e.Value);
        MessagingCenter.Send<object, bool>(this, SettingsKeys.ShowBatteryIndicatorChanged, e.Value);
    }
    
    private void OnSafeModeToggled(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            Console.WriteLine("Safe mode ON");
        }
        else
        {
            Console.WriteLine("Safe mode OFF");
        }
    }
    
    private void SelectedLanguageChanged(object sender, EventArgs e)
    {
        var picker = (CustomPicker)sender;
        string lang = picker.Items[picker.SelectedIndex];

        switch (lang)
        {
            case "DE":
                Console.WriteLine("German selected");
                break;
            case "EN":
                Console.WriteLine("English selected");
                break;
            case "SP":
                Console.WriteLine("Spanish selected");
                break;
        }
    }
    
    private void OnDarkModeToggled(object sender, ToggledEventArgs e)
    {
        if (_initializing) return;
        var setting = e.Value ? ThemeSetting.Dark : ThemeSetting.Light;
        _theme.Apply(setting);   // sofort umschalten
        _theme.Save(setting);    // speichern
    }
}