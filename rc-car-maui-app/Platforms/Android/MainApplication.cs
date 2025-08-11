using Android.App;
using Android.Runtime;
using rc_car_maui_app.Services;

namespace rc_car_maui_app;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
        DependencyService.Register<IDeviceOrientationService, DeviceOrientationService>();
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}