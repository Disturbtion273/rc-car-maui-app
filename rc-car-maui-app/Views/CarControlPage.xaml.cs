
namespace rc_car_maui_app.Views;

public partial class CarControlPage : ContentPage
{
    public CarControlPage()
    {
        InitializeComponent();
    }
        private async void OnShowCameraClicked(object sender, EventArgs e)
        {
         await Shell.Current.GoToAsync("//CameraPage");   
        }
    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }


}