using rc_car_maui_app.Services;
using rc_car_maui_app.Websocket;

namespace rc_car_maui_app.Views;

public abstract partial class AbstractDrivingPage : ContentPage
{
    public string WebViewUrl { get; }

    private readonly IDeviceOrientationService _orientationService;

    protected AbstractDrivingPage()
    {
        InitializeComponent();
        _orientationService = DependencyService.Get<IDeviceOrientationService>();
        WebViewUrl = $"http://{WebsocketClient.GetHost()}:8080";
        BindingContext = this; 

        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            MainGrid.Padding = new Thickness(50, 0);
        }
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

    protected void AddToTopLeftLayout(View contentView)
    {
        TopLeftLayout.Add(contentView);
    }
    
    protected void AddToTopRightLayout(View contentView)
    {
        TopRightLayout.Add(contentView);
    }
    
    protected void AddToGrid(View contentView, LayoutOptions horizontalOptions, LayoutOptions verticalOptions, Thickness margin = new Thickness())
    {
        contentView.HorizontalOptions = horizontalOptions;
        contentView.VerticalOptions = verticalOptions;
        contentView.Margin = margin;
        MainGrid.Add(contentView);
    }
}