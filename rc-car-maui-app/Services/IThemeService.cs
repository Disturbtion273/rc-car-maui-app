public enum ThemeSetting { Light, Dark }

public interface IThemeService
{
    ThemeSetting GetSaved();
    void Apply(ThemeSetting setting);
    void Save(ThemeSetting setting);
    void ApplySaved();
}