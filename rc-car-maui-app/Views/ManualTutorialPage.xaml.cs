using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rc_car_maui_app.Views;

public partial class ManualTutorialPage : ContentPage
{
    public ManualTutorialPage()
    {
        InitializeComponent();
    }
    
    private async void BackButton(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopAsync(animated: true);
    }
}