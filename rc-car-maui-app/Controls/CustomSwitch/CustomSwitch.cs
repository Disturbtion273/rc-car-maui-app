namespace rc_car_maui_app.Controls.CustomSwitch;

public class CustomSwitch : GraphicsView
    {
        readonly CustomSwitchDrawable _drawable;

        // IsToggled bindable
        public static readonly BindableProperty IsToggledProperty =
            BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(CustomSwitch), false,
                BindingMode.TwoWay, propertyChanged: OnIsToggledChanged);

        public bool IsToggled
        {
            get => (bool)GetValue(IsToggledProperty);
            set => SetValue(IsToggledProperty, value);
        }

        // Appearance bindables (only the four you asked for)
        public static readonly BindableProperty TrackColorOnProperty =
            BindableProperty.Create(nameof(TrackColorOn), typeof(Color), typeof(CustomSwitch),
                Color.FromArgb("#002c28"), propertyChanged: OnAppearanceChanged);
        public Color TrackColorOn { get => (Color)GetValue(TrackColorOnProperty); set => SetValue(TrackColorOnProperty, value); }

        public static readonly BindableProperty TrackColorOffProperty =
            BindableProperty.Create(nameof(TrackColorOff), typeof(Color), typeof(CustomSwitch),
                Color.FromArgb("#00514a"), propertyChanged: OnAppearanceChanged);
        public Color TrackColorOff { get => (Color)GetValue(TrackColorOffProperty); set => SetValue(TrackColorOffProperty, value); }

        public static readonly BindableProperty ThumbColorOnProperty =
            BindableProperty.Create(nameof(ThumbColorOn), typeof(Color), typeof(CustomSwitch),
                Color.FromArgb("#05bfb5"), propertyChanged: OnAppearanceChanged);
        public Color ThumbColorOn { get => (Color)GetValue(ThumbColorOnProperty); set => SetValue(ThumbColorOnProperty, value); }

        public static readonly BindableProperty ThumbColorOffProperty =
            BindableProperty.Create(nameof(ThumbColorOff), typeof(Color), typeof(CustomSwitch),
                Color.FromArgb("#02988f"), propertyChanged: OnAppearanceChanged);
        public Color ThumbColorOff { get => (Color)GetValue(ThumbColorOffProperty); set => SetValue(ThumbColorOffProperty, value); }

        // Toggled event
        public event EventHandler<ToggledEventArgs> Toggled;

        bool _isAnimating;

        public CustomSwitch()
        {
            // sensible defaults; override in XAML if needed
            HeightRequest = 30;
            WidthRequest = 73;

            _drawable = new CustomSwitchDrawable(IsToggled);
            Drawable = _drawable;

            var tap = new TapGestureRecognizer();
            tap.Tapped += async (s, e) => await ToggleAsync(true);
            GestureRecognizers.Add(tap);

            UpdateDrawableFromProperties();
        }

        static void OnAppearanceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomSwitch cs)
            {
                cs.UpdateDrawableFromProperties();
                cs.Invalidate();
            }
        }

        static void OnIsToggledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomSwitch cs)
            {
                var newBool = (bool)newValue;
                _ = cs.AnimateToStateAsync(newBool, animate: true);
                cs.Toggled?.Invoke(cs, new ToggledEventArgs(newBool));
            }
        }

        void UpdateDrawableFromProperties()
        {
            _drawable.TrackColorOn = TrackColorOn;
            _drawable.TrackColorOff = TrackColorOff;
            _drawable.ThumbColorOn = ThumbColorOn;
            _drawable.ThumbColorOff = ThumbColorOff;
            _drawable.IsToggled = IsToggled;
            _drawable.AnimatedPosition = IsToggled ? 1f : 0f;
        }

        public Task ToggleAsync(bool animated = true)
        {
            IsToggled = !IsToggled;
            return Task.CompletedTask;
        }

        async Task AnimateToStateAsync(bool toOn, bool animate)
        {
            if (_isAnimating) return;

            _isAnimating = true;
            try
            {
                var start = _drawable.AnimatedPosition;
                var end = toOn ? 1f : 0f;

                if (!animate)
                {
                    _drawable.AnimatedPosition = end;
                    _drawable.IsToggled = toOn;
                    Invalidate();
                }
                else
                {
                    const int durationMs = 200;
                    var startTime = DateTime.UtcNow;
                    while (true)
                    {
                        var elapsed = (float)(DateTime.UtcNow - startTime).TotalMilliseconds;
                        var t = Math.Min(1f, elapsed / durationMs);
                        var eased = 1 - (float)Math.Pow(1 - t, 3); // ease-out
                        _drawable.AnimatedPosition = start + (end - start) * eased;
                        Invalidate();

                        if (t >= 1f) break;
                        await Task.Delay(16);
                    }

                    _drawable.AnimatedPosition = end;
                    _drawable.IsToggled = toOn;
                    Invalidate();
                }
            }
            finally
            {
                _isAnimating = false;
            }
        }
    }