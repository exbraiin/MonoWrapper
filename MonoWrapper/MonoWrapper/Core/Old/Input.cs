/*
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using XInput = Microsoft.Xna.Framework.Input;

namespace MonoWrapper.Old
{
    /// <summary>
    ///     Module responsible for managing the input state.
    ///     <para>Wraps the <see cref="XInput.GamePadState"/>, <see cref="XInput.KeyboardState"/> and <see cref="XInput.MouseState"/> class.</para>
    /// </summary>
    public static class Input
    {
        /// Current gamepad states.
        private static readonly GamePadState[] gamePads;

        /// <summary>
        ///     Gets the mouse state.
        /// </summary>
        public static MouseState Mouse { get; } = new MouseState();

        /// <summary>
        ///     Gets the keyboard state.
        /// </summary>
        public static KeyboardState Keyboard { get; } = new KeyboardState();

        /// <summary>
        ///     Gets the maximum number of supported gamepads.
        /// </summary>
        public static int MaximumGamePadCount => XInput.GamePad.MaximumGamePadCount;

        /// <summary>
        ///     Gets the gamepad state by the given index.
        /// </summary>
        /// <param name="index">The gamepad index</param>
        /// <returns>Returns the gamepad state at the given index</returns>
        public static GamePadState GamePad (int index) => gamePads[index];

        static Input ()
        {
            gamePads = new GamePadState[XInput.GamePad.MaximumGamePadCount];
            for (int i = 0; i < gamePads.Length; ++i) gamePads[i] = new GamePadState(i);
        }

        internal static void Update ()
        {
            Mouse.Update();
            Keyboard.Update();
            foreach (var gamePad in gamePads) gamePad.Update();
        }
    }

    internal enum KeyState { None, Down, Hold, Released, }

    /// <summary>
    ///     The mouse buttons.
    /// </summary>
    public enum MouseButton { None, Left, Middle, Right, X1, X2, }

    /// <summary>
    ///     The mouse state.
    /// </summary>
    public sealed class MouseState
    {
        /// The mouse states.
        private readonly KeyState[] states = new KeyState[6];

        /// <summary>
        ///     Gets the mouse X position.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        ///     Gets the mouse Y position.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        ///     Gets the scroll wheel value.
        /// </summary>
        public int ScrollWheelValue { get; private set; }

        /// <summary>
        ///     Gets the mouse position.
        /// </summary>
        public Point Position => new Point(X, Y);

        /// <summary>
        ///     Gets if the given button is down.
        /// </summary>
        /// <param name="button">The mouse button</param>
        public bool IsButton (MouseButton button)
        {
            var state = states[(int)button];
            return state == KeyState.Down || state == KeyState.Hold;
        }

        /// <summary>
        ///     Gets if the given button is up (once).
        ///     This state is reseted on the next frame.
        /// </summary>
        /// <param name="button">The mouse button</param>
        public bool IsButtonUp (MouseButton button)
        {
            return states[(int)button] == KeyState.Released;
        }

        /// <summary>
        ///     Gets if the given button is down (once).
        ///     This state is reseted on the next frame.
        /// </summary>
        /// <param name="button">The mouse button</param>
        public bool IsButtonDown (MouseButton button)
        {
            return states[(int)button] == KeyState.Down;
        }

        internal void Update ()
        {
            XInput.MouseState state = XInput.Mouse.GetState();
            X = state.X;
            Y = state.Y;
            ScrollWheelValue = state.ScrollWheelValue;

            var buttons = new[] { XInput.ButtonState.Released, state.LeftButton, state.MiddleButton, state.RightButton, state.XButton1, state.XButton2 };
            for (int i = 1; i < buttons.Length; ++i) states[i] = states[i].Next(buttons[i] == XInput.ButtonState.Pressed);
        }
    }

    /// <summary>
    ///     The keyboard state.
    /// </summary>
    public sealed class KeyboardState
    {
        /// The caps lock current state.
        public bool CapsLock { get; private set; }

        /// The keyboard states.
        private readonly Dictionary<XInput.Keys, KeyState> states = new Dictionary<XInput.Keys, KeyState>();

        /// <summary>
        ///     Gets if the given key is down.
        /// </summary>
        /// <param name="key">The keyboard key</param>
        public bool IsKey (XInput.Keys key)
        {
            if (!states.ContainsKey(key)) return false;
            var state = states[key];
            return state == KeyState.Down || state == KeyState.Hold;
        }

        /// <summary>
        ///     Gets if the given key is up (once).
        ///     This state is reseted on the next frame.
        /// </summary>
        /// <param name="key">The keyboard key</param>
        public bool IsKeyUp (XInput.Keys key)
        {
            if (!states.ContainsKey(key)) return false;
            return states[key] == KeyState.Released;
        }

        /// <summary>
        ///     Gets if the given key is down (once).
        ///     This state is reseted on the next frame.
        /// </summary>
        /// <param name="key">The keyboard key</param>
        public bool IsKeyDown (XInput.Keys key)
        {
            if (!states.ContainsKey(key)) return false;
            return states[key] == KeyState.Down;
        }

        internal void Update ()
        {
            var state = XInput.Keyboard.GetState();
            CapsLock = state.CapsLock;

            foreach (var key in state.GetPressedKeys())
                if (!states.ContainsKey(key)) states[key] = KeyState.None;

            foreach (var key in states.Keys.ToArray())
                states[key] = states[key].Next(state.IsKeyDown(key));
        }
    }

    /// <summary>
    ///     The gamepad state.
    /// </summary>
    public sealed class GamePadState
    {
        /// Vibration seconds amount.
        private float vibration;

        /// The gamepad index.
        private readonly int index;

        /// The gamepad states.
        private readonly Dictionary<XInput.Buttons, KeyState> states = new Dictionary<XInput.Buttons, KeyState>();

        /// The gamepad state.
        private XInput.GamePadState State => XInput.GamePad.GetState(index);

        /// <summary>
        ///     Gets the gamepad packet number.
        /// </summary>
        public int PacketNumber => State.PacketNumber;

        /// <summary>
        ///     Gets whether the gamepad is connected or not.
        /// </summary>
        public bool IsConnected => State.IsConnected;

        /// <summary>
        ///     Gets the left trigger value.
        /// </summary>
        public float LeftTrigger => State.Triggers.Left;

        /// <summary>
        ///     Gets the right trigger value.
        /// </summary>
        public float RightTrigger => State.Triggers.Right;

        /// <summary>
        ///     Gets the left thumbstick coords.
        /// </summary>
        public Vector2 LeftThumbStick => State.ThumbSticks.Left;

        /// <summary>
        ///     Gets the right thumbstick coords.
        /// </summary>
        public Vector2 RightThumbStick => State.ThumbSticks.Right;

        /// <summary>
        ///     Creates a new gamepad state.
        /// </summary>
        /// <param name="index">The gamepad index</param>
        public GamePadState (int index)
        {
            this.index = index;
            foreach (XInput.Buttons button in Enum.GetValues(typeof(XInput.Buttons)))
                states.Add(button, KeyState.None);
        }

        /// <summary>
        ///     Gets if the given button is down.
        /// </summary>
        /// <param name="key">The gamepad button</param>
        public bool IsButton (XInput.Buttons button)
        {
            if (!states.ContainsKey(button)) return false;
            var state = states[button];
            return state == KeyState.Down || state == KeyState.Hold;
        }

        /// <summary>
        ///     Gets if the given button is up (once).
        ///     This state is reseted on the next frame.
        /// </summary>
        /// <param name="button">The gamepad button</param>
        public bool IsButtonUp (XInput.Buttons button)
        {
            if (!states.ContainsKey(button)) return false;
            return states[button] == KeyState.Released;
        }

        /// <summary>
        ///     Gets if the given button is down (once).
        ///     This state is reseted on the next frame.
        /// </summary>
        /// <param name="button">The gamepad button</param>
        public bool IsButtonDown (XInput.Buttons button)
        {
            if (!states.ContainsKey(button)) return false;
            return states[button] == KeyState.Down;
        }

        /// <summary>
        ///     Sets the gamepad vibration
        /// </summary>
        /// <param name="amount">The vibration amount from 0 to 1</param>
        /// <param name="seconds">The vibration duration, if negative the vibration is continuously</param>
        public void SetVibration (float amount, float seconds = -1)
        {
            vibration = seconds;
            XInput.GamePad.SetVibration(index, amount, amount);
        }

        /// <summary>
        ///     Sets the gamepad left and right vibration
        /// </summary>
        /// <param name="leftAmount">The left vibration amount from 0 to 1</param>
        /// <param name="rightAmount">The right vibration amount from 0 to 1</param>
        /// <param name="seconds">The vibration duration, if negative the vibration is continuously</param>
        public void SetVibration (float leftAmount, float rightAmount, float seconds = -1)
        {
            vibration = seconds;
            XInput.GamePad.SetVibration(index, leftAmount, rightAmount);
        }

        internal void Update ()
        {
            if (vibration > 0)
            {
                vibration -= Time.ElapsedTime;
                if (vibration <= 0) SetVibration(0);
            }

            foreach (XInput.Buttons button in states.Keys.ToArray())
                states[button] = states[button].Next(State.IsButtonDown(button));
        }
    }
}
*/