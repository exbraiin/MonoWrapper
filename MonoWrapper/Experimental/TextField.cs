using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoWrapper.Experimental
{
    public class TextField : IDisposable
    {

        /// The key repeat delay
        private float keyDelay = 0;

        /// The last pressed key
        private KeyCode lastKey = KeyCode.None;

        /// Text color.
        public Color TextColor = Color.White;

        /// Cursor color.
        public Color CursorColor = Color.White;

        /// Select color.
        public Color SelectColor = Color.LightBlue;

        /// The amount of seconds between each cursor blink.
        public float CursorBlinkRate = 1f;

        private int cursorIndex;
        private int cursorPinIndex;

        private bool drawCursor;
        private float blinkCooldown;

        /// Whether the field is ready to receive input.
        public bool Active { get; set; }

        /// The field text.
        private string text;
        public string Text
        {
            get => text;
            set => MoveCursor((text = value).Length, true);
        }

        public event EventHandler<string> OnSubmit;

        /// The field font.
        public SpriteFont Font { get; private set; }

        /// <summary>
        ///     Creates a new <see cref="TextField"/> instance.
        /// </summary>
        /// <param name="font">Text field font</param>
        public TextField (SpriteFont font)
        {
            Font = font;
            text = string.Empty;
            Window.TextInput += TextInputEvent;
        }

        public void Dispose ()
        {
            Window.TextInput -= TextInputEvent;
        }

        private void TextInputEvent (object sender, TextInputEventArgs textEvent)
        {
            if (!Active) return;
            if (textEvent.Key == Keys.Back) return;
            if (textEvent.Key == Keys.Delete) return;
            if (!Font.Characters.Contains(textEvent.Character)) return;

            EditText(textEvent.Character);
        }

        private bool IsKey (KeyCode key)
        {
            if (Input.GetKeyDown(key))
            {
                keyDelay = 0.5f;
                lastKey = key;
                return true;
            }

            if (Input.GetKey(key) && lastKey == key && keyDelay == 0)
            {
                keyDelay = 1f / 30f;
                lastKey = key;
                return true;
            }

            return false;
        }

        private void DeleteSelected (int offset = 0)
        {
            if (cursorIndex == cursorPinIndex)
                cursorPinIndex = Mathf.Clamp(cursorIndex + offset, 0, text.Length);

            if (cursorIndex == cursorPinIndex) return;
            var start = Mathf.Min(cursorIndex, cursorPinIndex);
            var count = Mathf.Abs(cursorIndex - cursorPinIndex);

            if (count == 0) return;
            text = text.Remove(start, count);
            cursorIndex = cursorPinIndex = start;
        }

        private void EditText (char character)
        {
            DeleteSelected();
            text = text.Insert(cursorIndex, character.ToString());
            cursorIndex = cursorPinIndex = cursorIndex + 1;
        }

        private void MoveCursor (int moveTo, bool forceMove = false)
        {
            var shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            if (shift) cursorIndex = Mathf.Clamp(moveTo, 0, text.Length);
            else if (cursorIndex != cursorPinIndex && !forceMove) cursorPinIndex = cursorIndex;
            else cursorIndex = cursorPinIndex = Mathf.Clamp(moveTo, 0, text.Length);
        }

        public void Update ()
        {
            if (!Active) return;
            if ((keyDelay -= Time.DeltaTime) < 0) keyDelay = 0;
            if ((blinkCooldown += Time.DeltaTime) > CursorBlinkRate)
            {
                blinkCooldown = 0;
                drawCursor = !drawCursor;
            }

            // Delete
            if (IsKey(KeyCode.Back)) DeleteSelected(-1);
            else if (IsKey(KeyCode.Delete)) DeleteSelected(1);

            // Special Movement
            else if (Input.GetKeyDown(KeyCode.Home)) MoveCursor(0, true);
            else if (Input.GetKeyDown(KeyCode.End)) MoveCursor(text.Length, true);

            // Cursor Movement
            else if (IsKey(KeyCode.Left)) MoveCursor(cursorIndex - 1);
            else if (IsKey(KeyCode.Right)) MoveCursor(cursorIndex + 1);

            // OnSubmit
            else if (Input.GetKeyDown(KeyCode.Enter)) OnSubmit?.Invoke(this, Text);
        }

        public void Draw (SpriteBatch batch, Rectangle rectangle)
        {
            // Select draw...
            var start = Mathf.Min(cursorIndex, cursorPinIndex);
            var count = Mathf.Abs(cursorIndex - cursorPinIndex);
            var text = this.text.Substring(start, count);
            var size0 = Font.MeasureString(text);
            var size1 = Font.MeasureString(this.text.Substring(0, start));
            var rect = new Rectangle((int)(rectangle.X + size1.X), rectangle.Y, (int)size0.X, Font.LineSpacing);
            Gizmos.DrawRectangle(batch, rect, SelectColor, 0);

            // Text draw...
            DrawText(batch, rectangle);

            // Cursor draw...
            if (drawCursor && Active)
            {
                var size = Font.MeasureString(this.text.Substring(0, cursorIndex));
                var l0 = new Vector2(rectangle.X + size.X, rectangle.Y);
                var l1 = new Vector2(rectangle.X + size.X, rectangle.Y + Font.LineSpacing);
                Gizmos.DrawLine(batch, l0, l1, CursorColor, 2);
            }
        }

        private void DrawText (SpriteBatch batch, Rectangle rectangle)
        {
            batch.DrawString(Font, text, rectangle.Location.ToVector2(), TextColor);
        }
    }
}
