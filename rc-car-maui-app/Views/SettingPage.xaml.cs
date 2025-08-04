using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rc_car_maui_app.Views;

public partial class SettingPage : ContentPage
{
    public SettingPage()
    {
        InitializeComponent();
    }
    
    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }
}