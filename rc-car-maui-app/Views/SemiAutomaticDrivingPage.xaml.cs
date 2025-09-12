using rc_car_maui_app.Controls;
using rc_car_maui_app.Websocket;

namespace rc_car_maui_app.Views;

public partial class SemiAutomaticDrivingPage : AbstractInteractiveDrivingPage
{
    public SemiAutomaticDrivingPage()
    {
        InitializeComponent();
        WebsocketClient.NotificationReceived += WebsocketClientOnNotificationReceived;
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
}