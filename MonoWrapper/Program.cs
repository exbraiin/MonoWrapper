#if DEBUG
using MonoWrapper.Testing;
using System;

namespace MonoWrapper
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            // SceneManager.AddScene("main", () => new MetricsScene());
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
}
#endif