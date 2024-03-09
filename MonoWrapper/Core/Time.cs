using Microsoft.Xna.Framework;

namespace MonoWrapper
{
    /// <summary>
    ///     Module responsible for managing the time.
    /// </summary>
    public static class Time
    {
        /// <summary>
        ///     Total number of frames.
        /// </summary>
        public static int FrameCount { get; private set; }

        /// <summary>
        ///     The total game time in seconds.
        /// </summary>
        public static double TotalTimeAsDouble { get; private set; }

        /// <summary>
        ///     The elapsed game time in seconds.
        /// </summary>
        public static double DeltaTimeAsDouble { get; private set; }

        /// <summary>
        ///     The total game time in seconds.
        /// </summary>
        public static float TotalTime { get; private set; }

        /// <summary>
        ///     The elapsed game time in seconds.
        /// </summary>
        public static float DeltaTime { get; private set; }

        /// <summary>
        ///     Updates the <see cref="Time"/> internally.
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        internal static void Update (GameTime gameTime)
        {
            FrameCount++;
            TotalTimeAsDouble = gameTime.TotalGameTime.TotalSeconds;
            DeltaTimeAsDouble = gameTime.ElapsedGameTime.TotalSeconds;
            TotalTime = (float)TotalTimeAsDouble;
            DeltaTime = (float)DeltaTimeAsDouble;
        }
    }
}
