using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Data;

namespace Element.Common.HelperClasses
{
    public static class PreferenceValidator
    {
        private static readonly int KEYBIND_COUNT = 9;
        private static Dictionary<ControlFunctions, List<Keys>> _defaultKeybinds;
        private static Dictionary<ControlFunctions, List<Buttons>> _defaultButtonBinds;

        static PreferenceValidator()
        {
            _defaultKeybinds = new Dictionary<ControlFunctions, List<Keys>>();
            _defaultButtonBinds = new Dictionary<ControlFunctions, List<Buttons>>();

            _defaultKeybinds.Add(ControlFunctions.MoveUp, new List<Keys>() { Keys.W });
            _defaultKeybinds.Add(ControlFunctions.MoveDown, new List<Keys>() { Keys.S });
            _defaultKeybinds.Add(ControlFunctions.MoveLeft, new List<Keys>() { Keys.A });
            _defaultKeybinds.Add(ControlFunctions.MoveRight, new List<Keys>() { Keys.D });
            _defaultKeybinds.Add(ControlFunctions.Confirm, new List<Keys>() { Keys.Q });
            _defaultKeybinds.Add(ControlFunctions.Back, new List<Keys>() { Keys.E });
            _defaultKeybinds.Add(ControlFunctions.Cast, new List<Keys>() { Keys.Z });
            _defaultKeybinds.Add(ControlFunctions.Cycle, new List<Keys>() { Keys.X });
            _defaultKeybinds.Add(ControlFunctions.Menu, new List<Keys>() { Keys.Escape });

            _defaultButtonBinds.Add(ControlFunctions.MoveUp, new List<Buttons>() { Buttons.DPadUp, Buttons.LeftThumbstickUp });
            _defaultButtonBinds.Add(ControlFunctions.MoveDown, new List<Buttons>() { Buttons.DPadDown, Buttons.LeftThumbstickDown });
            _defaultButtonBinds.Add(ControlFunctions.MoveLeft, new List<Buttons>() { Buttons.DPadLeft, Buttons.LeftThumbstickLeft });
            _defaultButtonBinds.Add(ControlFunctions.MoveRight, new List<Buttons>() { Buttons.DPadRight, Buttons.LeftThumbstickRight });
            _defaultButtonBinds.Add(ControlFunctions.Confirm, new List<Buttons>() { Buttons.A });
            _defaultButtonBinds.Add(ControlFunctions.Back, new List<Buttons>() { Buttons.B });
            _defaultButtonBinds.Add(ControlFunctions.Cast, new List<Buttons>() { Buttons.X });
            _defaultButtonBinds.Add(ControlFunctions.Cycle, new List<Buttons>() { Buttons.Y });
            _defaultButtonBinds.Add(ControlFunctions.Menu, new List<Buttons>() { Buttons.Start });
        }

        public static void ValidatePreferenceData(PreferenceData data)
        {
            if (data.Volume < 0)
                data.Volume = 0;
            else if (data.Volume > 10)
                data.Volume = 10;

            if (data.Resolution != Resolutions.r960x540 &&
                data.Resolution != Resolutions.r1280x720 &&
                data.Resolution != Resolutions.r1600x900 &&
                data.Resolution != Resolutions.r1920x1080)
                data.Resolution = Resolutions.r1280x720;

            // validate keybinds here
            
            if (data.Keybindings == null)
                data.Keybindings = _defaultKeybinds.ToDictionary(entry => entry.Key, entry => entry.Value);

            if (data.ButtonBindings == null)
                data.ButtonBindings = _defaultButtonBinds.ToDictionary(entry => entry.Key, entry => entry.Value);
            
            foreach (ControlFunctions function in Enum.GetValues(typeof(ControlFunctions)))
            {
                if (!data.Keybindings.ContainsKey(function) || data.Keybindings[function] == null || data.Keybindings[function].Count == 0)
                {
                    data.Keybindings = _defaultKeybinds.ToDictionary(entry => entry.Key, entry => entry.Value);
                    goto Buttons;
                }
                
                foreach (ControlFunctions otherFunction in Enum.GetValues(typeof(ControlFunctions)))
                {
                    if (function == otherFunction)
                        continue;

                    if (!data.Keybindings.ContainsKey(otherFunction) || data.Keybindings[otherFunction] == null || data.Keybindings[function].Count == 0)
                    {
                        data.Keybindings = _defaultKeybinds.ToDictionary(entry => entry.Key, entry => entry.Value);
                        goto Buttons;
                    }

                    foreach (var key in data.Keybindings[function])
                    {
                        foreach (var otherKey in data.Keybindings[otherFunction])
                        {
                            if (key == otherKey)
                            {
                                data.Keybindings = _defaultKeybinds.ToDictionary(entry => entry.Key, entry => entry.Value);
                                goto Buttons;
                            }
                        }
                    }
                }
            }

            Buttons:

            foreach (ControlFunctions function in Enum.GetValues(typeof(ControlFunctions)))
            {
                if (!data.ButtonBindings.ContainsKey(function) || data.ButtonBindings[function] == null || data.ButtonBindings[function].Count == 0)
                {
                    data.ButtonBindings = _defaultButtonBinds.ToDictionary(entry => entry.Key, entry => entry.Value);
                    return;
                }

                foreach (ControlFunctions otherFunction in Enum.GetValues(typeof(ControlFunctions)))
                {
                    if (function == otherFunction)
                        continue;

                    if (!data.ButtonBindings.ContainsKey(otherFunction) || data.ButtonBindings[otherFunction] == null || data.ButtonBindings[function].Count == 0)
                    {
                        data.ButtonBindings = _defaultButtonBinds.ToDictionary(entry => entry.Key, entry => entry.Value);
                        return;
                    }

                    foreach (var key in data.ButtonBindings[function])
                    {
                        foreach (var otherKey in data.ButtonBindings[otherFunction])
                        {
                            if (key == otherKey)
                            {
                                data.ButtonBindings = _defaultButtonBinds.ToDictionary(entry => entry.Key, entry => entry.Value);
                                return;
                            }
                        }
                    }
                }
            }
        }

        public static void AddDefaultKeybindsToDictionaries(Dictionary<ControlFunctions, List<Keys>> keybindings, Dictionary<ControlFunctions, List<Buttons>> buttonBindings)
        {
            keybindings = _defaultKeybinds.ToDictionary(entry => entry.Key, entry => entry.Value);
            buttonBindings = _defaultButtonBinds.ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        public static int DefaultVolume = 5;
        public static Resolutions DefaultResolution = Resolutions.r1280x720;
    }
}
