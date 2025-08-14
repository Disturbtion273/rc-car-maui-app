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
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}