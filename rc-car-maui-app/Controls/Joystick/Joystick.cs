namespace rc_car_maui_app.Controls.Joystick;

public class Joystick : GraphicsView
{
    public event EventHandler<ValueChangedEventArgs>? ValueXChanged;
    public event EventHandler<ValueChangedEventArgs>? ValueYChanged;
    public event EventHandler? DragCompleted;

    private static readonly BindableProperty ValueXProperty =
        BindableProperty.Create(nameof(ValueX), typeof(double), typeof(Joystick), 0.0,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var joystick = (Joystick)bindable;
                joystick.Invalidate();
                joystick.ValueXChanged?.Invoke(joystick, new ValueChangedEventArgs((double)oldValue, (double)newValue));
            });

    private static readonly BindableProperty ValueYProperty =
        BindableProperty.Create(nameof(ValueY), typeof(double), typeof(Joystick), 0.0,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var joystick = (Joystick)bindable;
                joystick.Invalidate();
                joystick.ValueYChanged?.Invoke(joystick, new ValueChangedEventArgs((double)oldValue, (double)newValue));
            });

    private static readonly BindableProperty BaseWidthProperty =
        BindableProperty.Create(nameof(BaseWidth), typeof(double), typeof(Joystick), 150.0);

    private static readonly BindableProperty BaseHeightProperty =
        BindableProperty.Create(nameof(BaseHeight), typeof(double), typeof(Joystick), 150.0);

    private static readonly BindableProperty ThumbWidthProperty =
        BindableProperty.Create(nameof(ThumbWidth), typeof(double), typeof(Joystick), 50.0);

    private static readonly BindableProperty ThumbHeightProperty =
        BindableProperty.Create(nameof(ThumbHeight), typeof(double), typeof(Joystick), 50.0);

    public double ValueX
    {
        get => (double)GetValue(ValueXProperty);
        set => SetValue(ValueXProperty, value);
    }

    public double ValueY
    {
        get => (double)GetValue(ValueYProperty);
        set => SetValue(ValueYProperty, value);
    }

    public double BaseWidth
    {
        get => (double)GetValue(BaseWidthProperty);
        init => SetValue(BaseWidthProperty, value);
    }

    public double BaseHeight
    {
        get => (double)GetValue(BaseHeightProperty);
        init => SetValue(BaseHeightProperty, value);
    }

    public double ThumbWidth
    {
        get => (double)GetValue(ThumbWidthProperty);
        init => SetValue(ThumbWidthProperty, value);
    }

    public double ThumbHeight
    {
        get => (double)GetValue(ThumbHeightProperty);
        init => SetValue(ThumbHeightProperty, value);
    }

    public Joystick()
    {
        Drawable = new JoystickDrawable(this);
        WidthRequest = BaseWidth + ThumbWidth;
        HeightRequest = BaseHeight + ThumbHeight;

        StartInteraction += OnTouch;
        DragInteraction += OnTouch;
        EndInteraction += OnDragCompleted;
    }

    private void OnTouch(object? sender, TouchEventArgs e)
    {
        if (e.Touches.Length == 0) return;
        var touch = e.Touches[0];

        var cx = BaseWidth / 2.0;
        var cy = BaseHeight / 2.0;
        var dx = touch.X - ThumbWidth / 2 - cx;
        var dy = touch.Y - ThumbHeight / 2 - cy;

        var dist = Math.Sqrt(dx * dx + dy * dy);
        var radius = Math.Min(BaseWidth, BaseHeight) / 2.0;

        if (dist > radius)
        {
            var scale = radius / dist;
            dx *= scale;
            dy *= scale;
        }

        var x = cx + dx;
        var y = cy + dy;

        var valueX = (float)(x / BaseWidth) * 2 - 1;
        var valueY = (float)(y / BaseHeight) * 2 - 1;

        ValueX = valueX;
        ValueY = valueY;
    }

    private void OnDragCompleted(object? sender, TouchEventArgs e)
    {
        DragCompleted?.Invoke(sender, EventArgs.Empty);
    }
}