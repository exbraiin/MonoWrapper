using Microsoft.Xna.Framework;
using System;

namespace MonoWrapper
{
    /// <summary>
    ///     The game manager class responsible for the framework logic.
    /// </summary>
    public static class Application
    {
        internal static readonly InternalGame game = new InternalGame();

        /// <summary>
        ///     Whether the elapsed time should be fixed or not.
        /// </summary>
        public static bool IsFixedElapsedTime
        {
            get => game.IsFixedTimeStep;
            set => game.IsFixedTimeStep = value;
        }

        /// <summary>
        ///     The target elapsed time in seconds.
        ///     This value is only used if <see cref="IsFixedElapsedTime"/> is <c>true</c>.
        /// </summary>
        public static float TargetElapsedTime
        {
            get => (float)game.TargetElapsedTime.TotalSeconds;
            set => game.TargetElapsedTime = TimeSpan.FromSeconds(value);
        }

        /// <summary>
        ///     Runs the application.
        /// </summary>
        public static void Run ()
        {
            Window.ApplyChanges();
            game.Run();
            game.Dispose();
        }

        /// <summary>
        ///     Exits the application.
        /// </summary>
        public static void Exit ()
        {
            game.Exit();
        }
    }

    internal sealed partial class InternalGame : Game
    {
        protected override void Initialize ()
        {
            SceneManager.Lock();
            base.Initialize();
        }

        protected override void LoadContent ()
        {
            SceneManager.LoadFirstScene();
        }

        protected override void Update (GameTime gameTime)
        {
            Time.Update(gameTime);
            Input.Update();

            SceneManager.Update();
            base.Update(gameTime);
        }

        protected override void Draw (GameTime gameTime)
        {
            SceneManager.Draw();
            base.Draw(gameTime);
        }
    }
}
