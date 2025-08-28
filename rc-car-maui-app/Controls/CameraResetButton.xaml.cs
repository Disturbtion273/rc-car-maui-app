using System.Text.Json;
using rc_car_maui_app.Websocket;

namespace rc_car_maui_app.Controls;

public partial class CameraResetButton : ContentView
{
    public CameraResetButton()
    {
        InitializeComponent();
    }

    private void CameraResetButton_OnClicked(object? sender, EventArgs e)
    {
        WebsocketClient.Send(JsonSerializer.Serialize(new { cameraReset = true }));
    }
}