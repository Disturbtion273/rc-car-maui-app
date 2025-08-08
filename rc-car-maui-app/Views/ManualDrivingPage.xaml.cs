using rc_car_maui_app.Services;

namespace rc_car_maui_app.Views;

public partial class ManualDrivingPage : ContentPage
{
    private readonly IDeviceOrientationService _orientationService;

    public ManualDrivingPage()
    {
        InitializeComponent();
        _orientationService = Application.Current.Windows[0].Page.Handler.MauiContext.Services
            .GetService<IDeviceOrientationService>();

        Content = new Label
        {
            Text = "Landscape Mode",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
        _orientationService.SetLandscape();
    }

    protected override void OnDisappearing()
    {
        _orientationService.SetPortrait();
        base.OnDisappearing();
    }
}