namespace rc_car_maui_app.Controls.CustomSwitch;

public class CustomSwitchDrawable : IDrawable
    {
        // Colors (defaults set to what you provided)
        public Color TrackColorOn { get; set; }   = Color.FromArgb("#002c28");
        public Color TrackColorOff { get; set; }  = Color.FromArgb("#00514a");
        public Color ThumbColorOn { get; set; }   = Color.FromArgb("#05bfb5");
        public Color ThumbColorOff { get; set; }  = Color.FromArgb("#02988f");

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

            // Inset so thumb has padding from track edges
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

            // Thumb geometry: width = half of track width, height slightly smaller than track height
            var thumbWidth = trackW * 0.5f;               // **half of track width**
            var thumbHeight = trackH * 0.84f;
            var thumbRadius = thumbHeight / 2f;

            // Allowed center X range (account for half thumb width)
            var minCenterX = trackX + thumbWidth / 2f + (trackH * 0.08f);
            var maxCenterX = trackX + trackW - thumbWidth / 2f - (trackH * 0.08f);
            var centerX = Lerp(minCenterX, maxCenterX, AnimatedPosition);
            var centerY = trackY + trackH / 2f;

            // Subtle shadow below thumb
            canvas.SaveState();
            canvas.FillColor = Color.FromRgba(0, 0, 0, 0.06f);
            // draw shadow as slightly larger capsule
            canvas.FillRoundedRectangle(centerX - thumbWidth / 2f + 1f, centerY - thumbHeight / 2f + 1.4f,
                                        thumbWidth, thumbHeight, thumbRadius);
            canvas.RestoreState();

            // Thumb color blends between off->on
            var thumbColor = BlendColors(ThumbColorOff, ThumbColorOn, AnimatedPosition);
            canvas.FillColor = thumbColor;

            // Draw thumb as a rounded rect (capsule)
            canvas.FillRoundedRectangle(centerX - thumbWidth / 2f,
                                        centerY - thumbHeight / 2f,
                                        thumbWidth,
                                        thumbHeight,
                                        thumbRadius);
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