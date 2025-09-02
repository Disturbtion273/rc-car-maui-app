namespace rc_car_maui_app.Controls;

public partial class Notification : ContentView
{
    public Notification(string source, string message)
    {
        InitializeComponent();
        Sign.Source = source;
        Label.Text = message;
    }

    public async Task Show(Layout layout)
    {
        Opacity = 0;
        TranslationY = -100;
        
        layout.Add(this);
        
        await Task.WhenAll(
            this.FadeTo(1, 300, Easing.SinIn),
            this.TranslateTo(0, 0, 300, Easing.SinOut)
        );
        
        await Task.Delay(5000);
        
        await Task.WhenAll(
            this.FadeTo(0, 300, Easing.SinOut),
            this.TranslateTo(0, -100, 300, Easing.SinIn)
        );
        
        layout.Remove(this);
    }
}