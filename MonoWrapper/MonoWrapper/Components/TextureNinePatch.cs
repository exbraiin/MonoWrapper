using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoWrapper
{
    public struct Borders
    {
        public readonly int Left, Top, Right, Bottom;

        private Borders(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public static Borders All(int border) => new Borders(border, border, border, border);
        public static Borders LTRB(int l, int t, int r, int b) => new Borders(l, t, r, b);
    }

    public static class SpriteBatchExtensions
    {
        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Rectangle destinationRectangle, Borders borders, Color color)
        {
            int rx = destinationRectangle.X, ry = destinationRectangle.Y;
            int rw = destinationRectangle.Width, rh = destinationRectangle.Height;
            int lt = borders.Left, up = borders.Top;
            int rt = borders.Right, dw = borders.Bottom;

            int dx0 = rx, dx1 = rx + lt, dx2 = rx + lt + rw - lt - rt;
            int dy0 = ry, dy1 = ry + up, dy2 = ry + up + rh - up - dw;
            int dw0 = lt, dw1 = rw - lt - rt, dw2 = rt;
            int dh0 = up, dh1 = rh - up - dw, dh2 = dw;

            int sx0 = 0, sx1 = lt, sx2 = texture.Height - rt;
            int sy0 = 0, sy1 = up, sy2 = texture.Height - dw;
            int sw0 = lt, sw1 = texture.Width - lt - rt, sw2 = rt;
            int sh0 = up, sh1 = texture.Height - up - dw, sh2 = dw;

            spriteBatch.Draw(texture, new Rectangle(dx0, dy0, dw0, dh0), new Rectangle(sx0, sy0, sw0, sh0), color);
            spriteBatch.Draw(texture, new Rectangle(dx1, dy0, dw1, dh0), new Rectangle(sx1, sy0, sw1, sh0), color);
            spriteBatch.Draw(texture, new Rectangle(dx2, dy0, dw2, dh0), new Rectangle(sx2, sy0, sw2, sh0), color);

            spriteBatch.Draw(texture, new Rectangle(dx0, dy1, dw0, dh1), new Rectangle(sx0, sy1, sw0, sh1), color);
            spriteBatch.Draw(texture, new Rectangle(dx1, dy1, dw1, dh1), new Rectangle(sx1, sy1, sw1, sh1), color);
            spriteBatch.Draw(texture, new Rectangle(dx2, dy1, dw2, dh1), new Rectangle(sx2, sy1, sw2, sh1), color);

            spriteBatch.Draw(texture, new Rectangle(dx0, dy2, dw0, dh2), new Rectangle(sx0, sy2, sw0, sh2), color);
            spriteBatch.Draw(texture, new Rectangle(dx1, dy2, dw1, dh2), new Rectangle(sx1, sy2, sw1, sh2), color);
            spriteBatch.Draw(texture, new Rectangle(dx2, dy2, dw2, dh2), new Rectangle(sx2, sy2, sw2, sh2), color);
        }
    }
}
