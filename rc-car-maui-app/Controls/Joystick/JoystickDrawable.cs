namespace rc_car_maui_app.Controls.Joystick;

public class JoystickDrawable(Joystick joystick) : IDrawable
{
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        var innerWidth = (float)joystick.ThumbWidth;
        var innerHeight = (float)joystick.ThumbHeight;

        canvas.FillColor = Color.FromArgb("66000000");
        canvas.FillEllipse(innerWidth / 2, innerHeight / 2, (float)joystick.BaseWidth, (float)joystick.BaseHeight);

        canvas.FillColor = Color.FromArgb("88000000");
        canvas.FillEllipse(
            (float)((joystick.ValueX + 1) / 2 * joystick.BaseWidth),
            (float)((joystick.ValueY + 1) / 2 * joystick.BaseHeight),
            innerWidth, innerHeight);
    }
}