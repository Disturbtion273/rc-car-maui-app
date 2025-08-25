namespace rc_car_maui_app.Services;

public static class BatteryService
{
    public static event Action? BatteryLevelChanged; 
    
    private static int _level = 100;
    
    public static int Level
    {
        get => _level;
        set
        {
            _level = value;
            BatteryLevelChanged?.Invoke();
        }
    }
}