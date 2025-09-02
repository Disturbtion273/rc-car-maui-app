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
        await Shell.Current.Navigation.PushModalAsync(new ManualDrivingPage());
    }
    
    private async void SemiAutomaticDriving(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PushModalAsync(new SemiAutomaticDrivingPage());
    }
    
    private void DrivingThree(object sender, EventArgs e)
    {
        Console.WriteLine("Third Driving Mode");
    }
}