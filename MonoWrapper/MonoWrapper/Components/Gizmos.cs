using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace MonoWrapper
{
    public static class Gizmos
    {

        private static Texture2D texture;
        private static Texture2D Texture
        {
            get
            {
                if (texture == null)
                {
                    texture = new Texture2D(Window.GraphicsDevice, 1, 1);
                    texture.SetData(new[] { Color.White });
                }
                return texture;
            }
        }

        private static BasicEffect effect;
        private static BasicEffect Effect
        {
            get
            {
                if (effect == null)
                {
                    effect = new BasicEffect(Window.GraphicsDevice);
                    effect.VertexColorEnabled = true;
                    effect.Projection = Matrix.CreateOrthographicOffCenter(0, Window.Width, Window.Height, 0, 0, 1);
                    Window.OnSizeChanged += (size) => effect.Projection = Matrix.CreateOrthographicOffCenter(0, size.X, size.Y, 0, 0, 1);
                }
                return effect;
            }
        }

        /// <summary>
        ///     Draws a rectangle.
        /// </summary>
        public static void DrawRectangle (SpriteBatch spriteBatch, Rectangle rectangle, Color color, int width = 1)
        {
            if (width < 1 || width * 2 >= rectangle.Width || width * 2 >= rectangle.Height)
            {
                spriteBatch.Draw(Texture, rectangle, color);
            }
            else
            {
                spriteBatch.Draw(Texture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, width), color);
                spriteBatch.Draw(Texture, new Rectangle(rectangle.X, rectangle.Y, width, rectangle.Height), color);
                spriteBatch.Draw(Texture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - width, rectangle.Width, width), color);
                spriteBatch.Draw(Texture, new Rectangle(rectangle.X + rectangle.Width - width, rectangle.Y, width, rectangle.Height), color);
            }
        }

        /// <summary>
        ///     Draws a rectangle using vertex shader.
        /// </summary>
        public static void DrawVsRectangle (SpriteBatch batch, Rectangle rectangle, Color color)
        {
            var vertex = new[]
            {
                new VertexPositionColor(new Vector3(rectangle.Left, rectangle.Top, 0), color),
                new VertexPositionColor(new Vector3(rectangle.Right, rectangle.Top, 0), color),
                new VertexPositionColor(new Vector3(rectangle.Right, rectangle.Bottom, 0), color),
                new VertexPositionColor(new Vector3(rectangle.Left, rectangle.Bottom, 0), color),
                new VertexPositionColor(new Vector3(rectangle.Left, rectangle.Top, 0), color),
            };
            batch.Begin(effect: Effect);
            Effect.CurrentTechnique.Passes[0].Apply();
            Window.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertex, 0, 4);
            batch.End();
        }

        /// <summary>
        ///     Draws a fileld rectangle using vertex shader.
        /// </summary>
        public static void DrawVsRectangleFill (SpriteBatch batch, Rectangle rectangle, Color color)
        {
            var vertex = new[]
            {
                new VertexPositionColor(new Vector3(rectangle.Left, rectangle.Top, 0), color),
                new VertexPositionColor(new Vector3(rectangle.Right, rectangle.Top, 0), color),
                new VertexPositionColor(new Vector3(rectangle.Right, rectangle.Bottom, 0), color),
                new VertexPositionColor(new Vector3(rectangle.Left, rectangle.Top, 0), color),
                new VertexPositionColor(new Vector3(rectangle.Right, rectangle.Bottom, 0), color),
                new VertexPositionColor(new Vector3(rectangle.Left, rectangle.Bottom, 0), color),
            };

            batch.Begin(effect: Effect);
            Effect.CurrentTechnique.Passes[0].Apply();
            Window.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex, 0, 2);
            batch.End();
        }

        /// <summary>
        ///     Draws a point.
        /// </summary>
        public static void DrawPoint (SpriteBatch spriteBatch, Vector2 a, Color color)
        {
            spriteBatch.Draw(Texture, a, color);
        }

        /// <summary>
        ///     Draws a line.
        /// </summary>
        public static void DrawLine (SpriteBatch spriteBatch, Vector2 a, Vector2 b, Color color, int width = 1)
        {
            float angle = Mathf.Atan2(b.Y - a.Y, b.X - a.X);
            int length = Mathf.RoundToInt(Vector2.Distance(a, b));
            spriteBatch.Draw(Texture, new Rectangle(a.ToPoint(), new Point(length, width)), null, color, angle, new Vector2(0, 1 / 2f), SpriteEffects.None, 0);
        }

        /// <summary>
        ///     Draws a line using vertex shader.
        /// </summary>
        public static void DrawVsLine (SpriteBatch batch, Vector2 a, Vector2 b, Color color)
        {
            var vertex = new[]
            {
                new VertexPositionColor(a.ToVector3(), color),
                new VertexPositionColor(b.ToVector3(), color),
            };
            batch.Begin(effect: Effect);
            Effect.CurrentTechnique.Passes[0].Apply();
            Window.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertex, 0, 1);
            batch.End();
        }

        /// <summary>
        ///     Draws several lines.
        /// </summary>
        public static void DrawLines (SpriteBatch spriteBatch, Vector2[] points, Color color, bool close = false, int width = 1)
        {
            if (points.Length < 2) return;
            for (int i = 0; i < points.Length - 1; ++i)
                DrawLine(spriteBatch, points[i], points[i + 1], color, width);
            if (close) DrawLine(spriteBatch, points[0], points[points.Length - 1], color, width);
        }

        /// <summary>
        ///     Draws several lines using vertex shader.
        /// </summary>
        public static void DrawVsLines (SpriteBatch batch, Vector2[] points, Color color, bool close = false)
        {
            if (points.Length < 2) return;
            var vertex = points.Select(i => new VertexPositionColor(i.ToVector3(), color)).ToList();
            if (close) vertex.Add(new VertexPositionColor(points[0].ToVector3(), color));

            batch.Begin(effect: Effect);
            Effect.CurrentTechnique.Passes[0].Apply();
            Window.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertex.ToArray(), 0, vertex.Count - 1);
            batch.End();
        }

        /// <summary>
        ///     Draws a circle.
        /// </summary>
        public static void DrawCircle (SpriteBatch spriteBatch, Vector2 center, float size, Color color, int width = 1)
        {
            const int SIDES = 20;
            const float ANGLE = 2 * Mathf.PI / SIDES;

            Vector2 toPoint (int i)
            {
                float rad = i * ANGLE;
                return center + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * size;
            }

            Vector2 previous = toPoint(SIDES - 1);
            for (int i = 0; i < SIDES; ++i)
            {
                Vector2 current = toPoint(i);
                DrawLine(spriteBatch, previous, current, color, width);
                previous = current;
            }
        }

        /// <summary>
        ///     Draws a circle using vertex shader.
        /// </summary>
        public static void DrawVsCircle (SpriteBatch batch, Vector2 center, float size, Color color, int sides = 40)
        {
            var vertex = new VertexPositionColor[sides + 1];
            float angle = 2 * Mathf.PI / sides;
            for (int i = -1; i < sides; ++i)
            {
                var rad = i * angle;
                var pos = new Vector3(center.X + Mathf.Cos(rad) * size, center.Y + Mathf.Sin(rad) * size, 0);
                vertex[i + 1] = new VertexPositionColor(pos, color);
            }

            batch.Begin(effect: Effect);
            Effect.CurrentTechnique.Passes[0].Apply();
            Window.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertex, 0, vertex.Length - 1);
            batch.End();
        }
    }
}
