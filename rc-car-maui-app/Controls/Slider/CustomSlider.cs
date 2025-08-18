namespace rc_car_maui_app.Controls.Slider;

public class CustomSlider : GraphicsView
{
    public event EventHandler<ValueChangedEventArgs> ValueChanged;
    public event EventHandler DragCompleted;
    
    public static readonly BindableProperty MinimumProperty =
        BindableProperty.Create(nameof(Minimum), typeof(double), typeof(CustomSlider), 0.0);

    public static readonly BindableProperty MaximumProperty =
        BindableProperty.Create(nameof(Maximum), typeof(double), typeof(CustomSlider), 100.0);

    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(nameof(Value), typeof(double), typeof(CustomSlider), 0.0,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var slider = (CustomSlider)bindable;
                slider.Invalidate();
                slider.ValueChanged.Invoke(slider, new ValueChangedEventArgs((double)oldValue, (double)newValue));
            });

    public static readonly BindableProperty TrackWidthProperty =
        BindableProperty.Create(nameof(TrackWidth), typeof(double), typeof(CustomSlider), 20.0);

    public static readonly BindableProperty TrackHeightProperty =
        BindableProperty.Create(nameof(TrackHeight), typeof(double), typeof(CustomSlider), 150.0);

    public static readonly BindableProperty ThumbWidthProperty =
        BindableProperty.Create(nameof(ThumbWidth), typeof(double), typeof(CustomSlider), 46.0);

    public static readonly BindableProperty ThumbHeightProperty =
        BindableProperty.Create(nameof(ThumbHeight), typeof(double), typeof(CustomSlider), 16.0);

    public double Minimum
    {
        get => (double)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public double Maximum
    {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public double TrackWidth
    {
        get => (double)GetValue(TrackWidthProperty);
        set => SetValue(TrackWidthProperty, value);
    }

    public double TrackHeight
    {
        get => (double)GetValue(TrackHeightProperty);
        set => SetValue(TrackHeightProperty, value);
    }

    public double ThumbWidth
    {
        get => (double)GetValue(ThumbWidthProperty);
        set => SetValue(ThumbWidthProperty, value);
    }

    public double ThumbHeight
    {
        get => (double)GetValue(ThumbHeightProperty);
        set => SetValue(ThumbHeightProperty, value);
    }

    public CustomSlider()
    {
        Drawable = new CustomSliderDrawable(this);
        WidthRequest = ThumbWidth;
        HeightRequest = TrackHeight + ThumbHeight;
        
        StartInteraction += OnTouch;
        DragInteraction += OnTouch;
        EndInteraction += OnDragCompleted;
    }

    private void OnTouch(object sender, TouchEventArgs e)
    {
        if (e.Touches.Length > 0)
        {
            var touch = e.Touches[0];
            var y = Math.Clamp(touch.Y, 0 + ThumbHeight / 2, TrackHeight + ThumbHeight / 2) - ThumbHeight / 2;
            var percent = 1.0 - y / TrackHeight; 
            Value = Minimum + percent * (Maximum - Minimum);
        }
    }

    private void OnDragCompleted(object sender, TouchEventArgs e)
    {
        DragCompleted.Invoke(sender, EventArgs.Empty);
    }
}