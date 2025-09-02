using System;
using System.Globalization;
using System.Threading;

namespace rc_car_maui_app.Helpers
{
    public static class Localization
    {
        public static event Action? CultureChanged;

        public static CultureInfo CurrentCulture { get; private set; } = new("en");
        public static string CurrentCode => CurrentCulture.TwoLetterISOLanguageName;

        public static void Initialize(string code)
        {
            SetCulture(code, raiseEvent: false);
        }

        public static void SetCulture(string code, bool raiseEvent = true)
        {
            var culture = new CultureInfo(code);
            CurrentCulture = culture;

            CultureInfo.DefaultThreadCurrentCulture   = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture       = culture;
            Thread.CurrentThread.CurrentUICulture     = culture;

            if (raiseEvent) CultureChanged?.Invoke();
        }
    }
}