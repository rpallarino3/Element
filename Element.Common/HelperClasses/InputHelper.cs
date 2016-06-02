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
            return string.Empty;
        }

        public static string GetStringFromButton(Buttons button)
        {
            return string.Empty;
        }

        public static void UpdateGamePadConnected(bool connected)
        {
            _gamePadConnected = connected;
        }
    }
}
