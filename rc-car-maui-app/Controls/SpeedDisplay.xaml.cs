namespace rc_car_maui_app.Controls;

public partial class SpeedDisplay : ContentView
{
    public static readonly BindableProperty SpeedProperty =
        BindableProperty.Create(nameof(Speed), typeof(int), typeof(SpeedDisplay), 0, propertyChanged: OnSpeedChanged);


    public static readonly BindableProperty DirectionProperty =
        BindableProperty.Create(nameof(Direction), typeof(int), typeof(SpeedDisplay), 0, propertyChanged: OnDirectionChanged);

    public int Speed
    {
        get => (int)GetValue(SpeedProperty);
        set => SetValue(SpeedProperty, value);
    }
    
    public int Direction
    {
        get => (int)GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    public SpeedDisplay()
    {
        InitializeComponent();
    }
    private static void OnSpeedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SpeedDisplay control)
            control.SpeedLabel.Text = Math.Abs((int)newValue).ToString();
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
