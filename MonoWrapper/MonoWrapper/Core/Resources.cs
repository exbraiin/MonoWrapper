using Microsoft.Xna.Framework.Content;

namespace MonoWrapper
{
    /// <summary>
    ///     Module responsible for managing game resources.
    ///     <para>Wraps the <see cref="ContentManager"/> class.</para>
    /// </summary>
    public static class Resources
    {
        /// The content manager instance.
        private static readonly ContentManager content;

        static Resources()
        {
            content = Application.game.Content;
            content.RootDirectory = "Content";
        }

        /// <summary>
        ///     Gets or sets the resources root directory.
        /// </summary>
        public static string RootDirectory
        {
            get => content.RootDirectory;
            set => content.RootDirectory = value;
        }

        /// <summary>
        ///     Loads the asset of type <see cref="{T}"/> by the given assetName.
        /// </summary>
        /// <typeparam name="T">The asset type</typeparam>
        /// <param name="assetName">The asset name relative to the root directory</param>
        /// <returns>Returns the loaded asset.</returns>
        public static T Load<T>(string assetName)
        {
            return content.Load<T>(assetName);
        }

        /// <summary>
        ///     Loads the asset of type <see cref="{T}"/> by the given assetName.
        /// </summary>
        /// <typeparam name="T">The asset type</typeparam>
        /// <param name="assetName">The asset name relative to the root directory</param>
        /// <returns>Returns the loaded asset.</returns>
        public static T LoadLocalized<T>(string assetName)
        {
            return content.LoadLocalized<T>(assetName);
        }

        /// <summary>
        ///     Unloads all the current loaded resources.
        /// </summary>
        public static void Unload()
        {
            content.Unload();
        }
    }
}
