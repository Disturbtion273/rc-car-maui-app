namespace rc_car_maui_app.Controls.CustomPicker;

public class CustomPicker : GraphicsView
{
    
    readonly CustomPickerDrawable _drawable;

    // Items
    public static readonly BindableProperty ItemsProperty =
        BindableProperty.Create(nameof(Items), typeof(IList<string>), typeof(CustomPicker),
            new List<string> { "EN", "DE", "SP" }, propertyChanged: OnAppearanceChanged);

    public IList<string> Items
    {
        get => (IList<string>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }
    
    // Selected index
    public static readonly BindableProperty SelectedIndexProperty =
        BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(CustomPicker), 0,
            BindingMode.TwoWay, propertyChanged: OnSelectedIndexChanged);

    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }
    
    // Appearance bindables
    public static readonly BindableProperty TrackColorProperty =
        BindableProperty.Create(nameof(TrackColor), typeof(Color), typeof(CustomPicker),
            Color.FromArgb("#A8000000"), propertyChanged: OnAppearanceChanged);
    public Color TrackColor { get => (Color)GetValue(TrackColorProperty); set => SetValue(TrackColorProperty, value); }

    public static readonly BindableProperty PickerIconColorProperty =
        BindableProperty.Create(nameof(PickerIconColor), typeof(Color), typeof(CustomPicker),
            Color.FromArgb("#03BFB5"), propertyChanged: OnAppearanceChanged);
    public Color PickerIconColor { get => (Color)GetValue(PickerIconColorProperty); set => SetValue(PickerIconColorProperty, value); }
    
    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CustomPicker),
            Color.FromArgb("#03BFB5"), propertyChanged: OnAppearanceChanged);
    public Color TextColor { get => (Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }

    public event EventHandler SelectedIndexChanged;
    
    public CustomPicker()
    {
        // sensible defaults; override in XAML if needed
        HeightRequest = 30;
        WidthRequest = 73;

        _drawable = new CustomPickerDrawable();
        Drawable = _drawable;

        var tap = new TapGestureRecognizer();
        tap.Tapped += async (s, e) => await OnTappedAsync();
        GestureRecognizers.Add(tap);

        UpdateDrawableFromProperties();
    }
    
    static void OnAppearanceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CustomPicker cs)
        {
            cs.UpdateDrawableFromProperties();
            cs.Invalidate();
        }
    }
    
    static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CustomPicker cp)
        {
            cp.UpdateDrawableFromProperties();
            cp.Invalidate();
            cp.SelectedIndexChanged?.Invoke(cp, EventArgs.Empty);
        }
    }
    
    void UpdateDrawableFromProperties()
    {
        _drawable.SelectedText = (Items != null && Items.Count > 0 && SelectedIndex >= 0 && SelectedIndex < Items.Count) ? Items[SelectedIndex] : "";
        _drawable.TrackColor = TrackColor;
        _drawable.PickerIconColor = PickerIconColor;
        _drawable.TextColor = TextColor;
        _drawable.IsOpen = false;
        Invalidate();
    }
    
    async Task OnTappedAsync()
    {
        // show dropdown modal page
        _drawable.IsOpen = true;
        Invalidate();

        var dropdown = new PickerDropdownPage(Items, SelectedIndex, OnItemPicked);
        // show as modal with transparent Track
        await Application.Current.MainPage.Navigation.PushModalAsync(dropdown, animated: true);

        // after returning ensure drawable state closed
        _drawable.IsOpen = false;
        Invalidate();
    }

    // callback when item selected
    void OnItemPicked(int selectedIndex)
    {
        SelectedIndex = selectedIndex;
    }

}