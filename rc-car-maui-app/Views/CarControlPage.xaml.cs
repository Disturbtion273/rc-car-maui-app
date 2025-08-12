
namespace rc_car_maui_app.Views;

public partial class CarControlPage : ContentPage
{
    public CarControlPage()
    {
        InitializeComponent();
    }
        private async void OnShowCameraClicked(object sender, EventArgs e)
        { 
         var testUrl = "http://10.0.2.2:5000";
         await Shell.Current.GoToAsync("//CameraPage", new Dictionary<string, object>
         {
             {"StreamUrl", testUrl }
         });   
        }
    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }


}