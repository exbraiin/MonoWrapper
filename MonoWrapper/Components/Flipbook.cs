using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoWrapper
{
    public enum Sequence
    {
        Forward,
        Loop,
        PingPong,
    }

    /// <summary>
    ///     Creates a flipbook that changes the sprite bounds to render it.
    /// </summary>
    public class Flipbook
    {
        /// Current frame.
        private int iFrame;

        /// Current frame time.
        private float frameTime;

        /// Current sprite frame size.
        private Point size;

        /// Total amount of seconds per frame.
        private readonly float frameDuration;

        /// <summary>
        ///     Gets or sets the velocity of the flipbook.
        /// </summary>
        public float Velocity { set; get; }

        /// <summary>
        ///     Gets the current frame index.
        /// </summary>
        public int Frame { private set; get; }

        /// <summary>
        ///     Gets the amount of total frames.
        /// </summary>
        public int TotalFrames { private set; get; }

        /// <summary>
        ///     Gets if the system is paused.
        /// </summary>
        public bool Paused { private set; get; }

        /// <summary>
        ///     Gets the amount of frames per second.
        /// </summary>
        public float FPS { private set; get; }

        /// <summary>
        ///     Gets the sequence type.
        /// </summary>
        public Sequence Sequence { private set; get; }

        /// <summary>
        ///     Get the current frame bounds on the main texture.
        /// </summary>
        public Rectangle Bounds { private set; get; }

        /// <summary>
        ///     Gets the flibook main texture.
        /// </summary>
        public Texture2D Texture { private set; get; }

        /// <summary>
        ///     Creates a new <see cref="Flipbook"/> instance.
        /// </summary>
        /// <param name="texture">The flipbook texture</param>
        /// <param name="sheetSize">The sheet size</param>
        /// <param name="fps">The amount of frames per second</param>
        /// <param name="sprites">The amount of sprites, if null the full sheet is used</param>
        /// <param name="sequence">The flipbook sequence</param>
        public Flipbook (Texture2D texture, Point sheetSize, int fps, int? sprites = null, Sequence sequence = Sequence.Forward)
        {
            size = sheetSize;
            Paused = sequence == Sequence.Forward;
            Sequence = sequence;

            FPS = fps;
            Velocity = 1;
            Texture = texture;
            TotalFrames = sprites ?? sheetSize.X * sheetSize.Y;

            frameDuration = 1f / fps;

            Frame = iFrame = 0;
            UpdateBounds();
        }

        private void UpdateBounds ()
        {
            int x = Frame % size.X;
            int y = Frame / size.X;
            int w = Texture.Width / size.X;
            int h = Texture.Height / size.Y;
            Bounds = new Rectangle(x * w, y * h, w, h);
        }

        /// <summary>
        ///     Updates the flipbook sequence.
        ///     <para>To be called on the scene Update method</para>
        /// </summary>
        public void Update ()
        {
            if (Paused) return;
            frameTime += Time.DeltaTime * Math.Abs(Velocity);
            if (frameTime < frameDuration) return;

            frameTime = 0;
            iFrame += Velocity < 0 ? -1 : 1;
            if (Sequence == Sequence.Forward)
            {
                Frame = Mathf.Clamp(iFrame, 0, TotalFrames - 1);
                Paused = Frame == 0 || Frame == TotalFrames - 1;
            }
            else if (Sequence == Sequence.Loop)
            {
                Frame = (int)Mathf.Repeat(iFrame, TotalFrames);
            }
            else if (Sequence == Sequence.PingPong)
            {
                Frame = (int)Mathf.PinpPong(iFrame, TotalFrames - 1);
            }

            UpdateBounds();
            if (Frame == 0 && (iFrame < 0 || iFrame > TotalFrames)) iFrame = 0;
        }

        /// <summary>
        ///     Plays the flipbook
        /// </summary>
        public void Play ()
        {
            Paused = false;
        }

        /// <summary>
        ///     Pauses the flipbook
        /// </summary>
        public void Pause ()
        {
            Paused = true;
        }

        /// <summary>
        ///     Resets the flipbook
        /// </summary>
        public void Reset ()
        {
            frameTime = 0;
            Frame = iFrame = 0;
            UpdateBounds();
        }
    }

    /// <summary>
    ///     The flipbook extensions.
    /// </summary>
    public static class FlipbookExtensions
    {
        private static Rectangle InnerRectangle (Rectangle source, Rectangle? inner)
        {
            if (!inner.HasValue) return source;
            return new Rectangle(source.Location + inner.Value.Location, inner.Value.Size);
        }

        /// <summary>
        ///     Draws the given flipbook.
        /// </summary>
        public static void Draw (this SpriteBatch spriteBatch, Flipbook texture, Rectangle destinationRectangle, Color color)
        {
            spriteBatch.Draw(texture.Texture, destinationRectangle, texture.Bounds, color);
        }

        /// <summary>
        ///     Draws the given flipbook.
        /// </summary>
        public static void Draw (this SpriteBatch spriteBatch, Flipbook texture, Vector2 position, Color color)
        {
            spriteBatch.Draw(texture.Texture, position, texture.Bounds, color);
        }

        /// <summary>
        ///     Draws the given flipbook.
        /// </summary>
        public static void Draw (this SpriteBatch spriteBatch, Flipbook texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            spriteBatch.Draw(texture.Texture, position, InnerRectangle(texture.Bounds, sourceRectangle), color);
        }

        /// <summary>
        ///     Draws the given flipbook.
        /// </summary>
        public static void Draw (this SpriteBatch spriteBatch, Flipbook texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(texture.Texture, destinationRectangle, InnerRectangle(texture.Bounds, sourceRectangle), color, rotation, origin, effects, layerDepth);
        }

        /// <summary>
        ///     Draws the given flipbook.
        /// </summary>
        public static void Draw (this SpriteBatch spriteBatch, Flipbook texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            spriteBatch.Draw(texture.Texture, destinationRectangle, InnerRectangle(texture.Bounds, sourceRectangle), color);
        }

        /// <summary>
        ///     Draws the given flipbook.
        /// </summary>
        public static void Draw (this SpriteBatch spriteBatch, Flipbook texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(texture.Texture, position, InnerRectangle(texture.Bounds, sourceRectangle), color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>
        ///     Draws the given flipbook.
        /// </summary>
        public static void Draw (this SpriteBatch spriteBatch, Flipbook texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(texture.Texture, position, InnerRectangle(texture.Bounds, sourceRectangle), color, rotation, origin, scale, effects, layerDepth);
        }
    }
}
