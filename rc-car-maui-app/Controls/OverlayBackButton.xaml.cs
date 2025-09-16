using rc_car_maui_app.Enums;
using rc_car_maui_app.Websocket;

namespace rc_car_maui_app.Controls;

public partial class OverlayBackButton : ContentView
{
    public OverlayBackButton()
    {
        InitializeComponent();
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        WebsocketClient.SetDrivingMode(DrivingMode.None);
        await Shell.Current.Navigation.PopModalAsync();
    }
}