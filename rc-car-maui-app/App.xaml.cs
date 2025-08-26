using System.Globalization;  
using rc_car_maui_app.Helpers;

namespace rc_car_maui_app;

public partial class App : Application
{
    public App(IThemeService themeService)
    {
        InitializeComponent();
        themeService.ApplySaved();
        
        var code = Preferences.Get("AppLanguage", "en");
        Localization.Initialize(code);   // setzt Culture – noch ohne Event


    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}