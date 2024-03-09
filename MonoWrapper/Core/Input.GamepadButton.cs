using Microsoft.Xna.Framework.Input;

namespace MonoWrapper
{
    public enum GamepadButton
    {
        /// <summary>Directional pad up.</summary>
        DPadUp = Buttons.DPadUp,

        /// <summary>Directional pad down.</summary>
        DPadDown = Buttons.DPadDown,

        /// <summary>Directional pad left.</summary>
        DPadLeft = Buttons.DPadLeft,

        /// <summary>Directional pad right.</summary>
        DPadRight = Buttons.DPadRight,

        /// <summary>START button.</summary>
        Start = Buttons.Start,

        /// <summary>BACK button.</summary>
        Back = Buttons.Back,

        /// <summary>Left stick button (pressing the left stick).</summary>
        LeftStick = Buttons.LeftStick,

        /// <summary>Right stick button (pressing the right stick).</summary>
        RightStick = Buttons.RightStick,

        /// <summary>Left bumper (shoulder) button.</summary>
        LeftShoulder = Buttons.LeftShoulder,

        /// <summary>Right bumper (shoulder) button.</summary>
        RightShoulder = Buttons.RightShoulder,

        /// <summary>Big button.</summary>
        BigButton = Buttons.BigButton,

        /// <summary>A button.</summary>
        A = Buttons.A,

        /// <summary>B button.</summary>
        B = Buttons.B,

        /// <summary>X button.</summary>
        X = Buttons.X,

        /// <summary>Y button.</summary>
        Y = Buttons.Y,

        /// <summary>Left stick is towards the left.</summary>
        LeftThumbstickLeft = Buttons.LeftThumbstickLeft,

        /// <summary>Right trigger.</summary>
        RightTrigger = Buttons.RightTrigger,

        /// <summary>Left trigger.</summary>
        LeftTrigger = Buttons.LeftTrigger,

        /// <summary>Right stick is towards up.</summary>
        RightThumbstickUp = Buttons.RightThumbstickUp,

        /// <summary>Right stick is towards down.</summary>
        RightThumbstickDown = Buttons.RightThumbstickDown,

        /// <summary>Right stick is towards the right.</summary>
        RightThumbstickRight = Buttons.RightThumbstickRight,

        /// <summary>Right stick is towards the left.</summary>
        RightThumbstickLeft = Buttons.RightThumbstickLeft,

        /// <summary>Left stick is towards up.</summary>
        LeftThumbstickUp = Buttons.LeftThumbstickUp,

        /// <summary>Left stick is towards down.</summary>
        LeftThumbstickDown = Buttons.LeftThumbstickDown,

        /// <summary>Left stick is towards the right.</summary>
        LeftThumbstickRight = Buttons.LeftThumbstickRight,
    }
}
