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
            // SceneManager.AddScene("controller", () => new ControllerScene());
            // SceneManager.AddScene("", () => new MetricsScene());
            // SceneManager.AddScene("main", () => new TextBarScene());
            SceneManager.AddScene("console", () => new ConsoleTest());

            Application.Run();

            // Window.Size = new Point(640, 480);
            // Window.IsMouseVisible = true;
            // Window.AllowUserResizing = true;

            // SceneManager.AddScene("main", () => new TestScene());
            // SceneManager.AddScene("counter", () => new CounterScene());
            // Application.Run();
        }
    }

    public class ConsoleTest : Scene
    {
        public override void Update()
        {

            var down = Input.GetKeyDown(KeyCode.P);
            var key = Input.GetKey(KeyCode.P);
            var up = Input.GetKeyUp(KeyCode.P);

            if (down || key || up)
            {
                Console.WriteLine(Time.FrameCount);
                Console.WriteLine($"KeyDown: {down}");
                Console.WriteLine($"Key: {key}");
                Console.WriteLine($"KeyUp: {up}");
            }

            var mdown = Input.GetMouseButtonDown(0);
            var mkey = Input.GetMouseButton(0);
            var mup = Input.GetMouseButtonUp(0);

            if (mdown || mkey || mup)
            {
                Console.WriteLine(Time.FrameCount);
                Console.WriteLine($"KeyDown: {mdown}");
                Console.WriteLine($"Key: {mkey}");
                Console.WriteLine($"KeyUp: {mup}");
            }

            Console.WriteLine(Input.MouseScroll);
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