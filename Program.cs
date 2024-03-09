#if DEBUG
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoWrapper.Testing;
using System;
using System.Text;

namespace MonoWrapper
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {

            // SceneManager.AddScene("lol", () => new TestingWrapper());
            // SceneManager.AddScene("main", () => new MetricsScene());
            SceneManager.AddScene("controller", () => new ControllerScene());
            SceneManager.AddScene("", () => new MetricsScene());
            SceneManager.AddScene("main", () => new TextBarScene());

            Application.Run();

            // Window.Size = new Point(640, 480);
            // Window.IsMouseVisible = true;
            // Window.AllowUserResizing = true;

            // SceneManager.AddScene("main", () => new TestScene());
            // SceneManager.AddScene("counter", () => new CounterScene());
            // Application.Run();
        }
    }

    public class TestingWrapper : Scene
    {
        private SpriteFont _font;
        private SpriteBatch _batch;

        public override void Initialize()
        {
            Window.Title = "MonoWrapper";
            Window.IsMouseVisible = true;
            Window.Size = new Point(800, 450);
            Window.ApplyChanges();
            _font = Resources.Load<SpriteFont>("Arial");
            _batch = new SpriteBatch(Window.GraphicsDevice);
        }

        public override void Dispose()
        {
            _batch.Dispose();
        }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) Application.Exit();
        }

        public override void Draw()
        {
            _batch.Begin();
            _batch.DrawString(_font, "Hello World!", Vector2.Zero, Color.Teal);
            _batch.End();
        }
    }
}
#endif