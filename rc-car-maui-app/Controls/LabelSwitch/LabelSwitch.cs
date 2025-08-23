namespace rc_car_maui_app.Controls.LabelSwitch;

public class LabelSwitch : GraphicsView
{
    readonly LabelSwitchDrawable _drawable;

        // IsLeftSelected bindable (TwoWay)
        public static readonly BindableProperty IsLeftSelectedProperty =
            BindableProperty.Create(nameof(IsLeftSelected), typeof(bool), typeof(LabelSwitch), true,
                BindingMode.TwoWay, propertyChanged: OnIsSelectedChanged);

        public bool IsLeftSelected
        {
            get => (bool)GetValue(IsLeftSelectedProperty);
            set => SetValue(IsLeftSelectedProperty, value);
        }

        // Texts
        public static readonly BindableProperty LeftTextProperty =
            BindableProperty.Create(nameof(LeftText), typeof(string), typeof(LabelSwitch), "KM/H", propertyChanged: OnAppearanceChanged);
        public string LeftText { get => (string)GetValue(LeftTextProperty); set => SetValue(LeftTextProperty, value); }

        public static readonly BindableProperty RightTextProperty =
            BindableProperty.Create(nameof(RightText), typeof(string), typeof(LabelSwitch), "MP/H", propertyChanged: OnAppearanceChanged);
        public string RightText { get => (string)GetValue(RightTextProperty); set => SetValue(RightTextProperty, value); }

        // Colors
        public static readonly BindableProperty TrackColorProperty =
            BindableProperty.Create(nameof(TrackColor), typeof(Color), typeof(LabelSwitch), Color.FromArgb("#00514a"), propertyChanged: OnAppearanceChanged);
        public Color TrackColor { get => (Color)GetValue(TrackColorProperty); set => SetValue(TrackColorProperty, value); }

        public static readonly BindableProperty CapsuleColorProperty =
            BindableProperty.Create(nameof(CapsuleColor), typeof(Color), typeof(LabelSwitch), Color.FromArgb("#05bfb5"), propertyChanged: OnAppearanceChanged);
        public Color CapsuleColor { get => (Color)GetValue(CapsuleColorProperty); set => SetValue(CapsuleColorProperty, value); }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(LabelSwitch), Color.FromArgb("#002c28"), propertyChanged: OnAppearanceChanged);
        public Color TextColor { get => (Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }

        // Toggled event
        public event EventHandler<ToggledEventArgs> Toggled;

        bool _isAnimating;

        public LabelSwitch()
        {
            // default size: you can override in XAML
            HeightRequest = 36;
            WidthRequest = 180;

            _drawable = new LabelSwitchDrawable(IsLeftSelected);
            Drawable = _drawable;

            var tap = new TapGestureRecognizer();
            tap.Tapped += async (s, e) =>
            {
                // toggle selection on tap
                await ToggleAsync(true);
            };
            GestureRecognizers.Add(tap);

            UpdateDrawableFromProperties();
        }

        static void OnAppearanceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LabelSwitch ts)
            {
                ts.UpdateDrawableFromProperties();
                ts.Invalidate();
            }
        }

        static void OnIsSelectedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LabelSwitch ts)
            {
                var newBool = (bool)newValue;
                _ = ts.AnimateToStateAsync(newBool, animate: true);
                ts.Toggled?.Invoke(ts, new ToggledEventArgs(newBool));
            }
        }

        void UpdateDrawableFromProperties()
        {
            _drawable.LeftText = LeftText;
            _drawable.RightText = RightText;
            _drawable.TrackColor = TrackColor;
            _drawable.CapsuleColor = CapsuleColor;
            _drawable.TextColor = TextColor;
            _drawable.IsLeftSelected = IsLeftSelected;
            _drawable.AnimatedPosition = IsLeftSelected ? 0f : 1f;
        }

        public Task ToggleAsync(bool animated = true)
        {
            IsLeftSelected = !IsLeftSelected;
            return Task.CompletedTask;
        }

        async Task AnimateToStateAsync(bool leftSelected, bool animate)
        {
            if (_isAnimating) return;

            _isAnimating = true;
            try
            {
                var start = _drawable.AnimatedPosition;
                var end = leftSelected ? 0f : 1f;

                if (!animate)
                {
                    _drawable.AnimatedPosition = end;
                    _drawable.IsLeftSelected = leftSelected;
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
                    _drawable.IsLeftSelected = leftSelected;
                    Invalidate();
                }
            }
            finally
            {
                _isAnimating = false;
            }
        }
}