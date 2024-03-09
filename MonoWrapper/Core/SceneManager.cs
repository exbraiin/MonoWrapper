using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoWrapper
{
    /// <summary>
    ///     Module responsible for loading scenes.
    /// </summary>
    public static class SceneManager
    {
        /// Scenes mapping.
        private static readonly Dictionary<string, Func<Scene>> scenes = new Dictionary<string, Func<Scene>>();

        /// Current scene.
        private static Scene scene;

        /// Scene manager locking flag.
        private static bool lockScenes;

        /// <summary>
        ///     Gets the current scene name.
        /// </summary>
        public static string CurrentScene { get; private set; }

        /// <summary>
        ///     Gets the scenes added to the <see cref="SceneManager"/>.
        /// </summary>
        public static string[] Scenes => scenes.Keys.ToArray();

        /// <summary>
        ///     Adds a new scene to the scene manager.
        /// </summary>
        /// <param name="sceneName">The scene name</param>
        /// <param name="scene">The scene instance</param>
        public static void AddScene (string sceneName, Func<Scene> scene)
        {
            if (lockScenes)
                throw new Exception("Cannot add scene on current scope!");

            scenes.Add(sceneName, scene);
        }

        /// <summary>
        ///     Loads the scene with the mapped sceneName.
        /// </summary>
        /// <param name="sceneName">The scene name</param>
        public static void LoadScene (string sceneName)
        {
            if (!scenes.ContainsKey(sceneName))
                throw new Exception($"No scene found for {sceneName}");

            Scene newScene = scenes[sceneName]?.Invoke();
            scene?.Dispose();
            newScene.Initialize();
            scene = newScene;
            CurrentScene = sceneName;
        }

        /// <summary>
        ///     Locks the scene manager state.
        /// </summary>
        internal static void Lock () => lockScenes = true;

        /// <summary>
        ///     Loads the first configured scene.
        /// </summary>
        internal static void LoadFirstScene ()
        {
            if (scenes.Count < 1)
                throw new Exception("No scenes found, add a scene using the SceneManager!");

            Window.ApplyChanges();
            LoadScene(scenes.Keys.First());
        }

        /// <summary>
        ///     Updates the current scene.
        /// </summary>
        internal static void Update () => scene.Update();

        /// <summary>
        ///     Draws the current scene.
        /// </summary>
        internal static void Draw ()
        {
            Window.GraphicsDevice.Clear(scene.BackgroundColor);
            scene.Draw();
        }
    }
}
