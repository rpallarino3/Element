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

        public static void ValidatePreferenceData()
        {
            var data = DataHelper.PreferenceData;

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
            {
                var nKeybinds = new List<KeyValuePair<ControlFunctions, List<Keys>>>();

                foreach (var key in _defaultKeybinds.Keys)
                {
                    nKeybinds.Add(new KeyValuePair<ControlFunctions, List<Keys>>(key, new List<Keys>(_defaultKeybinds[key])));
                }

                data.Keybindings = nKeybinds;
            }

            if (data.ButtonBindings == null)
            {
                var nButtonBindings = new List<KeyValuePair<ControlFunctions, List<Buttons>>>();

                foreach (var key in _defaultButtonBinds.Keys)
                {
                    nButtonBindings.Add(new KeyValuePair<ControlFunctions, List<Buttons>>(key, new List<Buttons>(_defaultButtonBinds[key])));
                }

                data.ButtonBindings = nButtonBindings;
            }
            
            foreach (ControlFunctions function in Enum.GetValues(typeof(ControlFunctions)))
            {
                var kvp = data.Keybindings.FirstOrDefault(k => k.Key == function);

                if (kvp.Equals(new KeyValuePair<ControlFunctions, List<Keys>>()) || kvp.Value == null || kvp.Value.Count == 0)
                {
                    ResetKeys(); ;
                    goto Buttons;
                }
                
                foreach (ControlFunctions otherFunction in Enum.GetValues(typeof(ControlFunctions)))
                {
                    if (function == otherFunction)
                        continue;

                    var okvp = data.Keybindings.FirstOrDefault(k => k.Key == otherFunction);

                    if (okvp.Equals(new KeyValuePair<ControlFunctions, List<Keys>>()) || okvp.Value == null || okvp.Value.Count == 0)
                    {
                        ResetKeys();
                        goto Buttons;
                    }

                    foreach (var key in kvp.Value)
                    {
                        foreach (var otherKey in okvp.Value)
                        {
                            if (key == otherKey)
                            {
                                ResetKeys();
                                goto Buttons;
                            }
                        }
                    }
                }
            }

            Buttons:

            foreach (ControlFunctions function in Enum.GetValues(typeof(ControlFunctions)))
            {
                var kvp = data.ButtonBindings.FirstOrDefault(k => k.Key == function);

                if (kvp.Equals(new KeyValuePair<ControlFunctions, List<Buttons>>()) || kvp.Value == null || kvp.Value.Count == 0)
                {
                    ResetButtons();
                    return;
                }

                foreach (ControlFunctions otherFunction in Enum.GetValues(typeof(ControlFunctions)))
                {
                    if (function == otherFunction)
                        continue;

                    var okvp = data.ButtonBindings.FirstOrDefault(k => k.Key == otherFunction);

                    if (okvp.Equals(new KeyValuePair<ControlFunctions, List<Buttons>>()) || okvp.Value == null || okvp.Value.Count == 0)
                    {
                        ResetButtons();
                        return;
                    }

                    foreach (var key in kvp.Value)
                    {
                        foreach (var otherKey in okvp.Value)
                        {
                            if (key == otherKey)
                            {
                                ResetButtons();
                                return;
                            }
                        }
                    }
                }
            }
        }

        public static void AddDefaultKeybindsToDictionaries()
        {
            ResetKeys();
            ResetButtons();
        }

        private static void ResetKeys()
        {
            var nKeybinds = new List<KeyValuePair<ControlFunctions, List<Keys>>>();

            foreach (var key in _defaultKeybinds.Keys)
            {
                nKeybinds.Add(new KeyValuePair<ControlFunctions, List<Keys>>(key, new List<Keys>(_defaultKeybinds[key])));
            }

            DataHelper.PreferenceData.Keybindings = nKeybinds;
        }

        private static void ResetButtons()
        {
            var nButtonBindings = new List<KeyValuePair<ControlFunctions, List<Buttons>>>();

            foreach (var key in _defaultButtonBinds.Keys)
            {
                nButtonBindings.Add(new KeyValuePair<ControlFunctions, List<Buttons>>(key, new List<Buttons>(_defaultButtonBinds[key])));
            }

            DataHelper.PreferenceData.ButtonBindings = nButtonBindings;
        }

        public static int DefaultVolume = 5;
        public static Resolutions DefaultResolution = Resolutions.r1280x720;
    }
}
