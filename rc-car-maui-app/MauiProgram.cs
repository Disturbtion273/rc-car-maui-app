using Microsoft.Extensions.Logging;

namespace rc_car_maui_app;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if ANDROID
        Microsoft.Maui.Handlers.WebViewHandler.Mapper.AppendToMapping("MyZoomSettings", (handler, view) =>
        {
            handler.PlatformView.Settings.UseWideViewPort = true;
            handler.PlatformView.SetInitialScale(1);
        });
        
        //Global: Disable scrolling for ALL WebViews on Android
        Microsoft.Maui.Handlers.WebViewHandler.Mapper.AppendToMapping("DisableScroll", (handler, view) =>
        {
            if (handler.PlatformView is Android.Webkit.WebView awv)
            {
                awv.VerticalScrollBarEnabled = false;
                awv.HorizontalScrollBarEnabled = false;
                
                awv.Touch += (s, e) =>
                {
                    if (e.Event?.Action == Android.Views.MotionEventActions.Move)
                        e.Handled = true;
                };
            }
        });
#endif
#if IOS || MACCATALYST
        // Global: Disable scrolling for ALL WebViews on IOS/macOS
        Microsoft.Maui.Handlers.WebViewHandler.Mapper.AppendToMapping("DisableScroll", (handler, view) =>
        {
            if (handler.PlatformView is WebKit.WKWebView wk)
            {
                wk.ScrollView.ScrollEnabled = false;
                wk.ScrollView.Bounces = false;
                wk.ScrollView.AlwaysBounceVertical = false;
                wk.ScrollView.AlwaysBounceHorizontal = false;
            }
        });
#endif
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}