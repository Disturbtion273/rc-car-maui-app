using Microsoft.Extensions.DependencyInjection;

public static class ServiceHelper
{
    public static T GetService<T>() where T : notnull =>
        Application.Current!.Handler!.MauiContext!.Services.GetRequiredService<T>();
}