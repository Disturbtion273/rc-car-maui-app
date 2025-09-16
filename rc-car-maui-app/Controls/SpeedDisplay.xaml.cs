using rc_car_maui_app.Helpers;

namespace rc_car_maui_app.Controls;

public partial class SpeedDisplay : ContentView
{
    private static readonly BindableProperty SpeedProperty =
        BindableProperty.Create(nameof(Speed), typeof(int), typeof(SpeedDisplay), 0, propertyChanged: OnSpeedChanged);


    private static readonly BindableProperty DirectionProperty =
        BindableProperty.Create(nameof(Direction), typeof(int), typeof(SpeedDisplay), 0, propertyChanged: OnDirectionChanged);

    private int Speed
    {
        get => (int)GetValue(SpeedProperty);
        set => SetValue(SpeedProperty, value);
    }
    
    private int Direction
    {
        get => (int)GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    public SpeedDisplay()
    {
        InitializeComponent();
        bool useMpH = Preferences.Get(SettingsKeys.UseMpH, false);
        Console.WriteLine(useMpH);
        UnitLabel.Text = useMpH ? "mp/h" : "km/h";

    }
    
    public void UpdateControls(int speed)
    {
        Speed = speed;
        Direction = speed == 0 ? 0 : speed > 0 ? 1 : -1;
    }
    
    private static void OnSpeedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SpeedDisplay control)
        {
            int value = Math.Abs((int)newValue);
            if (Preferences.Get(SettingsKeys.UseMpH, false))
            {
                value = (int)Math.Round(value * 0.621371);
            }
            control.SpeedLabel.Text = value.ToString();
        }
    }
    
    private static async void OnDirectionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not SpeedDisplay control) return;

        int oldDir = (int)oldValue;
        int newDir = (int)newValue;

        if (newDir == 0)
        {
            control.ArrowImage.Source = "minus";
            control.ArrowImage.Rotation = 0;
        }
        else
        {
            control.ArrowImage.Source = "arrow";
            if (oldDir != newDir)
            {
                double targetRotation = newDir == 1 ? 0 : 180;
                await control.ArrowImage.RotateTo(targetRotation, 250, Easing.CubicInOut);
            }
        }
    }
}
