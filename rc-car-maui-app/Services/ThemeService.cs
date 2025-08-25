using Microsoft.Maui.Storage;

public sealed class ThemeService : IThemeService
{
    private const string ThemeKey = "app_theme";

    public ThemeSetting GetSaved()
    {
        var raw = Preferences.Get(ThemeKey, "Light");
        return raw == "Dark" ? ThemeSetting.Dark : ThemeSetting.Light;
    }

    public void Apply(ThemeSetting setting)
    {
        Application.Current!.UserAppTheme =
            setting == ThemeSetting.Dark ? AppTheme.Dark : AppTheme.Light;
    }

    public void Save(ThemeSetting setting)
    {
        Preferences.Set(ThemeKey, setting == ThemeSetting.Dark ? "Dark" : "Light");
    }

    public void ApplySaved() => Apply(GetSaved());
}