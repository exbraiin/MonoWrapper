using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoWrapper
{
    /// <summary>
    ///     A camera sytenm to provide the graphics device matrix.
    /// </summary>
    public sealed class Camera2D
    {
        private bool isDirty = true;
        private Matrix matrix;
        private Vector2 viewport;

        private Vector2 position;
        public Vector2 Position
        {
            get => position;
            set
            {
                if (position == value) return;
                isDirty = true;
                position = value;
            }
        }

        private Vector2 scale = Vector2.One;
        public Vector2 Scale
        {
            get => scale;
            set
            {
                if (scale == value) return;
                isDirty = true;
                scale = value;
            }
        }

        private float rotation;
        public float Rotation
        {
            get => rotation;
            set
            {
                if (rotation == value) return;
                isDirty = true;
                rotation = value;
            }
        }

        /// <summary>
        ///     Evaluates the matrix to be passed to the <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="graphics">The window graphics device</param>
        /// <returns>The evaluated matrix</returns>
        public Matrix Matrix
        {
            get
            {
                GraphicsDevice graphics = Window.GraphicsDevice;
                Vector2 screen = graphics.Viewport.Bounds.Size.ToVector2();
                if (screen != viewport)
                {
                    isDirty = true;
                    viewport = screen;
                }

                if (isDirty)
                {
                    isDirty = false;
                    Matrix mtxPos = Matrix.CreateTranslation(-position.ToVector3());
                    Matrix mtxRot = Matrix.CreateRotationZ(rotation);
                    Matrix mtxScl = Matrix.CreateScale(scale.ToVector3(1));
                    Matrix mtxMdl = Matrix.CreateTranslation(screen.ToVector3() * 0.5f);
                    matrix = mtxPos * mtxRot * mtxScl * mtxMdl;
                }
                return matrix;
            }
        }

        /// <summary>
        ///     Converts the given screen vector into a world position vector.
        /// </summary>
        /// <param name="vector">The vector to be converted</param>
        /// <returns>The given vector in world space</returns>
        public Vector2 ScreenToWorld(Vector2 vector)
        {
            var mp = vector;
            var mt = Matrix.Invert(Matrix);
            Vector2.Transform(ref mp, ref mt, out Vector2 dir);
            return dir;
        }

        /// <summary>
        ///     Converts the given world vector into a screen position vector.
        /// </summary>
        /// <param name="vector">The vector to be converted</param>
        /// <returns>The given vector in screen space</returns>
        public Vector2 WorldToScreen(Vector2 vector)
        {
            var mp = vector;
            var mt = Matrix;
            Vector2.Transform(ref mp, ref mt, out Vector2 dir);
            return dir;
        }
    }
}
