namespace rc_car_maui_app.Controls;

public partial class Notification : ContentView
{
    private UserOptions? _userOptions;

    public Notification(string source, string message, UserOptions? userOptions = null)
    {
        InitializeComponent();
        Sign.Source = source;
        Label.Text = message;

        if (userOptions != null)
        {
            _userOptions = userOptions;
            LeftButton.Text = _userOptions.LeftText;
            RightButton.Text = _userOptions.RightText;
            LeftButton.Command = new Command(() =>
            {
                _userOptions.ActionExecuted = true;
                _userOptions.LeftAction();
            });
            RightButton.Command = new Command(() =>
            {
                _userOptions.ActionExecuted = true;
                _userOptions.RightAction();
            });

            Separator.IsVisible = true;
            Options.IsVisible = true;
        }
    }

    public async Task Show(Layout layout)
    {
        Opacity = 0;
        TranslationY = -100;
        
        layout.Add(this);
        
        await Task.WhenAll(
            this.FadeTo(1, 300, Easing.SinIn),
            this.TranslateTo(0, 0, 300, Easing.SinOut)
        );

        if (_userOptions != null)
        {
            while (!_userOptions.ActionExecuted)
            {
                await Task.Delay(50);
            }

            _userOptions.ActionExecuted = false;
        }
        else
        {
            await Task.Delay(5000);
        }

        await Task.WhenAll(
            this.FadeTo(0, 300, Easing.SinOut),
            this.TranslateTo(0, -100, 300, Easing.SinIn)
        );
        
        layout.Remove(this);
    }

    public class UserOptions
    {
        public bool ActionExecuted = false;
        public readonly string LeftText;
        public readonly string RightText;
        public readonly Action LeftAction;
        public readonly Action RightAction;

        public UserOptions(string leftText, string rightText, Action leftAction, Action rightAction)
        {
            LeftText = leftText;
            RightText = rightText;
            LeftAction = leftAction;
            RightAction = rightAction;
        }
    }
}