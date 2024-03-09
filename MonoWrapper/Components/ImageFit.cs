using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoWrapper
{
    public enum BoxFit
    {
        None,
        Fill,
        Cover,
        Contain,
        FitWidth,
        FitHeight,
        ScaleDown,
    }

    public static class ImageFit
    {
        private static Rectangle Center(Point size, Point target)
        {
            var x = Mathf.FloorToInt(target.X - size.X / 2f);
            var y = Mathf.FloorToInt(target.Y - size.Y / 2f);
            return new Rectangle(x, y, size.X, size.Y);
        }

        public static void Draw(SpriteBatch batch, Texture2D texture, BoxFit fit, Rectangle? rectangle = null, Color? color = null)
        {
            var bounds = rectangle ?? new Rectangle(Point.Zero, Window.Size);
            var rect = GetBounds(bounds, texture.Bounds.Size.ToVector2(), fit);

            // Crop texture to fit bounds.
            var dx = Mathf.Max(rect.Width - bounds.Width, 0);
            var dy = Mathf.Max(rect.Height - bounds.Height, 0);
            var tx = texture.Width * dx / rect.Width;
            var ty = texture.Height * dy / rect.Height;
            var src = new Rectangle(tx / 2, ty / 2, texture.Width - tx, texture.Height - ty);
            rect = Rectangle.Intersect(rect, bounds);

            batch.Draw(texture, rect, src, color ?? Color.White);
        }

        private static Rectangle GetBounds(Rectangle bounds, Vector2 size, BoxFit fit)
        {
            Point parseRatio(Func<Vector2, Vector2> parse)
            {
                var ratio = bounds.Size.ToVector2() / size;
                return (size * parse(ratio)).ToPoint();
            }

            switch (fit)
            {
                case BoxFit.None:
                    var pt0 = size.ToPoint();
                    return Center(pt0, bounds.Center);
                case BoxFit.Fill:
                    var pt1 = parseRatio((v) => v);
                    return Center(pt1, bounds.Center);
                case BoxFit.Cover:
                    var pt2 = parseRatio((v) => Vector2.One * Mathf.Max(v.X, v.Y));
                    return Center(pt2, bounds.Center);
                case BoxFit.Contain:
                    var pt3 = parseRatio((v) => Vector2.One * Mathf.Min(v.X, v.Y));
                    return Center(pt3, bounds.Center);
                case BoxFit.FitWidth:
                    var pt4 = parseRatio((v) => Vector2.One * v.X);
                    return Center(pt4, bounds.Center);
                case BoxFit.FitHeight:
                    var pt5 = parseRatio((v) => Vector2.One * v.Y);
                    return Center(pt5, bounds.Center);
                case BoxFit.ScaleDown:
                    var pt6 = parseRatio((v) => Vector2.One * Mathf.Min(v.X, v.Y, 1));
                    return Center(pt6, bounds.Center);
            }

            return Rectangle.Empty;
        }
    }


}
