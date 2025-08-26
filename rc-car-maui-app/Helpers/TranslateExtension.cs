using System;
using System.ComponentModel;
using System.Reflection;
using System.Resources;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace rc_car_maui_app.Helpers
{
    [ContentProperty(nameof(Text))]
    public class TranslateExtension : IMarkupExtension<BindingBase>
    {
        const string ResourceId = "rc_car_maui_app.Resources.Strings.AppResources";
        static readonly Lazy<ResourceManager> ResMgr =
            new(() => new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly));

        public string Text { get; set; } = string.Empty;

        // Kleines Bindable-Objekt, das auf Kulturwechsel reagiert
        class LocalizedString : INotifyPropertyChanged
        {
            readonly string _key;
            public event PropertyChangedEventHandler? PropertyChanged;

            public LocalizedString(string key)
            {
                _key = key;
                Localization.CultureChanged += OnCultureChanged;
            }

            void OnCultureChanged() =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));

            public string Value =>
                ResMgr.Value.GetString(_key, Localization.CurrentCulture) ?? $"[{_key}]";
        }

        public BindingBase ProvideValue(IServiceProvider serviceProvider)
        {
            var src = new LocalizedString(Text);
            // Liefert eine Binding-Quelle statt eines fixen Strings -> kann aktualisieren
            return new Binding(nameof(LocalizedString.Value), source: src, mode: BindingMode.OneWay);
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
    }
}