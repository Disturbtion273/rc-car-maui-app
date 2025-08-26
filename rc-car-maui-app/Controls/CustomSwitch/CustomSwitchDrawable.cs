namespace rc_car_maui_app.Controls.CustomSwitch;

public class CustomSwitchDrawable : IDrawable
{
    // Colors (defaults set to what you provided)
    public Color TrackColorOn { get; set; } = Color.FromArgb("#002c28");
    public Color TrackColorOff { get; set; } = Color.FromArgb("#00514a");
    public Color CapsuleColorOn { get; set; } = Color.FromArgb("#05bfb5");
    public Color CapsuleColorOff { get; set; } = Color.FromArgb("#02988f");

    // State
    public bool IsToggled { get; set; }

    // 0 = off (left), 1 = on (right)
    float _animatedPosition;

    public float AnimatedPosition
    {
        get => _animatedPosition;
        set => _animatedPosition = Math.Clamp(value, 0f, 1f);
    }

    public CustomSwitchDrawable(bool isToggled = false)
    {
        IsToggled = isToggled;
        AnimatedPosition = isToggled ? 1f : 0f;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        var w = dirtyRect.Width;
        var h = dirtyRect.Height;
        if (w <= 0 || h <= 0) return;

        // Inset so Capsule has padding from track edges
        var inset = Math.Min(w, h) * 0.06f;
        var trackX = dirtyRect.X + inset;
        var trackY = dirtyRect.Y + inset;
        var trackW = Math.Max(0, w - inset * 2);
        var trackH = Math.Max(0, h - inset * 2);
        var trackRadius = trackH / 2f;

        // Track
        var trackColor = BlendColors(TrackColorOff, TrackColorOn, AnimatedPosition);
        canvas.FillColor = trackColor;
        canvas.FillRoundedRectangle(trackX, trackY, trackW, trackH, trackRadius);

        // Capsule geometry: width = half of track width, height slightly smaller than track height
        var CapsuleWidth = trackW * 0.5f; // **half of track width**
        var CapsuleHeight = trackH * 0.84f;
        var CapsuleRadius = CapsuleHeight / 2f;

        // Allowed center X range (account for half Capsule width)
        var minCenterX = trackX + CapsuleWidth / 2f + (trackH * 0.08f);
        var maxCenterX = trackX + trackW - CapsuleWidth / 2f - (trackH * 0.08f);
        var centerX = Lerp(minCenterX, maxCenterX, AnimatedPosition);
        var centerY = trackY + trackH / 2f;

        // Subtle shadow below Capsule
        canvas.SaveState();
        canvas.FillColor = Color.FromRgba(0, 0, 0, 0.06f);
        // draw shadow as slightly larger capsule
        canvas.FillRoundedRectangle(centerX - CapsuleWidth / 2f + 1f, centerY - CapsuleHeight / 2f + 1.4f,
            CapsuleWidth, CapsuleHeight, CapsuleRadius);
        canvas.RestoreState();

        // Capsule color blends between off->on
        var CapsuleColor = BlendColors(CapsuleColorOff, CapsuleColorOn, AnimatedPosition);
        canvas.FillColor = CapsuleColor;

        // Draw Capsule as a rounded rect (capsule)
        canvas.FillRoundedRectangle(centerX - CapsuleWidth / 2f,
            centerY - CapsuleHeight / 2f,
            CapsuleWidth,
            CapsuleHeight,
            CapsuleRadius);
    }

    // helpers
    static float Lerp(float a, float b, float t) => a + (b - a) * t;

    static Color BlendColors(Color a, Color b, float t)
    {
        t = Math.Clamp(t, 0f, 1f);
        return new Color(
            a.Red + (b.Red - a.Red) * t,
            a.Green + (b.Green - a.Green) * t,
            a.Blue + (b.Blue - a.Blue) * t,
            a.Alpha + (b.Alpha - a.Alpha) * t
        );
    }
}