using Microsoft.Xna.Framework;

namespace MonoWrapper
{
    public static class VectorExtensions
    {
        /// <summary>
        ///     Converts this vector to a <see cref="Vector3"/> with the given z compoent.
        /// </summary>
        /// <param name="vector">The vector to be converted</param>
        /// <param name="z">The Z parameter</param>
        /// <returns>Returns a <see cref="Vector3"/></returns>
        public static Vector3 ToVector3 (this Vector2 vector, float z = 0)
        {
            return new Vector3(vector.X, vector.Y, z);
        }

        /// <summary>
        ///     Converts this vector to a <see cref="Vector2"/> discarding the z compoent.
        /// </summary>
        /// <param name="vector">The vector to be converted</param>
        /// <returns>Returns a <see cref="Vector2"/></returns>
        public static Vector2 ToVector2 (this Vector3 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }
    }
}
