namespace rc_car_maui_app.Controls.ToggleSwitch;

public class ToggleSwitchDrawable : IDrawable
{
    readonly ToggleSwitch _owner;

        public ToggleSwitchDrawable(ToggleSwitch owner)
        {
            _owner = owner;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float width = dirtyRect.Width;
            float height = dirtyRect.Height;

            // clear background (GraphicsView's background might already be set)
            canvas.SaveState();
            canvas.FillColor = Colors.Transparent;
            canvas.FillRectangle(dirtyRect);
            canvas.RestoreState();

            // friendly local refs
            string onText = _owner.OnText ?? "";
            string offText = _owner.OffText ?? "";
            float padding = _owner.Padding;
            float progress = _owner.animationProgress;

            // pick a font size relative to height
            float fontSize = height * 0.38f; // similar to Skia code
            canvas.FontSize = fontSize;

            
// create an IFont instance (adjust family name if you want)
            var font = new Microsoft.Maui.Graphics.Font("Arial"); // implements IFont

// measure text widths using the required overload
            var onSize = canvas.GetStringSize(onText, font, fontSize);
            var offSize = canvas.GetStringSize(offText, font, fontSize);

            float onWidth = onSize.Width + padding * 5;
            float offWidth = offSize.Width + padding * 5;
            float gap = height * 0.035f;

            float totalWidth = onWidth + offWidth + gap;
            float startX = (width - totalWidth) / 2f;
            float midY = height / 2f;

            float onStart = startX;
            float offStart = onStart + onWidth + gap;

            float pillHeight = height * 0.6f;
            float pillRadius = pillHeight / 2f;
            float pillY = midY - pillRadius;

            float rightEdge = offStart + offWidth;

            // Draw static background pill
            canvas.FillColor = _owner.PillBackgroundColor;
            canvas.FillRoundedRectangle(onStart, pillY, rightEdge - onStart, pillHeight, pillRadius);

            // Animated active pill rectangle (interpolates start + width)
            float animStart = Lerp(offStart, onStart, progress);
            float animWidth = Lerp(offWidth, onWidth, progress);

            canvas.FillColor = _owner.PillColor;
            canvas.FillRoundedRectangle(animStart, pillY, animWidth, pillHeight, pillRadius);

            // Draw unselected text (on top of bg)
            canvas.FontColor = _owner.UnselectedTextColor;
            // left text rects
            var onTextRect = new RectF(onStart + padding * 2, pillY, onWidth, pillHeight);
            var offTextRect = new RectF(offStart + padding, pillY, offWidth, pillHeight);

            canvas.DrawString(onText, onTextRect, HorizontalAlignment.Left, VerticalAlignment.Center);
            canvas.DrawString(offText, offTextRect, HorizontalAlignment.Left, VerticalAlignment.Center);

            // Draw selected text clipped to active pill area
            // Clip to the active rectangle so selected text only shows inside it
            var activeRect = new RectF(animStart, pillY, animWidth, pillHeight);

            // Save, clip, draw selected text, restore
            canvas.SaveState();
            canvas.ClipRectangle(activeRect);
            canvas.FontColor = _owner.SelectedTextColor;
            canvas.DrawString(onText, onTextRect, HorizontalAlignment.Left, VerticalAlignment.Center);
            canvas.DrawString(offText, offTextRect, HorizontalAlignment.Left, VerticalAlignment.Center);
            canvas.RestoreState();
        }

        static float Lerp(float from, float to, float t) => from + (to - from) * t;
}