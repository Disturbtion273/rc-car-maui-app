using Android.Content.PM;
using rc_car_maui_app.Services;

namespace rc_car_maui_app;

public class DeviceOrientationService : IDeviceOrientationService
{
    public void SetLandscape()
    {
        var activity = Platform.CurrentActivity;
        if (activity != null)
        {
            activity.RequestedOrientation = ScreenOrientation.SensorLandscape;
        }
    }

    public void SetPortrait()
    {
        var activity = Platform.CurrentActivity;
        if (activity != null)
        {
            activity.RequestedOrientation = ScreenOrientation.Portrait;
        }
    }
}