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
        private static readonly Dictionary<ControlFunctions, List<Keys>> _defaultKeybinds;
        private static readonly Dictionary<ControlFunctions, List<Buttons>> _defaultButtonBinds;

        static PreferenceValidator()
        {
            _defaultKeybinds = new Dictionary<ControlFunctions, List<Keys>>();
            _defaultButtonBinds = new Dictionary<ControlFunctions, List<Buttons>>();

            _defaultKeybinds.Add(ControlFunctions.MoveUp, new List<Keys>() { Keys.W });
            _defaultKeybinds.Add(ControlFunctions.MoveDown, new List<Keys>() { Keys.S });
            _defaultKeybinds.Add(ControlFunctions.MoveLeft, new List<Keys>() { Keys.A });
            _defaultKeybinds.Add(ControlFunctions.MoveRight, new List<Keys>() { Keys.D });
            _defaultKeybinds.Add(ControlFunctions.Confirm, new List<Keys>() { Keys.Q });
            _defaultKeybinds.Add(ControlFunctions.Grab, new List<Keys>() { Keys.Q });
            _defaultKeybinds.Add(ControlFunctions.Back, new List<Keys>() { Keys.E });
            _defaultKeybinds.Add(ControlFunctions.Run, new List<Keys>() { Keys.E });
            _defaultKeybinds.Add(ControlFunctions.Cast, new List<Keys>() { Keys.Z });
            _defaultKeybinds.Add(ControlFunctions.Cycle, new List<Keys>() { Keys.X });
            _defaultKeybinds.Add(ControlFunctions.Menu, new List<Keys>() { Keys.Escape });

            _defaultButtonBinds.Add(ControlFunctions.MoveUp, new List<Buttons>() { Buttons.DPadUp, Buttons.LeftThumbstickUp });
            _defaultButtonBinds.Add(ControlFunctions.MoveDown, new List<Buttons>() { Buttons.DPadDown, Buttons.LeftThumbstickDown });
            _defaultButtonBinds.Add(ControlFunctions.MoveLeft, new List<Buttons>() { Buttons.DPadLeft, Buttons.LeftThumbstickLeft });
            _defaultButtonBinds.Add(ControlFunctions.MoveRight, new List<Buttons>() { Buttons.DPadRight, Buttons.LeftThumbstickRight });
            _defaultButtonBinds.Add(ControlFunctions.Confirm, new List<Buttons>() { Buttons.A });
            _defaultButtonBinds.Add(ControlFunctions.Grab, new List<Buttons>() { Buttons.A });
            _defaultButtonBinds.Add(ControlFunctions.Back, new List<Buttons>() { Buttons.B });
            _defaultButtonBinds.Add(ControlFunctions.Run, new List<Buttons>() { Buttons.B });
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

            // All this horrible code needs to be redone, the inputs are really bad in general and need to be redone

            CheckFunctions();

            if (data.Keybindings == null || data.Keybindings.Count != data.Functions.Count)
                ResetKeys();

            if (data.ButtonBindings == null || data.ButtonBindings.Count != data.Functions.Count)
                ResetButtons();
            
            foreach (ControlFunctions function in Enum.GetValues(typeof(ControlFunctions)))
            {
                var keys = data.Keybindings[data.Functions.IndexOf(function)];

                if (keys == null || keys.Count == 0 || keys.Count > 1)
                {
                    ResetKeys();
                    goto Buttons;
                }
                
                foreach (ControlFunctions otherFunction in Enum.GetValues(typeof(ControlFunctions)))
                {
                    if (function == otherFunction)
                        continue;

                    if (function == ControlFunctions.Confirm || function == ControlFunctions.Grab)
                    {
                        if (otherFunction == ControlFunctions.Grab || otherFunction == ControlFunctions.Confirm)
                            continue;
                    }

                    if (function == ControlFunctions.Back || function == ControlFunctions.Run)
                    {
                        if (otherFunction == ControlFunctions.Back || otherFunction == ControlFunctions.Run)
                            continue;
                    }

                    var otherKeys = data.Keybindings[data.Functions.IndexOf(otherFunction)];

                    if (otherKeys == null || otherKeys.Count == 0 || otherKeys.Count > 1)
                    {
                        ResetKeys();
                        goto Buttons;
                    }

                    if (keys[0] == otherKeys[0])
                    {
                        ResetKeys();
                        goto Buttons;
                    }
                }
            }

            Buttons:

            foreach (ControlFunctions function in Enum.GetValues(typeof(ControlFunctions)))
            {
                var buttons = data.ButtonBindings[data.Functions.IndexOf(function)];

                if (buttons == null || buttons.Count == 0)
                {
                    ResetButtons();
                    return;
                }

                foreach (ControlFunctions otherFunction in Enum.GetValues(typeof(ControlFunctions)))
                {
                    if (function == otherFunction)
                        continue;

                    if (function == ControlFunctions.Confirm || function == ControlFunctions.Grab)
                    {
                        if (otherFunction == ControlFunctions.Grab || otherFunction == ControlFunctions.Confirm)
                            continue;
                    }

                    if (function == ControlFunctions.Back || function == ControlFunctions.Run)
                    {
                        if (otherFunction == ControlFunctions.Back || otherFunction == ControlFunctions.Run)
                            continue;
                    }

                    var otherButtons = data.ButtonBindings[data.Functions.IndexOf(otherFunction)];

                    if (otherButtons == null || otherButtons.Count == 0)
                    {
                        ResetButtons();
                        return;
                    }

                    foreach (var key in buttons)
                    {
                        foreach (var otherKey in otherButtons)
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

        private static void CheckFunctions()
        {
            if (DataHelper.PreferenceData.Functions == null)
            {
                ResetFunctions();
                return;
            }

            foreach (ControlFunctions cf in Enum.GetValues(typeof(ControlFunctions)))
            {
                if (!DataHelper.PreferenceData.Functions.Contains(cf))
                {
                    ResetFunctions();
                    return;
                }
            }
        }

        private static void ResetFunctions()
        {
            Console.WriteLine("Invalid pref data. Reseting.");
            var functions = new List<ControlFunctions>();
            
            foreach (ControlFunctions cf in Enum.GetValues(typeof(ControlFunctions)))
            {
                functions.Add(cf);
            }

            DataHelper.PreferenceData.Functions = functions;
        }

        private static void ResetKeys()
        {
            Console.WriteLine("Invalid pref data. Reseting.");
            CheckFunctions();
            var nKeybinds = new List<List<Keys>>();

            foreach (var function in DataHelper.PreferenceData.Functions)
            {
                nKeybinds.Add(_defaultKeybinds[function]);
            }

            DataHelper.PreferenceData.Keybindings = nKeybinds;
        }

        private static void ResetButtons()
        {
            Console.WriteLine("Invalid pref data. Reseting.");
            CheckFunctions();
            var nButtonBindings = new List<List<Buttons>>();

            foreach (var function in DataHelper.PreferenceData.Functions)
            {
                nButtonBindings.Add(_defaultButtonBinds[function]);
            }

            DataHelper.PreferenceData.ButtonBindings = nButtonBindings;
        }

        public static int DefaultVolume = 5;
        public static Resolutions DefaultResolution = Resolutions.r1280x720;
    }
}
