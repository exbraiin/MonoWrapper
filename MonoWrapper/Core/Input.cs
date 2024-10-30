using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoWrapper
{
    public static class Input
    {
        public static bool NumLock => _currentKeyboardState.NumLock;
        public static bool CapsLock => _currentKeyboardState.CapsLock;
        public static Point MouseScroll
        {
            get
            {
                var py = _previousMouseState.ScrollWheelValue;
                var px = _previousMouseState.HorizontalScrollWheelValue;
                var cy = _currentMouseState.ScrollWheelValue;
                var cx = _currentMouseState.HorizontalScrollWheelValue;

                var x = px < cx ? 1 : px > cx ? -1 : 0;
                var y = py < cy ? 1 : py > cy ? -1 : 0;

                return new Point(x, y);
            }
        }
        public static Point MousePosition => _currentMouseState.Position;
        public static int MaximumGamePadCount => statesGamePad.Length;

        private static MouseState _currentMouseState = new MouseState();
        private static MouseState _previousMouseState = new MouseState();

        private static KeyboardState _currentKeyboardState = new KeyboardState();
        private static KeyboardState _previousKeyboardState = new KeyboardState();

        private static readonly GamepadState[] statesGamePad;

        static Input()
        {
            statesGamePad = Enumerable.Range(0, GamePad.MaximumGamePadCount).Select(i => new GamepadState(i)).ToArray();
        }

        internal static void Update()
        {
            UpdateMouse();
            UpdateKeyboard();
            UpdateGamePads();
        }

        private static void UpdateKeyboard()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
        }

        private static void UpdateMouse()
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }

        private static void UpdateGamePads()
        {
            foreach (var state in statesGamePad) state.Update();
        }

        /// <summary>
        ///     Gets whether the given <see cref="KeyCode"/> is pressed or held in the current frame.
        /// </summary>
        public static bool GetKey(KeyCode key)
        {
            return _currentKeyboardState.IsKeyDown((Keys)key);
        }

        /// <summary>
        ///     Gets whether the given <see cref="KeyCode"/> is released in the current frame.
        /// </summary>
        public static bool GetKeyUp(KeyCode key)
        {
            return _previousKeyboardState.IsKeyDown((Keys)key) && _currentKeyboardState.IsKeyUp((Keys)key);
        }

        /// <summary>
        ///     Gets whether the given <see cref="KeyCode"/> is pressed in the current frame.
        /// </summary>
        public static bool GetKeyDown(KeyCode key)
        {
            return _previousKeyboardState.IsKeyUp((Keys)key) && _currentKeyboardState.IsKeyDown((Keys)key);
        }

        /// <summary>
        ///     Gets whether the given mouse button (by index) is pressed or held in the current frame.
        ///     <para>Index 0 if the left button, 1 right button, 2 middle button, 3 and 4 the extra buttons.</para>
        /// </summary>
        public static bool GetMouseButton(int index)
        {
            return _currentMouseState.GetButtonState(index) == ButtonState.Pressed;
        }

        /// <summary>
        ///     Gets whether the given mouse button (by index) is released in the current frame.
        ///     <para>Index 0 if the left button, 1 right button, 2 middle button, 3 and 4 the extra buttons.</para>
        /// </summary>
        public static bool GetMouseButtonUp(int index)
        {
            return _previousMouseState.GetButtonState(index) == ButtonState.Pressed && _currentMouseState.GetButtonState(index) == ButtonState.Released;
        }

        /// <summary>
        ///     Gets whether the given mouse button (by index) is pressed in the current frame.
        ///     <para>Index 0 if the left button, 1 right button, 2 middle button, 3 and 4 the extra buttons.</para>
        /// </summary>
        public static bool GetMouseButtonDown(int index)
        {
            return _previousMouseState.GetButtonState(index) == ButtonState.Released && _currentMouseState.GetButtonState(index) == ButtonState.Pressed;
        }

        /// <summary>
        ///     Gets the gamepad state for the given index.
        ///     Index should not be negative or bigger than <see cref="MaximumGamePadCount"/>.
        /// </summary>
        public static GamepadState GetGamepad(int index)
        {
            return 0 <= index && index < statesGamePad.Length ? statesGamePad[index] : default;
        }
    }

    public sealed class GamepadState
    {
        private float vibration;

        private GamePadState _currentState;
        private GamePadState _previousState;

        public int Index { get; }
        public int PacketNumber => _currentState.PacketNumber;
        public bool IsConnected => _currentState.IsConnected;

        public float LeftTrigger => _currentState.Triggers.Left;
        public float RightTrigger => _currentState.Triggers.Right;

        public Vector2 LeftThumbStick => _currentState.ThumbSticks.Left;
        public Vector2 RightThumbStick => _currentState.ThumbSticks.Right;

        internal GamepadState(int index)
        {
            Index = index;
        }

        internal void Update()
        {
            if (vibration > 0)
            {
                vibration -= Time.DeltaTime;
                if (vibration <= 0) SetVibration(0);
            }

            _previousState = _currentState;
            _currentState = GamePad.GetState(Index);
        }

        /// <summary>
        ///     Sets the gamepad vibration for the left and right motors with the same intesity.
        ///     If the value of <paramref name="seconds"/> is negative the vibration is continously.
        /// </summary>
        public void SetVibration(float amount, float seconds = -1)
        {
            vibration = seconds;
            GamePad.SetVibration(Index, amount, amount);
        }

        /// <summary>
        ///     Sets the gamepad vibration for the left and right motors.
        ///     If the value of <paramref name="seconds"/> is negative the vibration is continously.
        /// </summary>
        public void SetVibration(float left, float right, float seconds = -1)
        {
            vibration = seconds;
            GamePad.SetVibration(Index, left, right);
        }

        /// <summary>
        ///     Gets whether the given <see cref="Buttons"/> is pressed or held in the current frame.
        /// </summary>
        public bool GetButton(GamepadButton button)
        {
            return _currentState.IsButtonDown((Buttons)button);
        }

        /// <summary>
        ///     Gets whether the given <see cref="Buttons"/> is released in the current frame.
        /// </summary>
        public bool GetButtonUp(GamepadButton button)
        {
            return _previousState.IsButtonDown((Buttons)button) && _currentState.IsButtonUp((Buttons)button);
        }

        /// <summary>
        ///     Gets whether the given <see cref="Buttons"/> is pressed in the current frame.
        /// </summary>
        public bool GetButtonDown(GamepadButton button)
        {
            return _previousState.IsButtonUp((Buttons)button) && _currentState.IsButtonDown((Buttons)button);
        }
    }

    internal static class InputExtensions
    {
        public static ButtonState? GetButtonState(this MouseState mouse, int index)
        {
            return index switch
            {
                0 => mouse.LeftButton,
                1 => mouse.RightButton,
                2 => mouse.MiddleButton,
                3 => mouse.XButton1,
                4 => mouse.XButton2,
                _ => null,
            };
        }
    }
}
