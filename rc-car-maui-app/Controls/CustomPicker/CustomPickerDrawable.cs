namespace rc_car_maui_app.Controls.CustomPicker;

public class CustomPickerDrawable : IDrawable
{
    public string SelectedText { get; set; } = "EN";

        // appearance
        public Color TrackColor { get; set; } = Color.FromArgb("#A8000000");
        public Color PickerIconColor { get; set; } = Color.FromArgb("#03BFB5");
        public Color TextColor { get; set; } = Color.FromArgb("#03BFB5");

        // arrow direction indicator (down when closed)
        public bool IsOpen { get; set; } = false;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            var w = dirtyRect.Width;
            var h = dirtyRect.Height;
            if (w <= 0 || h <= 0) return;

            // inset so text/icon don't touch edges
            var inset = Math.Min(w, h) * 0.08f;
            var x = dirtyRect.X + inset;
            var y = dirtyRect.Y + inset;
            var width = Math.Max(0, w - inset * 2);
            var height = Math.Max(0, h - inset * 2);
            var radius = height / 2f;

            // draw Track rounded rect
            canvas.FillColor = TrackColor;
            canvas.FillRoundedRectangle(x, y, width, height, radius);

            // draw text (left side)
            var textRect = new RectF(x + 8, y, width * 0.7f, height); // text area leaving room for arrow
            canvas.FontSize = height * 0.48f;
            canvas.Font = Microsoft.Maui.Graphics.Font.Default;
            canvas.FontColor = TextColor;
            canvas.DrawString(SelectedText, textRect, HorizontalAlignment.Left, VerticalAlignment.Center);

            // draw arrow on the right
            var arrowSize = height * 0.35f;
            var arrowCenterX = x + width - (height * 0.5f); // keep arrow near right edge with some padding
            var arrowCenterY = y + height / 2f;

            DrawDownArrow(canvas, arrowCenterX, arrowCenterY, arrowSize, PickerIconColor, IsOpen);
        }

        void DrawDownArrow(ICanvas canvas, float cx, float cy, float size, Color color, bool open)
        {
            // simple triangle arrow; if open = true, draw up arrow instead
            float half = size / 2f;
            PointF p1, p2, p3;
            if (!open)
            {
                // down
                p1 = new PointF(cx - half, cy - (half / 2f));
                p2 = new PointF(cx + half, cy - (half / 2f));
                p3 = new PointF(cx, cy + (half / 2f));
            }
            else
            {
                // up
                p1 = new PointF(cx - half, cy + (half / 2f));
                p2 = new PointF(cx + half, cy + (half / 2f));
                p3 = new PointF(cx, cy - (half / 2f));
            }

            var path = new PathF();
            path.MoveTo(p1.X, p1.Y);
            path.LineTo(p2.X, p2.Y);
            path.LineTo(p3.X, p3.Y);
            path.Close();

            canvas.FillColor = color;
            canvas.FillPath(path);
        }
}