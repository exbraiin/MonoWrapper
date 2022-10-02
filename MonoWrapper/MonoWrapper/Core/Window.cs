using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoWrapper
{
    /// <summary>
    ///     Module responsible for managing the window state.
    ///     <para>Wraps the <see cref="GameWindow"/> and <see cref="GraphicsDeviceManager"/> class.</para>
    /// </summary>
    public static class Window
    {
        public delegate void OnSizeChangedDelegate(Point size);

        /// The game window
        private static readonly GameWindow window;

        /// The game graphics device manager.
        private static readonly GraphicsDeviceManager graphics;

        static Window()
        {
            graphics = new GraphicsDeviceManager(Application.game);
            window = Application.game.Window;
            window.ClientSizeChanged += (sender, args) =>
              {
                  Size = window.ClientBounds.Size;
                  ApplyChanges();
                  OnSizeChanged?.Invoke(Size);
              };
        }

        public static event OnSizeChangedDelegate OnSizeChanged;

        /// <summary>
        ///     Gets the game <see cref="GraphicsDevice"/>.
        /// </summary>
        public static GraphicsDevice GraphicsDevice => graphics.GraphicsDevice;

        /// <summary>
        ///     Gets the game supported display modes.
        /// </summary>
        public static DisplayModeCollection SupportedDisplayModes => GraphicsAdapter.DefaultAdapter.SupportedDisplayModes;

        /// <summary>
        ///     Gets the Key Up event <see cref="GameWindow.KeyUp"/>
        /// </summary>
        public static event EventHandler<InputKeyEventArgs> KeyUp
        {
            add => window.KeyUp += value;
            remove => window.KeyUp -= value;
        }

        /// <summary>
        ///     Gets the Key Down event <see cref="GameWindow.KeyDown"/>
        /// </summary>
        public static event EventHandler<InputKeyEventArgs> KeyDown
        {
            add => window.KeyDown += value;
            remove => window.KeyDown -= value;
        }

        /// <summary>
        ///     Gets the Text Input event <see cref="GameWindow.TextInput"/>.
        /// </summary>
        public static event EventHandler<TextInputEventArgs> TextInput
        {
            add => window.TextInput += value;
            remove => window.TextInput -= value;
        }

        /// <summary>
        ///     Gets or sets the window size.
        /// </summary>
        public static Point Size
        {
            get => new Point(Width, Height);
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }

        /// <summary>
        ///     Gets orr sets the window vertical syncronization.
        /// </summary>
        public static bool VSync
        {
            get => graphics.SynchronizeWithVerticalRetrace;
            set => graphics.SynchronizeWithVerticalRetrace = value;
        }

        /// <summary>
        ///     Gets or sets the window width.
        /// </summary>
        public static int Width
        {
            get => graphics.PreferredBackBufferWidth;
            set => graphics.PreferredBackBufferWidth = value;
        }

        /// <summary>
        ///     Gets or sets the window height.
        /// </summary>
        public static int Height
        {
            get => graphics.PreferredBackBufferHeight;
            set => graphics.PreferredBackBufferHeight = value;
        }

        /// <summary>
        ///     Gets or sets the window as fullscreen.
        /// </summary>
        public static bool IsFullscreen
        {
            get => graphics.IsFullScreen;
            set => graphics.IsFullScreen = value;
        }

        /// <summary>
        ///     Gets or sets the mouse visibility.
        /// </summary>
        public static bool IsMouseVisible
        {
            get => Application.game.IsMouseVisible;
            set => Application.game.IsMouseVisible = value;
        }

        /// <summary>
        ///     Gets or sets the window as borderless.
        /// </summary>
        public static bool IsBorderless
        {
            get => window.IsBorderless;
            set => window.IsBorderless = value;
        }

        /// <summary>
        ///     Gets or sets the window title.
        /// </summary>
        public static string Title
        {
            get => window.Title;
            set => window.Title = value;
        }

        /// <summary>
        ///     Gets or sets the window position.
        /// </summary>
        public static Point Position
        {
            get => window.Position;
            set => window.Position = value;
        }

        /// <summary>
        ///     Gets or sets the allowability to close the window using the Alt+F4 shortcut.
        /// </summary>
        public static bool AllowAltF4
        {
            get => window.AllowAltF4;
            set => window.AllowAltF4 = value;
        }

        /// <summary>
        ///     Gets or sets the user allowability to change the windows size.
        /// </summary>
        public static bool AllowUserResizing
        {
            get => window.AllowUserResizing;
            set => window.AllowUserResizing = value;
        }

        /// <summary>
        ///     Gets the window bounds.
        /// </summary>
        public static Rectangle ClientBounds => window.ClientBounds;

        /// <summary>
        ///     Applies the pendent changes to the window.
        /// </summary>
        public static void ApplyChanges()
        {
            // Forces graphics buffer size update.
            graphics.PreferredBackBufferWidth = Size.X;
            graphics.PreferredBackBufferHeight = Size.Y;
            graphics.ApplyChanges();
        }

        /// <summary>
        ///     Toggles the fullscreen of the window.
        /// </summary>
        public static void ToggleFullScreen()
        {
            graphics.ToggleFullScreen();
        }
    }
}
