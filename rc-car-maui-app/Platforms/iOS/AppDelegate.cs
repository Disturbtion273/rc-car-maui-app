using Foundation;
using rc_car_maui_app.Services;
using UIKit;

namespace rc_car_maui_app;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp()
    {
        DependencyService.Register<IDeviceOrientationService, DeviceOrientationService>();
        return MauiProgram.CreateMauiApp();
    }

    [Export("application:supportedInterfaceOrientationsForWindow:")]
    public UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
    {
        return OrientationLock.AllowedOrientations;
    }
}