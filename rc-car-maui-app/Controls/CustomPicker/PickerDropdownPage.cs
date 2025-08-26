namespace rc_car_maui_app.Controls.CustomPicker;

public class PickerDropdownPage : ContentPage
{
    readonly Action<int> _onPicked;
    readonly IList<string> _items;
    int _selectedIndex;

    public PickerDropdownPage(IList<string> items, int selectedIndex, Action<int> onPicked)
    {
        _items = items ?? new List<string>();
        _selectedIndex = selectedIndex;
        _onPicked = onPicked;

        BackgroundColor = Color.FromRgba(0f, 0f, 0f, 0.25f); // translucent backdrop

        var container = new Frame
        {
            BackgroundColor = Color.FromArgb("#00514a"),
            CornerRadius = 8,
            Padding = new Thickness(8),
            HasShadow = true,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Content = BuildItemsStack()
        };

        // Tap outside to dismiss without change
        var outerTap = new TapGestureRecognizer();
        outerTap.Tapped += async (s, e) =>
        {
            await Application.Current.MainPage.Navigation.PopModalAsync(animated: true);
        };

        var outer = new Grid();
        outer.GestureRecognizers.Add(outerTap);
        outer.Children.Add(container);

        Content = outer;
    }

    StackLayout BuildItemsStack()
    {
        var stack = new StackLayout
        {
            Spacing = 6,
            Orientation = StackOrientation.Vertical,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Center
        };

        for (int i = 0; i < _items.Count; i++)
        {
            var idx = i;
            var lbl = new Label
            {
                Text = _items[i],
                Padding = new Thickness(12, 8),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = idx == _selectedIndex ? Color.FromArgb("#05bfb5") : Colors.Transparent,
                TextColor = Colors.White
            };

            var tap = new TapGestureRecognizer();
            tap.Tapped += async (s, e) =>
            {
                _onPicked?.Invoke(idx);
                await Application.Current.MainPage.Navigation.PopModalAsync(animated: true);
            };
            lbl.GestureRecognizers.Add(tap);

            stack.Children.Add(lbl);
        }

        return stack;
    }
}