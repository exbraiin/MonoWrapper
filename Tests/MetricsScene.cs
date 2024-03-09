#if DEBUG
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;

namespace MonoWrapper.Testing
{
    public class MetricsScene : Scene
    {
        private SpriteFont font;
        private SpriteBatch batch;


        public override Color BackgroundColor => Color.Gray;

        public override void Initialize ()
        {
            Window.Title = "Metrics";
            Window.IsMouseVisible = true;
            Window.AllowUserResizing = true;
            batch = new SpriteBatch(Window.GraphicsDevice);
            font = Resources.Load<SpriteFont>("Arial");
        }

        public override void Dispose ()
        {
            batch.Dispose();
        }

        public override void Update ()
        {
            var gp = Input.GetGamepad(0);
            if (gp.GetButtonDown(GamepadButton.Back))
            {
                Application.Exit();
                return;
            }

            gp.SetVibration(gp.LeftTrigger, gp.RightTrigger, -1);
            if (Input.GetKeyDown(KeyCode.N)) SceneManager.LoadScene("counter");
        }

        public override void Draw ()
        {

            Gizmos.DrawVsRectangle(batch, new Rectangle(200, 200, 100, 100), Color.Red);
            Gizmos.DrawVsRectangleFill(batch, new Rectangle(300, 300, 100, 100), Color.Blue);

            Gizmos.DrawVsCircle(batch, Vector2.One * 300, 100, Color.Yellow);

            Gizmos.DrawVsLine(batch, new Vector2(100, 100), new Vector2(200, 200), Color.Orange);
            Gizmos.DrawVsLines(batch, new[] { new Vector2(10, 10), new Vector2(100, 20), new Vector2(200, 100) }, Color.Aqua, true);

            Gizmos.DrawVsRectangleFill(batch, new Rectangle(300, 20, 1, 1), Color.Red);


            batch.Begin();
            var builder = new StringBuilder()
                .AppendLine($"ClearCount: {Window.GraphicsDevice.Metrics.ClearCount}")
                .AppendLine($"DrawCount: {Window.GraphicsDevice.Metrics.DrawCount}")
                .AppendLine($"PixelShaderCount: {Window.GraphicsDevice.Metrics.PixelShaderCount}")
                .AppendLine($"PrimitiveCount: {Window.GraphicsDevice.Metrics.PrimitiveCount}")
                .AppendLine($"SpriteCount: {Window.GraphicsDevice.Metrics.SpriteCount}")
                .AppendLine($"TargetCount: {Window.GraphicsDevice.Metrics.TargetCount}")
                .AppendLine($"TextureCount: {Window.GraphicsDevice.Metrics.TextureCount}")
                .AppendLine($"VertexShaderCount: {Window.GraphicsDevice.Metrics.VertexShaderCount}")
                .AppendLine()
                .AppendLine($"Frames: {Time.FrameCount}")
                .AppendLine($"Total Time: {Time.TotalTime}")
                .AppendLine($"Delta Time: {Time.DeltaTime}")
                .AppendLine()
                .AppendLine($"Mouse Position: {Input.MousePosition}")
                .AppendLine($"Space: {Input.GetKey(KeyCode.Space)}")
                .AppendLine($"Left: {Input.GetMouseButton(0)}")
                .AppendLine($"Mouse Scroll: {Input.MouseScroll}")
                .AppendLine($"GamePads Supported: {Input.MaximumGamePadCount}");

            AppendGamepad(builder, Input.GetGamepad(0));
            batch.DrawString(font, builder, Vector2.One * 10, Color.Black);
            batch.End();
        }

        private void AppendGamepad (StringBuilder builder, GamepadState gamePad)
        {
            builder
                .AppendLine()
                .AppendLine($"GamePad Index: {gamePad.Index}")
                .AppendLine($"Connected: {gamePad.IsConnected}")
                .AppendLine($"Packet: {gamePad.PacketNumber}")
                .AppendLine($"Lt Trigger: {gamePad.LeftTrigger}")
                .AppendLine($"Rt Trigger: {gamePad.RightTrigger}")
                .AppendLine($"Lt Thumb: {gamePad.LeftThumbStick}")
                .AppendLine($"Rt Thumb: {gamePad.RightThumbStick}");

            builder.AppendLine();

            foreach (var button in EnumUtils.GetValues<GamepadButton>())
            {
                builder.AppendLine($"{button}: {gamePad.GetButton(button)}");
            }
        }
    }
}
#endif