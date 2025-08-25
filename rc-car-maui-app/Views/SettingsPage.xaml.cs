using rc_car_maui_app.Controls.CustomPicker;

namespace rc_car_maui_app.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }
    
    private void OnUnitToggled(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            Console.WriteLine("Unit: KM/H");
        }
        else
        {
            Console.WriteLine("Unit: MP/H");
        }
    }
    
    private void OnBatteryToggled(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            Console.WriteLine("Show Battery: ON");
        }
        else
        {
            Console.WriteLine("Show Battery: OFF");
        }
    }
    
    private void OnSafeModeToggled(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            Console.WriteLine("Safe mode ON");
        }
        else
        {
            Console.WriteLine("Safe mode OFF");
        }
    }
    
    private void SelectedLanguageChanged(object sender, EventArgs e)
    {
        var picker = (CustomPicker)sender;
        string lang = picker.Items[picker.SelectedIndex];

        switch (lang)
        {
            case "DE":
                Console.WriteLine("German selected");
                break;
            case "EN":
                Console.WriteLine("English selected");
                break;
            case "SP":
                Console.WriteLine("Spanish selected");
                break;
        }
    }
    
    private void OnDarkModeToggled(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            Console.WriteLine("Dark Mode");
        }
        else
        {
            Console.WriteLine("Light Mode");
        }
    }
}