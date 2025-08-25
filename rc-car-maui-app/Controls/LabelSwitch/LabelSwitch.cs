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
        public static readonly BindableProperty UnselectedTextProperty =
            BindableProperty.Create(nameof(UnselectedText), typeof(string), typeof(LabelSwitch), "KM/H", propertyChanged: OnAppearanceChanged);
        public string UnselectedText { get => (string)GetValue(UnselectedTextProperty); set => SetValue(UnselectedTextProperty, value); }

        public static readonly BindableProperty SelectedTextProperty =
            BindableProperty.Create(nameof(SelectedText), typeof(string), typeof(LabelSwitch), "MP/H", propertyChanged: OnAppearanceChanged);
        public string SelectedText { get => (string)GetValue(SelectedTextProperty); set => SetValue(SelectedTextProperty, value); }

        // Colors
        public static readonly BindableProperty TrackColorProperty =
            BindableProperty.Create(nameof(TrackColor), typeof(Color), typeof(LabelSwitch), 
                Color.FromArgb("#A8000000"), propertyChanged: OnAppearanceChanged);
        public Color TrackColor { get => (Color)GetValue(TrackColorProperty); set => SetValue(TrackColorProperty, value); }

        public static readonly BindableProperty CapsuleColorProperty =
            BindableProperty.Create(nameof(CapsuleColor), typeof(Color), typeof(LabelSwitch), 
                Color.FromArgb("#03BFB5"), propertyChanged: OnAppearanceChanged);
        public Color CapsuleColor { get => (Color)GetValue(CapsuleColorProperty); set => SetValue(CapsuleColorProperty, value); }

        public static readonly BindableProperty UnselectedTextColorProperty =
            BindableProperty.Create(nameof(UnselectedTextColor), typeof(Color), typeof(LabelSwitch),
                Color.FromArgb("#4103BFB5"), propertyChanged: OnAppearanceChanged);
        public Color UnselectedTextColor { get => (Color)GetValue(UnselectedTextColorProperty); set => SetValue(UnselectedTextColorProperty, value); }

        public static readonly BindableProperty SelectedTextColorProperty =
            BindableProperty.Create(nameof(SelectedTextColor), typeof(Color), typeof(LabelSwitch),
                Color.FromArgb("#014D47"), propertyChanged: OnAppearanceChanged);
        public Color SelectedTextColor { get => (Color)GetValue(SelectedTextColorProperty); set => SetValue(SelectedTextColorProperty, value); }

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
            _drawable.UnselectedText = UnselectedText;
            _drawable.SelectedText = SelectedText;
            _drawable.TrackColor = TrackColor;
            _drawable.CapsuleColor = CapsuleColor;
            _drawable.SelectedTextColor = SelectedTextColor;
            _drawable.UnselectedTextColor = UnselectedTextColor;
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