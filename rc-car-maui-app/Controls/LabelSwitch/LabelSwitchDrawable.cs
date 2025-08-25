namespace rc_car_maui_app.Controls.LabelSwitch;

public class LabelSwitchDrawable : IDrawable
{
    // Texts
        public string SelectedText { get; set; } = "KM/H";
        public string UnselectedText { get; set; } = "MP/H";

        // Colors (customize via properties on the control)
        public Color TrackColor { get; set; }    = Color.FromArgb("#A8000000");
        public Color CapsuleColor { get; set; }  = Color.FromArgb("#05bfb5");
        public Color SelectedTextColor { get; set; } = Colors.White;
        public Color UnselectedTextColor { get; set; } = Colors.Black;

        // State
        public bool IsLeftSelected { get; set; }

        // Animated 0..1 where 0 = left, 1 = right
        float _animatedPosition;
        public float AnimatedPosition
        {
            get => _animatedPosition;
            set => _animatedPosition = Math.Clamp(value, 0f, 1f);
        }

        public LabelSwitchDrawable(bool leftSelected = true)
        {
            IsLeftSelected = leftSelected;
            AnimatedPosition = leftSelected ? 0f : 1f;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            var w = dirtyRect.Width;
            var h = dirtyRect.Height;
            if (w <= 0 || h <= 0) return;

            // small inset so things don't touch edges
            var inset = Math.Min(w, h) * 0.06f;
            var trackX = dirtyRect.X + inset;
            var trackY = dirtyRect.Y + inset;
            var trackW = Math.Max(0, w - inset * 2);
            var trackH = Math.Max(0, h - inset * 2);
            var trackRadius = trackH / 2f;

            // draw track
            canvas.FillColor = TrackColor;
            canvas.FillRoundedRectangle(trackX, trackY, trackW, trackH, trackRadius);

            // left/right halves
            var halfW = trackW / 2f;
            var leftCenterX = trackX + halfW / 2f + (trackH * 0.018f);
            var rightCenterX = trackX + halfW + halfW / 2f - (trackH * 0.018f);
            var centerY = trackY + trackH / 2f;

            // capsule size (smaller than half so it sits under the text)
            var capsuleWidth = halfW * 0.93f;
            var capsuleHeight = trackH * 0.84f;
            var capsuleRadius = capsuleHeight / 2f;

            // capsule center moves between leftCenterX and rightCenterX
            var capsuleCenterX = Lerp(leftCenterX, rightCenterX, AnimatedPosition);

            // draw capsule (slightly offset shadow for depth)
            canvas.SaveState();
            canvas.FillColor = Color.FromRgba(0, 0, 0, 0.06f);
            canvas.FillRoundedRectangle(capsuleCenterX - capsuleWidth / 2f + 1f,
                                        centerY - capsuleHeight / 2f + 1.4f,
                                        capsuleWidth,
                                        capsuleHeight,
                                        capsuleRadius);
            canvas.RestoreState();

            canvas.FillColor = CapsuleColor;
            canvas.FillRoundedRectangle(capsuleCenterX - capsuleWidth / 2f,
                                        centerY - capsuleHeight / 2f,
                                        capsuleWidth,
                                        capsuleHeight,
                                        capsuleRadius);

            // draw labels centered inside each half (text sits above capsule)
            // set font size relative to track height
            var fontSize = trackH * 0.45f;
            canvas.FontSize = fontSize;
            canvas.Font = Microsoft.Maui.Graphics.Font.Default;

            // left text rect
            var leftRect = new RectF(trackX, trackY, halfW, trackH);
            canvas.FontColor = IsLeftSelected ? SelectedTextColor : UnselectedTextColor;
            canvas.DrawString(UnselectedText, leftRect, HorizontalAlignment.Center, VerticalAlignment.Center);

            // right text rect
            var rightRect = new RectF(trackX + halfW, trackY, halfW, trackH);
            canvas.FontColor = IsLeftSelected ? UnselectedTextColor : SelectedTextColor;
            canvas.DrawString(SelectedText, rightRect, HorizontalAlignment.Center, VerticalAlignment.Center);
        }

        static float Lerp(float a, float b, float t) => a + (b - a) * t;
}