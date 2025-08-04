namespace rc_car_maui_app.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnCarControlClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///CarControlPage");
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///SettingsPage");
    }

}