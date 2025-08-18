namespace rc_car_maui_app.Controls.Slider;

public class CustomSliderDrawable : IDrawable
{
    private readonly CustomSlider _customSlider;

    public CustomSliderDrawable(CustomSlider customSlider)
    {
        _customSlider = customSlider;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        float width = dirtyRect.Width;
        float height = dirtyRect.Height;
        
        canvas.FillColor = Color.FromArgb("66000000");
        canvas.FillRoundedRectangle((float)(width / 2 - _customSlider.TrackWidth / 2),(float)(height - _customSlider.TrackHeight - _customSlider.ThumbHeight / 2), (float)_customSlider.TrackWidth, (float)_customSlider.TrackHeight, 10);
        
        double percent = (_customSlider.Value - _customSlider.Minimum) / (_customSlider.Maximum - _customSlider.Minimum);
        float thumbY = (float)((1 - percent) * _customSlider.TrackHeight);
        canvas.FillColor = Colors.Black;
        canvas.FillRoundedRectangle(0, thumbY, (float)_customSlider.ThumbWidth, (float)_customSlider.ThumbHeight, 10);
    }
}