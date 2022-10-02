using Microsoft.Xna.Framework;

namespace MonoWrapper
{
    public static class ColorExtensions
    {
        /// Returns this color with the given R component.
        public static Color WithR(this Color color, byte r) => new Color(r, color.G, color.B, color.A);

        /// Returns this color with the given G component.
        public static Color WithG(this Color color, byte g) => new Color(color.R, g, color.B, color.A);

        /// Returns this color with the given B component.
        public static Color WithB(this Color color, byte b) => new Color(color.R, color.G, b, color.A);

        /// Returns this color with the given A component.
        public static Color WithA(this Color color, byte a) => new Color(color.R, color.G, color.B, a);
    }
}
