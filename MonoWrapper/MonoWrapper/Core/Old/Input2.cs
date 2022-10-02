/*
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoWrapper
{
    internal enum KeyState { None, Down, Hold, Released, }

    public static class Input
    {
        public static bool NumLock { get; private set; }
        public static bool CapsLock { get; private set; }
        public static Point MouseScroll { get; private set; }
        public static Point MousePosition => Mouse.GetState().Position;
        public static int MaximumGamePadCount => statesGamePad.Length;

        private static Point lastMouseScroll;

        private static MouseState? stateMouse;
        private static KeyboardState? stateKeyboard;

        private static readonly KeyState[] statesMouse;
        private static readonly Dictionary<Keys, KeyState> statesKeyboard;
        private static readonly GamepadState[] statesGamePad;

        static Input ()
        {
            statesMouse = new KeyState[5];
            statesKeyboard = new Dictionary<Keys, KeyState>(EnumUtils.GetValues<Keys>().Select(k => KeyValuePair.Create(k, KeyState.None)));
            statesGamePad = Enumerable.Range(0, GamePad.MaximumGamePadCount).Select(i => new GamepadState(i)).ToArray();
        }

        internal static void Update ()
        {
            UpdateMouse();
            UpdateKeyboard();
            UpdateGamePads();
        }

        private static void UpdateKeyboard ()
        {
            var state = Keyboard.GetState();
            if (state == stateKeyboard) return;
            stateKeyboard = state;

            NumLock = state.NumLock;
            CapsLock = state.CapsLock;

            foreach (var key in statesKeyboard.Keys.ToArray())
            {
                var down = state.IsKeyDown(key);
                var next = statesKeyboard[key] = statesKeyboard[key].Next(down);
                if (next != KeyState.None) stateKeyboard = default;
            }
        }

        private static void UpdateMouse ()
        {
            var state = MouseButtonsState();
            if (state == stateMouse) return;
            stateMouse = state;

            var states = new[] { state.LeftButton, state.RightButton, state.MiddleButton, state.XButton1, state.XButton2 };
            for (var i = 0; i < statesMouse.Length; ++i)
            {
                var next = statesMouse[i] = statesMouse[i].Next(states[i] == ButtonState.Pressed);
                if (next != KeyState.None) stateMouse = default;
            }

            var my = state.ScrollWheelValue;
            var mx = state.HorizontalScrollWheelValue;

            var x = lastMouseScroll.X < mx ? 1 : lastMouseScroll.X > mx ? -1 : 0;
            var y = lastMouseScroll.Y < my ? 1 : lastMouseScroll.Y > my ? -1 : 0;

            MouseScroll = new Point(x, y);
            if (MouseScroll != Point.Zero) stateMouse = default;

            lastMouseScroll.X = mx;
            lastMouseScroll.Y = my;
        }

        private static void UpdateGamePads ()
        {
            foreach (var state in statesGamePad) state.Update();
        }

        private static MouseState MouseButtonsState ()
        {
            var state = Mouse.GetState();
            return new MouseState(
                0, 0, state.ScrollWheelValue,
                state.LeftButton, state.MiddleButton, state.RightButton,
                state.XButton1, state.XButton2, state.HorizontalScrollWheelValue);
        }

        /// <summary>
        ///     Gets whether the given <see cref="Keys"/> is pressed or holded in the current frame.
        /// </summary>
        public static bool GetKey (Keys key)
        {
            return statesKeyboard.ContainsKey(key) && (statesKeyboard[key] == KeyState.Down || statesKeyboard[key] == KeyState.Hold);
        }

        /// <summary>
        ///     Gets whether the given <see cref="Keys"/> is released in the current frame.
        /// </summary>
        public static bool GetKeyUp (Keys key)
        {
            return statesKeyboard.ContainsKey(key) && statesKeyboard[key] == KeyState.Released;
        }

        /// <summary>
        ///     Gets whether the given <see cref="Keys"/> is pressed in the current frame.
        /// </summary>
        public static bool GetKeyDown (Keys key)
        {
            return statesKeyboard.ContainsKey(key) && statesKeyboard[key] == KeyState.Down;
        }

        /// <summary>
        ///     Gets whether the given mouse button (by index) is pressed or holded in the current frame.
        ///     <para>Index 0 if the left button, 1 right button, 2 middle button, 3 and 4 the extra buttons.</para>
        /// </summary>
        public static bool GetMouseButton (int index)
        {
            return 0 <= index && index < statesMouse.Length && statesMouse[index] == KeyState.Down || statesMouse[index] == KeyState.Hold;
        }

        /// <summary>
        ///     Gets whether the given mouse button (by index) is released in the current frame.
        ///     <para>Index 0 if the left button, 1 right button, 2 middle button, 3 and 4 the extra buttons.</para>
        /// </summary>
        public static bool GetMouseButtonUp (int index)
        {
            return 0 <= index && index < statesMouse.Length && statesMouse[index] == KeyState.Released;
        }

        /// <summary>
        ///     Gets whether the given mouse button (by index) is pressed in the current frame.
        ///     <para>Index 0 if the left button, 1 right button, 2 middle button, 3 and 4 the extra buttons.</para>
        /// </summary>
        public static bool GetMouseButtonDown (int index)
        {
            return 0 <= index && index < statesMouse.Length && statesMouse[index] == KeyState.Down;
        }

        /// <summary>
        ///     Gets the gamepad state for the given index.
        ///     Index should not be negative or bigger than <see cref="MaximumGamePadCount"/>.
        /// </summary>
        public static GamepadState GetGamepad (int index)
        {
            return 0 <= index && index < statesGamePad.Length ? statesGamePad[index] : default;
        }
    }

    public sealed class GamepadState
    {
        private float vibration;
        private GamePadState? lastState;
        private GamePadState State => GamePad.GetState(Index);
        private readonly Dictionary<Buttons, KeyState> states;

        public int Index { get; }
        public int PacketNumber => State.PacketNumber;
        public bool IsConnected => State.IsConnected;

        public float LeftTrigger => State.Triggers.Left;
        public float RightTrigger => State.Triggers.Right;

        public Vector2 LeftThumbStick => State.ThumbSticks.Left;
        public Vector2 RightThumbStick => State.ThumbSticks.Right;

        internal GamepadState (int index)
        {
            Index = index;
            states = new Dictionary<Buttons, KeyState>(EnumUtils.GetValues<Buttons>().Select(k => KeyValuePair.Create(k, KeyState.None)));
        }

        internal void Update ()
        {
            if (lastState == State) return;
            lastState = State;

            if (vibration > 0)
            {
                vibration -= Time.ElapsedTime;
                if (vibration <= 0) SetVibration(0);
                else lastState = default;
            }

            foreach (var key in states.Keys.ToArray())
            {
                var next = states[key] = states[key].Next(State.IsButtonDown(key));
                if (next != KeyState.None) lastState = default;
            }
        }

        /// <summary>
        ///     Sets the gamepad vibration for the left and right motors with the same intesity.
        ///     If the value of <paramref name="seconds"/> is negative the vibration is continously.
        /// </summary>
        public void SetVibration (float amount, float seconds = -1)
        {
            vibration = seconds;
            GamePad.SetVibration(Index, amount, amount);
        }

        /// <summary>
        ///     Sets the gamepad vibration for the left and right motors.
        ///     If the value of <paramref name="seconds"/> is negative the vibration is continously.
        /// </summary>
        public void SetVibration (float left, float right, float seconds = -1)
        {
            vibration = seconds;
            GamePad.SetVibration(Index, left, right);
        }

        /// <summary>
        ///     Gets whether the given <see cref="Buttons"/> is pressed or holded in the current frame.
        /// </summary>
        public bool GetButton (Buttons button)
        {
            return states.ContainsKey(button) && (states[button] == KeyState.Down || states[button] == KeyState.Hold);
        }

        /// <summary>
        ///     Gets whether the given <see cref="Buttons"/> is released in the current frame.
        /// </summary>
        public bool GetButtonUp (Buttons button)
        {
            return states.ContainsKey(button) && states[button] == KeyState.Released;
        }

        /// <summary>
        ///     Gets whether the given <see cref="Buttons"/> is pressed in the current frame.
        /// </summary>
        public bool GetButtonDown (Buttons button)
        {
            return states.ContainsKey(button) && states[button] == KeyState.Down;
        }
    }

    internal static class InputExtensions
    {
        public static KeyState Next (this KeyState state, bool down)
        {
            var s = state == KeyState.None && down || state == KeyState.Down || state == KeyState.Hold && !down || state == KeyState.Released;
            return (KeyState)(((int)state + (s ? 1 : 0)) % 4);
        }
    }

    internal static class EnumUtils
    {
        public static T[] GetValues<T> () where T : struct
        {
            return (T[])Enum.GetValues(typeof(T));
        }
    }
}
*/