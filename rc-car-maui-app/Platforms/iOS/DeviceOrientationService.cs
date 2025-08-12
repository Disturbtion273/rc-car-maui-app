using System.Diagnostics;
using rc_car_maui_app;
using rc_car_maui_app.Services;
using UIKit;

namespace rc_car_maui_app;

public class DeviceOrientationService : IDeviceOrientationService
{
    public void SetLandscape()
    {
        SetOrientation(UIInterfaceOrientationMask.LandscapeRight);
        OrientationLock.AllowedOrientations = UIInterfaceOrientationMask.Landscape;
    }

    public void SetPortrait()
    {
        SetOrientation(UIInterfaceOrientationMask.Portrait);
        OrientationLock.AllowedOrientations = UIInterfaceOrientationMask.Portrait;
    }

    private void SetOrientation(UIInterfaceOrientationMask orientation)
    {
        if (UIDevice.CurrentDevice.CheckSystemVersion(16, 0))
        {
            var scene = UIApplication.SharedApplication.ConnectedScenes.ToArray()[0] as UIWindowScene;
            if (scene != null)
            {
                var rootViewController = UIApplication.SharedApplication.KeyWindow?.RootViewController;
                if (rootViewController != null)
                {
                    scene.RequestGeometryUpdate(
                        new UIWindowSceneGeometryPreferencesIOS(orientation),
                        error => { Debug.WriteLine(error.ToString()); });
                    rootViewController.SetNeedsUpdateOfSupportedInterfaceOrientations();
                    rootViewController.NavigationController?.SetNeedsUpdateOfSupportedInterfaceOrientations();
                }
            }
        }
    }
}