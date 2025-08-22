using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rc_car_maui_app.Controls;

public partial class OverlayBackButton : ContentView
{
    public OverlayBackButton()
    {
        InitializeComponent();
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopAsync(animated: true);
    }
}