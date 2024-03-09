using Microsoft.Xna.Framework;

namespace MonoWrapper
{
    /// <summary>
    ///     Base interface for all game scenes.
    /// </summary>
    public abstract class Scene
    {
        /// <summary>
        ///     The scene background color.
        /// </summary>
        public virtual Color BackgroundColor { get; } = Color.Black;

        /// <summary>
        ///     Initializes and loads scene content.
        /// </summary>
        public virtual void Initialize () { }

        /// <summary>
        ///     Disposes scene content.
        /// </summary>
        public virtual void Dispose () { }

        /// <summary>
        ///     Updates scene.
        /// </summary>
        public virtual void Update () { }

        /// <summary>
        ///     Draws scene.
        /// </summary>
        public virtual void Draw () { }
    }
}
