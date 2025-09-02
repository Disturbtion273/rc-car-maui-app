using rc_car_maui_app.Enums;
using rc_car_maui_app.Websocket;

namespace rc_car_maui_app.Views;

public partial class DrivingPage : ContentPage
{
    public DrivingPage()
    {
        InitializeComponent();
    }
    
    // these function serve as placeholder for now
    // for later these function should navigate to the corrected driving mode view
    private async void ManualDriving(object sender, EventArgs e)
    {
        WebsocketClient.SetDrivingMode(DrivingMode.Manual);
        await Shell.Current.Navigation.PushModalAsync(new ManualDrivingPage());
    }
    
    private async void SemiAutomaticDriving(object sender, EventArgs e)
    {
        WebsocketClient.SetDrivingMode(DrivingMode.SemiAutomatic);
        await Shell.Current.Navigation.PushModalAsync(new SemiAutomaticDrivingPage());
    }
    
    private void DrivingThree(object sender, EventArgs e)
    {
        WebsocketClient.SetDrivingMode(DrivingMode.Automatic);
        Console.WriteLine("Third Driving Mode");
    }
}