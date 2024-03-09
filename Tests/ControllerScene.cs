#if DEBUG
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoWrapper.Experimental;
using System.Collections.Generic;
using System.Text;

namespace MonoWrapper.Testing
{
    public class ControllerScene : Scene
    {
        private SpriteFont font;
        private SpriteBatch batch;
        private ParticleSystem particleSystem;

        public override void Initialize()
        {
            Window.IsMouseVisible = true;
            Window.AllowUserResizing = true;
            batch = new SpriteBatch(Window.GraphicsDevice);
            font = Resources.Load<SpriteFont>("Arial");

            var text = new Texture2D(Window.GraphicsDevice, 24, 24);

            var colors = new Color[24*24];
            for(int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.White;
            }
            text.SetData(colors);

            particleSystem = new ParticleSystem(text);
            particleSystem.Debug = true;
            particleSystem.Location = new Vector2(100, 100);
        }

        public override void Dispose()
        {
            batch.Dispose();
        }

        public override void Update()
        {
            particleSystem.Update();
        }

        public override void Draw()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"GamePads: {Input.MaximumGamePadCount}");
            builder.AppendLine(GamePad.GetState(PlayerIndex.One).IsConnected.ToString());

            var connected = -1;
            for (var i = 0; i < Input.MaximumGamePadCount; ++i)
            {
                if (!Input.GetGamepad(i).IsConnected) continue;
                builder.AppendLine($"Connected: {i}");
            }

            if (connected != -1)
            {
                var gp = Input.GetGamepad(connected);
                builder.AppendLine(gp.IsConnected.ToString());
                foreach (var bt in EnumUtils.GetValues<GamepadButton>())
                {
                    builder.AppendLine($"{bt}: {gp.GetButton(bt)}");
                }
            }

            batch.Begin();
            batch.DrawString(font, builder, Vector2.Zero, Color.White);
            particleSystem.Draw(batch);
            batch.End();
        }
    }
}
#endif