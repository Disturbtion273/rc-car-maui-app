using rc_car_maui_app.Controls;
using rc_car_maui_app.Websocket;

namespace rc_car_maui_app.Views;

public partial class AutomaticDrivingPage : AbstractDrivingPage
{
    public AutomaticDrivingPage()
    {
        InitializeComponent();
        WebsocketClient.SpeedInfoChanged += WebsocketClientOnSpeedInfoChanged;
        WebsocketClient.NotificationReceived += WebsocketClientOnNotificationReceived;
    }

    private void WebsocketClientOnSpeedInfoChanged(int speed)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            SpeedDisplayControl.UpdateControls(speed);
        });
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