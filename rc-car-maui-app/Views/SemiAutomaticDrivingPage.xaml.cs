using rc_car_maui_app.Controls;
using rc_car_maui_app.Websocket;

namespace rc_car_maui_app.Views;

public partial class SemiAutomaticDrivingPage : AbstractInteractiveDrivingPage
{
    public SemiAutomaticDrivingPage()
    {
        InitializeComponent();
        WebsocketClient.NotificationReceived += WebsocketClientOnNotificationReceived;
        WebsocketClient.SpeedLimitReceived += WebsocketClientOnSpeedLimitReceived;
    }
    
    private void WebsocketClientOnNotificationReceived(Notification? notification)
    {
        if (notification != null)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await notification.Show(MainGrid);
            });
        }
    }
    
    private void WebsocketClientOnSpeedLimitReceived(string value)
    {
        if (value == "dreissig")
        {
            Slider.Maximum = 30;
            Slider.Minimum = -30;
        } else if (value == "fuenfzig")
        {
            Slider.Maximum = 50;
            Slider.Minimum = -50;
        }
        else if (value == "unbegrenzt")
        {
            Slider.Maximum = 100;
            Slider.Minimum = -100;
        }
    }
}