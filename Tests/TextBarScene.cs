#if DEBUG
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace MonoWrapper.Testing
{
    public class TextBarScene : Scene
    {
        private SpriteFont font;
        private SpriteBatch batch;
        private TextController text;

        public override void Initialize()
        {
            Window.Size = new Point(800, 100);
            Window.Title = "Text Bar";
            Window.AllowUserResizing = false;
            Window.IsMouseVisible = true;
            Window.ApplyChanges();

            font = Resources.Load<SpriteFont>("Arial");
            batch = new SpriteBatch(Window.GraphicsDevice);
            text = new TextController(font, false);
            text.Initialize();
        }

        public override void Dispose()
        {
            text.Dispose();
            batch.Dispose();
        }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) Application.Exit();
            if (Input.GetKeyDown(KeyCode.Enter)) Console.WriteLine(text.Text);
        }

        public override void Draw()
        {
            var off = new Vector2(10, 10);
            Window.GraphicsDevice.Clear(Color.Black);

            batch.Begin(blendState: BlendState.NonPremultiplied);
            text.Draw(batch, off, Color.White);
            batch.End();
        }
    }

    public class TextController
    {
        private int idx0 = 0;
        private int idx1 = 0;
        private bool lShift = false;
        private bool rShift = false;
        private string text = string.Empty;

        private readonly float height;
        public readonly bool multiline;
        public readonly SpriteFont font;

        public string Text => text;

        private bool IsShiftOn => lShift || rShift;
        private (int min, int max) MinMax => idx0 < idx1 ? (idx0, idx1) : (idx1, idx0);

        // Multiline does not work.
        // Selection is not beign correctly assigned.
        // Home and End keys move the selection to the whole text instead of line start and line end.
        public TextController(SpriteFont font, bool multiline = false)
        {
            this.font = font;
            this.multiline = multiline;
            this.height = this.font.MeasureString(" ").Y;
        }

        public void Initialize()
        {
            Window.KeyUp += KeyUpEvent;
            Window.KeyDown += KeyDownEvent;
            Window.TextInput += TextInputEvent;
        }

        public void Dispose()
        {
            Window.KeyUp -= KeyUpEvent;
            Window.KeyDown -= KeyDownEvent;
            Window.TextInput -= TextInputEvent;
        }

        private (Vector2 ptr, float min, float max) Offsets()
        {
            var w0 = text[..idx0].Split('\n');
            var l0 = w0.Last();
            var x0 = font.MeasureString(l0).X;

            var x1 = x0;
            if (idx0 != idx1)
            {
                var w1 = text[..idx1].Split('\n');
                var l1 = w1.Last();
                x1 = font.MeasureString(l1).X;
            }

            var y0 = (w0.Length - 1) * height;
            var ptr = new Vector2(x0, y0);

            return idx1 < idx0 ? (ptr, x1, x0) : (ptr, x0, x1);
        }

        public void Draw(SpriteBatch batch, Vector2 position, Color color)
        {
            // Offsets
            (var ptr, var min, var max) = Offsets();

            // Draw selection
            var sstr = position + new Vector2(min, 0);
            var srct = new Vector2(Mathf.Abs(min - max), height);
            var rect = new Rectangle(sstr.ToPoint(), srct.ToPoint());
            Gizmos.DrawRectangle(batch, rect, Color.Gray, 0);

            // Draw text
            batch.DrawString(font, text, position, color);

            // Draw caret
            var cstr = position + ptr;
            var cend = position + ptr + Vector2.UnitY * height;
            var caph = Mathf.PinpPong(Time.TotalTime, 1) * byte.MaxValue;
            var cclr = Color.White.WithA((byte)caph);
            Gizmos.DrawLine(batch, cstr, cend, cclr, 2);
        }

        private void TextInputEvent(object sender, TextInputEventArgs e)
        {
            if (e.Key == Keys.Back) TextDelete(false);
            else if (e.Key == Keys.Delete) TextDelete(true);
            else if (font.Characters.Contains(e.Character)) TextWrite(e.Character);
            else if (e.Key == Keys.Enter && multiline) TextWrite('\n');
            // else Console.WriteLine($"Text Input: {e.Key} > {e.Character}");
        }

        private void KeyUpEvent(object sender, InputKeyEventArgs e)
        {
            if (e.Key == Keys.LeftShift) lShift = false;
            else if (e.Key == Keys.RightShift) rShift = false;
            // else Console.WriteLine($"Key Up: {e.Key}");
        }

        private void KeyDownEvent(object sender, InputKeyEventArgs e)
        {
            if (e.Key == Keys.Left) MoveCursorOnce(false);
            else if (e.Key == Keys.Right) MoveCursorOnce(true);
            else if (e.Key == Keys.Home) UpdateCursor(0, IsShiftOn);
            else if (e.Key == Keys.End) UpdateCursor(text.Length, IsShiftOn);
            else if (e.Key == Keys.LeftShift) lShift = true;
            else if (e.Key == Keys.RightShift) rShift = true;
            // else Console.WriteLine($"Key Down: {e.Key}");
        }

        private void TextDelete(bool positive)
        {
            void remove(int start, int count)
            {
                var abs = Mathf.Abs(count);
                var nend = Mathf.Min(start, start + count);
                var nstr = Mathf.Max(0, nend);
                var ncnt = Mathf.Clamp(nend + abs - nstr, 0, text.Length - nstr);
                text = text.Remove(nstr, ncnt);
            }

            var ptr = MinMax.min;
            if (idx0 != idx1)
            {
                remove(ptr, Mathf.Abs(idx0 - idx1));
                UpdateCursor(ptr);
            }
            else
            {
                (int cnt, int off) = positive ? (1, 0) : (-1, -1);
                remove(ptr, cnt);
                UpdateCursor(ptr + off);
            }
        }

        private void TextWrite(char character)
        {
            var (min, max) = MinMax;
            text = text[..min] + character + text[max..];
            UpdateCursor(min + 1);
        }

        private void UpdateCursor(int idx, bool keepOther = false)
        {
            idx0 = Mathf.Clamp(idx, 0, text.Length);
            if (!keepOther) idx1 = idx0;
        }

        private void MoveCursorOnce(bool positive)
        {
            var dir = positive ? 1 : -1;
            if (idx0 == idx1 || IsShiftOn) UpdateCursor(idx0 + dir, IsShiftOn);
            else if (positive) UpdateCursor(MinMax.max);
            else UpdateCursor(MinMax.min);
        }
    }
}
#endif