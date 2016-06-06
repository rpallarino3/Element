using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Element.Common.Enumerations.GameBasics;

namespace Element.Common.HelperClasses
{
    public static class InputHelper
    {
        private static bool _gamePadConnected;

        public static string GetStringForFunction(ControlFunctions function)
        {
            return string.Empty;
        }

        public static string GetStringFromKey(Keys key)
        {
            switch (key)
            {
                case Keys.A:
                    return "A";
                case Keys.B:
                    return "B";
                case Keys.C:
                    return "C";
                case Keys.D:
                    return "D";
                case Keys.E:
                    return "E";
                case Keys.F:
                    return "F";
                case Keys.G:
                    return "G";
                case Keys.H:
                    return "H";
                case Keys.I:
                    return "I";
                case Keys.J:
                    return "J";
                case Keys.K:
                    return "K";
                case Keys.L:
                    return "L";
                case Keys.M:
                    return "M";
                case Keys.N:
                    return "N";
                case Keys.O:
                    return "O";
                case Keys.P:
                    return "P";
                case Keys.Q:
                    return "Q";
                case Keys.R:
                    return "R";
                case Keys.S:
                    return "S";
                case Keys.T:
                    return "T";
                case Keys.U:
                    return "U";
                case Keys.V:
                    return "V";
                case Keys.W:
                    return "W";
                case Keys.X:
                    return "X";
                case Keys.Y:
                    return "Y";
                case Keys.Z:
                    return "Z";
                case Keys.D0:
                    return "0";
                case Keys.D1:
                    return "1";
                case Keys.D2:
                    return "2";
                case Keys.D3:
                    return "3";
                case Keys.D4:
                    return "4";
                case Keys.D5:
                    return "5";
                case Keys.D6:
                    return "6";
                case Keys.D7:
                    return "7";
                case Keys.D8:
                    return "8";
                case Keys.D9:
                    return "9";
                case Keys.Tab:
                    return "Tab";
                case Keys.CapsLock:
                    return "Caps Lock";
                case Keys.LeftShift:
                    return "L Shift";
                case Keys.LeftControl:
                    return "L Ctrl";
                case Keys.LeftAlt:
                    return "L Alt";
                case Keys.LeftWindows:
                    return "L Windows";
                case Keys.OemTilde:
                    return "~";
                case Keys.F1:
                    return "F1";
                case Keys.F2:
                    return "F2";
                case Keys.F3:
                    return "F3";
                case Keys.F4:
                    return "F4";
                case Keys.F5:
                    return "F5";
                case Keys.F6:
                    return "F6";
                case Keys.F7:
                    return "F7";
                case Keys.F8:
                    return "F8";
                case Keys.F9:
                    return "F9";
                case Keys.F10:
                    return "F10";
                case Keys.F11:
                    return "F11";
                case Keys.F12:
                    return "F12";
                case Keys.F13:
                    return "F13";
                case Keys.F14:
                    return "F14";
                case Keys.F15:
                    return "F15";
                case Keys.F16:
                    return "F16";
                case Keys.F17:
                    return "F17";
                case Keys.F18:
                    return "F18";
                case Keys.F19:
                    return "F19";
                case Keys.OemMinus:
                    return "-";
                case Keys.OemPlus:
                    return "+";
                case Keys.Back:
                    return "Backspace";
                case Keys.Scroll:
                    return "Scroll";
                case Keys.PrintScreen:
                    return "Print Screen";
                case Keys.OemOpenBrackets:
                    return "[";
                case Keys.OemCloseBrackets:
                    return "]";
                case Keys.OemBackslash:
                    return "\\";
                case Keys.OemSemicolon:
                    return ";";
                case Keys.OemQuotes:
                    return "'";
                case Keys.OemComma:
                    return ",";
                case Keys.OemPeriod:
                    return ".";
                case Keys.OemQuestion:
                    return "/";
                case Keys.Enter:
                    return "Enter";
                case Keys.RightShift:
                    return "R Shift";
                case Keys.RightAlt:
                    return "R Alt";
                case Keys.RightControl:
                    return "R Ctrl";
                case Keys.RightWindows:
                    return "Function";
                case Keys.Left:
                    return "Left";
                case Keys.Up:
                    return "Up";
                case Keys.Down:
                    return "Down";
                case Keys.Right:
                    return "Right";
                case Keys.Insert:
                    return "Insert";
                case Keys.Pause:
                    return "Pause";
                case Keys.Home:
                    return "Home";
                case Keys.End:
                    return "End";
                case Keys.Delete:
                    return "Delete";
                case Keys.PageUp:
                    return "PageUp";
                case Keys.PageDown:
                    return "PageDown";
                case Keys.NumLock:
                    return "Num Lock";
                case Keys.NumPad0:
                    return "Num 0";
                case Keys.NumPad1:
                    return "Num 1";
                case Keys.NumPad2:
                    return "Num 2";
                case Keys.NumPad3:
                    return "Num 3";
                case Keys.NumPad4:
                    return "Num 4";
                case Keys.NumPad5:
                    return "Num 5";
                case Keys.NumPad6:
                    return "Num 6";
                case Keys.NumPad7:
                    return "Num 7";
                case Keys.NumPad8:
                    return "Num 8";
                case Keys.NumPad9:
                    return "Num 9";
                case Keys.Multiply:
                    return "*";
                case Keys.Divide:
                    return "/";
                case Keys.Escape:
                    return "Esc";
                default:
                    return string.Empty;
            }
        }

        public static string GetStringFromButton(Buttons button)
        {
            switch (button)
            {
                case Buttons.A:
                    return "A";
                case Buttons.B:
                    return "B";
                case Buttons.X:
                    return "X";
                case Buttons.Y:
                    return "Y";
                case Buttons.LeftShoulder:
                    return "L Shoulder";
                case Buttons.RightShoulder:
                    return "R Shoulder";
                case Buttons.LeftTrigger:
                    return "L Trigger";
                case Buttons.RightTrigger:
                    return "R Trigger";
                case Buttons.Back:
                    return "Back";
                case Buttons.Start:
                    return "Start";
                case Buttons.LeftThumbstickUp:
                case Buttons.RightThumbstickUp:
                case Buttons.DPadUp:
                    return "Up";
                case Buttons.LeftThumbstickDown:
                case Buttons.RightThumbstickDown:
                case Buttons.DPadDown:
                    return "Down";
                case Buttons.LeftThumbstickLeft:
                case Buttons.RightThumbstickLeft:
                case Buttons.DPadLeft:
                    return "Left";
                case Buttons.LeftThumbstickRight:
                case Buttons.RightThumbstickRight:
                case Buttons.DPadRight:
                    return "Right";
                default:
                    return string.Empty;
            }
        }

        public static string GetStringBasedOnConnectedController(Keys key, Buttons button)
        {
            if (_gamePadConnected)
                return GetStringFromButton(button);
            else
                return GetStringFromKey(key);
        }

        public static void UpdateGamePadConnected(bool connected)
        {
            // somehow need to switch between them?
            _gamePadConnected = connected;
        }
    }
}
