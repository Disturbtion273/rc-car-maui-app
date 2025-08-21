namespace rc_car_maui_app.Controls.ToggleSwitch;

public class ToggleSwitch : GraphicsView
{
    #region Bindable Properties
        public static readonly BindableProperty OnTextProperty = BindableProperty.Create(
            nameof(OnText),
            typeof(string),
            typeof(ToggleSwitch),
            "Focused",
            propertyChanged: Redraw);

        public string OnText { get => (string)GetValue(OnTextProperty); set => SetValue(OnTextProperty, value); }

        public static readonly BindableProperty OffTextProperty = BindableProperty.Create(
            nameof(OffText),
            typeof(string),
            typeof(ToggleSwitch),
            "Other",
            propertyChanged: Redraw);

        public string OffText { get => (string)GetValue(OffTextProperty); set => SetValue(OffTextProperty, value); }

        public static readonly BindableProperty IsToggledProperty = BindableProperty.Create(
            nameof(IsToggled),
            typeof(bool),
            typeof(ToggleSwitch),
            false,
            propertyChanged: (b, o, n) => ((ToggleSwitch)b).AnimateToggle((bool)n));

        public bool IsToggled { get => (bool)GetValue(IsToggledProperty); set => SetValue(IsToggledProperty, value); }

        public static readonly BindableProperty SelectedTextColorProperty = BindableProperty.Create(
            nameof(SelectedTextColor),
            typeof(Color),
            typeof(ToggleSwitch),
            Color.FromArgb("#004D47"),
            propertyChanged: Redraw);

        public Color SelectedTextColor { get => (Color)GetValue(SelectedTextColorProperty); set => SetValue(SelectedTextColorProperty, value); }

        public static readonly BindableProperty UnselectedTextColorProperty = BindableProperty.Create(
            nameof(UnselectedTextColor),
            typeof(Color),
            typeof(ToggleSwitch),
            Color.FromArgb("#004D47"),
            propertyChanged: Redraw);

        public Color UnselectedTextColor { get => (Color)GetValue(UnselectedTextColorProperty); set => SetValue(UnselectedTextColorProperty, value); }

        public static readonly BindableProperty PillColorProperty = BindableProperty.Create(
            nameof(PillColor),
            typeof(Color),
            typeof(ToggleSwitch),
            Color.FromArgb("#05bfb6"),
            propertyChanged: Redraw);

        public Color PillColor { get => (Color)GetValue(PillColorProperty); set => SetValue(PillColorProperty, value); }

        public static readonly BindableProperty PillBackgroundColorProperty = BindableProperty.Create(
            nameof(PillBackgroundColor),
            typeof(Color),
            typeof(ToggleSwitch),
            Color.FromArgb("#002c28"),
            propertyChanged: Redraw);

        public Color PillBackgroundColor { get => (Color)GetValue(PillBackgroundColorProperty); set => SetValue(PillBackgroundColorProperty, value); }

        public static readonly BindableProperty PaddingProperty = BindableProperty.Create(
            nameof(Padding),
            typeof(float),
            typeof(ToggleSwitch),
            12f,
            propertyChanged: Redraw);

        public float Padding { get => (float)GetValue(PaddingProperty); set => SetValue(PaddingProperty, value); }

        public event EventHandler<bool> ToggleChanged;
        #endregion

        // animation progress 0..1 (0 = Off, 1 = On)
        internal float animationProgress = 0f;

        // Drawable reference (so we can Invalidate easily)
        readonly ToggleSwitchDrawable _drawable;

        public ToggleSwitch()
        {
            // assign the drawable painter
            _drawable = new ToggleSwitchDrawable(this);
            Drawable = _drawable;

            // enable touch events
            StartInteraction += OnStartInteraction;

            // sensible defaults (can be overridden in XAML)
            WidthRequest = 300;
            HeightRequest = 64;
        }

        private void OnStartInteraction(object sender, TouchEventArgs e)
        {
            // on any tap/press toggle state
            if (e.Touches?.Length > 0)
            {
                IsToggled = !IsToggled;
                ToggleChanged?.Invoke(this, IsToggled);

                // AnimateToggle will be called by the property changed handler (we call it also just in case)
                AnimateToggle(IsToggled);
            }
        }

        private static void Redraw(BindableObject bindable, object oldVal, object newVal)
        {
            ((ToggleSwitch)bindable).Invalidate();
        }

        public void AnimateToggle(bool newState)
        {
            var start = animationProgress;
            var end = newState ? 1f : 0f;

            var anim = new Animation(v =>
            {
                animationProgress = (float)v;
                Invalidate(); // redraw GraphicsView
            }, start, end);

            anim.Commit(this, "ToggleAnim", length: 200, easing: Easing.SinInOut, finished: (v, c) =>
            {
                // ensure final value
                animationProgress = end;
                Invalidate();
            });
        }
}