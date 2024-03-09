using Microsoft.Xna.Framework;
using System;

namespace MonoWrapper
{
    /// <summary>
    ///     <see cref="Random"/> call extension methods.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        ///     Returns a random bool value.
        /// </summary>
        /// <param name="random">The random instance.</param>
        public static bool NextBool(this Random random)
        {
            return random.Next(2) != 0;
        }

        /// <summary>
        ///     Returns a random floating-point number that is greater than or equal to 0.0,
        ///     and less than 1.0.
        /// </summary>
        /// <param name="random">The random instance.</param>
        /// <param name="max">The maximum value</param>
        public static float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }

        /// <summary>
        ///     Returns a random floating-point number that is greater than or equal to 0.0,
        ///     and less than <c>max</c>.
        /// </summary>
        /// <param name="random">The random instance.</param>
        /// <param name="max">The maximum value</param>
        public static float NextFloat(this Random random, float max)
        {
            return max * random.NextFloat();
        }

        /// <summary>
        ///     Returns a random floating-point number that is greater than or equal to <c>min</c>,
        ///     and less than <c>max</c>.
        /// </summary>
        /// <param name="random">The random instance.</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        public static float NextFloat(this Random random, float min, float max)
        {
            return min + (max - min) * random.NextFloat();
        }

        /// <summary>
        ///     Returns a random floating-point number that is greater than or equal to 0.0,
        ///     and less than <c>max</c>.
        /// </summary>
        /// <param name="random">The random instance.</param>
        /// <param name="max">The maximum value</param>
        public static double NextDouble(this Random random, double max)
        {
            return max * random.NextDouble();
        }

        /// <summary>
        ///     Returns a random floating-point number that is greater than or equal to <c>min</c>,
        ///     and less than <c>max</c>.
        /// </summary>
        /// <param name="random">The random instance.</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        public static double NextDouble(this Random random, double min, double max)
        {
            return min * (max - min) * random.NextDouble();
        }

        /// <summary>
        ///     Returns a random <see cref="Vector2"/> where both component are greater than or equals to 0.0,
        ///     and less than 1.0.
        /// </summary>
        /// <param name="random">~The random instance.</param>
        public static Vector2 NextVector2(this Random random)
        {
            return new Vector2(random.NextFloat(), random.NextFloat());
        }

        /// <summary>
        ///     Returns a random <see cref="Vector2"/> where both component are greater than or equals to 0.0,
        ///     and less than <c>max</c>.
        /// </summary>
        /// <param name="random">~The random instance.</param>
        public static Vector2 NextVector2(this Random random, Vector2 max)
        {
            return new Vector2(random.NextFloat(max.X), random.NextFloat(max.Y));
        }

        /// <summary>
        ///     Returns a random <see cref="Vector2"/> where both component are greater than or equals to <c>min</c>,
        ///     and less than <c>max</c>.
        /// </summary>
        /// <param name="random">The random instance.</param>
        public static Vector2 NextVector2(this Random random, Vector2 min, Vector2 max)
        {
            return new Vector2(random.NextFloat(min.X, max.X), random.NextFloat(min.Y, max.Y));
        }

        /// <summary>
        ///     Returns a non-negative random <see cref="Point"/> where both components are less than System.Int32.MaxValue.
        /// </summary>
        /// <param name="random">The random instance.</param>
        public static Point NextPoint(this Random random)
        {
            return new Point(random.Next(), random.Next());
        }

        /// <summary>
        ///     Returns a random <see cref="Point"/> where both components are greates or equals to 0.0,
        ///     and less than <c>max</c>.
        /// </summary>
        /// <param name="random">The random instance.</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        public static Point NextPoint(this Random random, Point max)
        {
            return new Point(random.Next(max.X), random.Next(max.Y));
        }

        /// <summary>
        ///     Returns a random <see cref="Point"/> where both components are greates or equals to <c>min</c>,
        ///     and less than <c>max</c>.
        /// </summary>
        /// <param name="random">The random instance.</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        public static Point NextPoint(this Random random, Point min, Point max)
        {
            return new Point(random.Next(min.X, max.X), random.Next(min.Y, max.Y));
        }
    }
}
