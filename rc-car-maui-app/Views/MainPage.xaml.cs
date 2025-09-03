using System.Text.RegularExpressions;
using rc_car_maui_app.Helpers;
using rc_car_maui_app.Websocket;

namespace rc_car_maui_app.Views;

public partial class MainPage : ContentPage
{
    private static readonly Regex ipAddressRegex = new Regex(@"^(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)){3}$");

    public MainPage()
    {
        InitializeComponent();
        WebsocketClient.StateChanged += OnWebsocketStateChanged;
        WebsocketClient.ConnectionInfoChanged += OnConnectionInfoChanged;
    }

    /**
     * This method is called when the WebSocket connection information changes.
     * It updates the UI with the provided message and color.
     */
    private void OnConnectionInfoChanged(string message, Color color)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            InformationLabel.IsVisible = true;
            InformationLabel.Text = message;
            InformationLabel.TextColor = color;
        });
    }

    /**
     * This method handles the state changes of the WebSocket client.
     * It updates the UI based on the current state of the WebSocket connection.
     */
    private void OnWebsocketStateChanged(WebsocketClientState state)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            switch (state)
            {
                case WebsocketClientState.Disconnected:
                    DisconnectedLayout.IsVisible = true;
                    ConnectedLayout.IsVisible = false;
                    LoaderOverlay.IsVisible = false;
                    break;
                case WebsocketClientState.Connecting:
                    DisconnectedLayout.IsVisible = true;
                    ConnectedLayout.IsVisible = false;
                    LoaderOverlay.IsVisible = true;
                    break;
                case WebsocketClientState.Connected:
                    DisconnectedLayout.IsVisible = false;
                    ConnectedLayout.IsVisible = true;
                    LoaderOverlay.IsVisible = false;
                    break;
                case WebsocketClientState.Disconnecting:
                    DisconnectedLayout.IsVisible = false;
                    ConnectedLayout.IsVisible = true;
                    LoaderOverlay.IsVisible = true;
                    break;
            }
        });
    }

    /**
     * This method is called when the user clicks the connect button.
     * It initiates the connection to the WebSocket server using the IP address entered by the user.
     */
    private void OnConnectClicked(object? sender, EventArgs e)
    {
        _ = WebsocketClient.Connect(IpEntry.Text);
    }

    /**
     * This method is called when the text in the IP address entry changes.
     * It filters the input to allow only digits and dots, and validates the IP address format.
     * If the input is valid, it enables the connect button; otherwise, it disables it.
     */
    private void OnIpEntryChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            var filtered = new string(e.NewTextValue.Where(c => char.IsDigit(c) || c == '.').ToArray());

            if (filtered.Length > 15)
                filtered = filtered[..15];
            if (entry.Text != filtered)
                entry.Text = filtered;

            var isValid = ipAddressRegex.IsMatch(filtered);
            IpEntryBorder.Stroke = isValid ? Colors.Green : Colors.Red;
            ConnectionButton.IsEnabled = isValid;
            InformationLabel.IsVisible = false;
        }
    }

    /**
     * This method is called when the user clicks the disconnect button.
     * It initiates the disconnection from the WebSocket server.
     */
    private void OnDisconnectClicked(object? sender, EventArgs e)
    {
        _ = WebsocketClient.Disconnect();
    }
}