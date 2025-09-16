namespace rc_car_maui_app.Controls;

public partial class OverlayBackButton : ContentView
{
    public OverlayBackButton()
    {
        InitializeComponent();
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopModalAsync();
    }
}