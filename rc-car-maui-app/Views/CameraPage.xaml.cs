// using System.Web;
//
// namespace rc_car_maui_app.Views;
//
// [QueryProperty(nameof(StreamUrl), "StreamUrl")]
// public partial class CameraPage : ContentPage
// {
//     string _streamUrl = string.Empty;
//     public string StreamUrl
//     {
//         get => _streamUrl;
//         set
//         {
//             _streamUrl = value ?? "";
//             Console.WriteLine($"[CameraPage] StreamUrl set: {_streamUrl}");
//             if (StreamWebView != null && !string.IsNullOrWhiteSpace(_streamUrl))
//                 LoadUrl(_streamUrl);
//         }
//     }
//
//     public CameraPage()
//     {
//         InitializeComponent();
//         StreamWebView.Navigating += (_, e) => Console.WriteLine($"[WebView] Navigating: {e.Url}");
//         StreamWebView.Navigated  += (_, e) => Console.WriteLine($"[WebView] Navigated: {e.Url}, Result: {e.Result}");
//     }
//
//     protected override void OnAppearing()
//     {
//         base.OnAppearing();
//         Console.WriteLine("[CameraPage] OnAppearing");
//         if (!string.IsNullOrWhiteSpace(_streamUrl))
//             LoadUrl(_streamUrl);
//     }
//
//     void LoadUrl(string url)
//     {
//         Console.WriteLine($"[CameraPage] LoadUrl: {url}");
//         StreamWebView.Source = new UrlWebViewSource { Url = url };
//     }
//
//     private async void OnHomeClicked(object sender, EventArgs e)
//     {
//         await Shell.Current.GoToAsync("///MainPage");
//     }
//
//
// }
namespace rc_car_maui_app.Views;


[QueryProperty(nameof(StreamUrl), "StreamUrl")]

public partial class CameraPage : ContentPage
{
    // public string? StreamUrl { get; set; }
    // public CameraPage()
    // {
    //     InitializeComponent();
    // }
    // protected override void OnAppearing()
    // {
    //     base.OnAppearing();
    //     StreamWebView.Source = new UrlWebViewSource { Url = StreamUrl };
    // }
    private string? _streamUrl;
    public string? StreamUrl
    {
        get => _streamUrl;
        set
        {
            _streamUrl = value;
            TryLoad();              // sofort laden, wenn Wert ankommt
        }
    }

    public CameraPage()
    {
        InitializeComponent();
        Loaded += (_, __) => TryLoad();   // stellt sicher: View existiert
    }

    void TryLoad()
    {
        if (!string.IsNullOrWhiteSpace(_streamUrl) && StreamWebView != null)
            StreamWebView.Source = _streamUrl;
    }
    private async void OnHomeClicked(object sender, EventArgs e)
     {
         await Shell.Current.GoToAsync("///CarControlPage");
     }
    protected override void OnDisappearing() {
        base.OnDisappearing();
        StreamWebView.Source = null;
    }
}
