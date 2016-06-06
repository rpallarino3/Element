using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Element.Common.Enumerations.GameBasics;
using Element.Common.HelperClasses;
using Element.Common.Misc;
using Element.Common.Data;
using Element.ResourceManagement;

namespace Element.Input
{
    public class InputHandler
    {
        private ResourceManager _resourceManager;
        private Dictionary<ControlFunctions, Control> _controls;

        private GamePadState _padState;
        private KeyboardState _keyboardState;

        private int _upCount;
        private int _downCount;
        private int _leftCount;
        private int _rightCount;

        public InputHandler(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
            _controls = new Dictionary<ControlFunctions, Control>();

            _upCount = 0;
            _downCount = 0;
            _leftCount = 0;
            _rightCount = 0;

            var keybinds = DataHelper.PreferenceData.Keybindings;
            var buttonBinds = DataHelper.PreferenceData.ButtonBindings;

            _controls.Add(ControlFunctions.MoveUp, new Control(new List<Keys>(keybinds.First(entry => entry.Key == ControlFunctions.MoveUp).Value),
                new List<Buttons>(buttonBinds.First(entry => entry.Key == ControlFunctions.MoveUp).Value), false, true));
            _controls.Add(ControlFunctions.MoveDown, new Control(new List<Keys>(keybinds.First(entry => entry.Key == ControlFunctions.MoveDown).Value),
                new List<Buttons>(buttonBinds.First(entry => entry.Key == ControlFunctions.MoveDown).Value), false, true));
            _controls.Add(ControlFunctions.MoveLeft, new Control(new List<Keys>(keybinds.First(entry => entry.Key == ControlFunctions.MoveLeft).Value),
                new List<Buttons>(buttonBinds.First(entry => entry.Key == ControlFunctions.MoveLeft).Value), false, true));
            _controls.Add(ControlFunctions.MoveRight, new Control(new List<Keys>(keybinds.First(entry => entry.Key == ControlFunctions.MoveRight).Value),
                new List<Buttons>(buttonBinds.First(entry => entry.Key == ControlFunctions.MoveRight).Value), false, true));
            _controls.Add(ControlFunctions.Confirm, new Control(new List<Keys>(keybinds.First(entry => entry.Key == ControlFunctions.Confirm).Value),
                new List<Buttons>(buttonBinds.First(entry => entry.Key == ControlFunctions.Confirm).Value), true, true));
            _controls.Add(ControlFunctions.Back, new Control(new List<Keys>(keybinds.First(entry => entry.Key == ControlFunctions.Back).Value),
                new List<Buttons>(buttonBinds.First(entry => entry.Key == ControlFunctions.Back).Value), true, true));
            _controls.Add(ControlFunctions.Cast, new Control(new List<Keys>(keybinds.First(entry => entry.Key == ControlFunctions.Cast).Value),
                new List<Buttons>(buttonBinds.First(entry => entry.Key == ControlFunctions.Cast).Value), true, true));
            _controls.Add(ControlFunctions.Cycle, new Control(new List<Keys>(keybinds.First(entry => entry.Key == ControlFunctions.Cycle).Value),
                new List<Buttons>(buttonBinds.First(entry => entry.Key == ControlFunctions.Cycle).Value), true, true));
            _controls.Add(ControlFunctions.Menu, new Control(new List<Keys>(keybinds.First(entry => entry.Key == ControlFunctions.Menu).Value),
                new List<Buttons>(buttonBinds.First(entry => entry.Key == ControlFunctions.Menu).Value), true, true));
        }

        public void UpdateInputs(GamePadState gamePadState, KeyboardState keyboardState)
        {
            _padState = gamePadState;
            _keyboardState = keyboardState;

            InputHelper.UpdateGamePadConnected(gamePadState.IsConnected); // might want to switch to keys if no game input but key input?

            foreach (ControlFunctions cf in _controls.Keys)
            {
                _controls[cf].UpdateReady(gamePadState, keyboardState);
            }

            UpdateMovementCounts();
        }

        public void UpdateMovementCounts()
        {
            _upCount = _controls[ControlFunctions.MoveUp].FunctionReady ? _upCount + 1 : 0;
            _downCount = _controls[ControlFunctions.MoveDown].FunctionReady ? _downCount + 1 : 0;
            _leftCount = _controls[ControlFunctions.MoveLeft].FunctionReady ? _leftCount + 1 : 0;
            _rightCount = _controls[ControlFunctions.MoveRight].FunctionReady ? _rightCount + 1 : 0;
        }

        public Directions? GetLongestDirection()
        {
            Directions? longest = null;
            int longestTime = 0;

            if (_upCount > longestTime)
            {
                longest = Directions.Up;
                longestTime = _upCount;
            }

            if (_downCount > longestTime)
            {
                longest = Directions.Down;
                longestTime = _downCount;
            }

            if (_leftCount > longestTime)
            {
                longest = Directions.Left;
                longestTime = _leftCount;
            }

            if (_rightCount > longestTime)
            {
                longest = Directions.Right;
                longestTime = _rightCount;
            }

            return longest;
        }

        public bool IsFunctionReady(ControlFunctions function)
        {
            return _controls[function].FunctionReady;
        }

        public void ClearInputs()
        {
            foreach (ControlFunctions cf in _controls.Keys)
            {
                _controls[cf].ClearFunction();
            }
        }
        
        public bool RequestSingleKeypress()
        {
            var keys = _keyboardState.GetPressedKeys();

            if (keys.Length != 0)
                return true;

            if (_padState.IsButtonDown(Buttons.A) ||
                _padState.IsButtonDown(Buttons.B) ||
                _padState.IsButtonDown(Buttons.X) ||
                _padState.IsButtonDown(Buttons.Y) ||
                _padState.IsButtonDown(Buttons.Back) ||
                _padState.IsButtonDown(Buttons.Start) ||
                _padState.IsButtonDown(Buttons.LeftShoulder) ||
                _padState.IsButtonDown(Buttons.RightShoulder) ||
                _padState.IsButtonDown(Buttons.LeftTrigger) ||
                _padState.IsButtonDown(Buttons.RightThumbstickDown))
            {
                return true;
            }

            return false;
        }

        public bool RequestSingleKeyRebind(ControlFunctions function)
        {
            var keys = _keyboardState.GetPressedKeys();

            bool rebound = false;

            // check here to make sure that the key is not one of the movement keys?

            if (keys.Length != 0)
            {

                RebindInput(keys[0], function);
                rebound = true;
            }

            // how do we do buttons? I guess we have to check everything individually?
            // don't allow to rebind stick/dpad

            if (_padState.IsButtonDown(Buttons.A))
            {
                RebindInput(Buttons.A, function);
                rebound = true;
            }
            else if (_padState.IsButtonDown(Buttons.B))
            {
                RebindInput(Buttons.B, function);
                rebound = true;
            }
            else if (_padState.IsButtonDown(Buttons.X))
            {
                RebindInput(Buttons.X, function);
                rebound = true;
            }
            else if (_padState.IsButtonDown(Buttons.Y))
            {
                RebindInput(Buttons.Y, function);
                rebound = true;
            }
            else if (_padState.IsButtonDown(Buttons.Back))
            {
                RebindInput(Buttons.Back, function);
                rebound = true;
            }
            else if (_padState.IsButtonDown(Buttons.LeftShoulder))
            {
                RebindInput(Buttons.LeftShoulder, function);
                rebound = true;
            }
            else if (_padState.IsButtonDown(Buttons.LeftTrigger))
            {
                RebindInput(Buttons.LeftTrigger, function);
                rebound = true;
            }
            else if (_padState.IsButtonDown(Buttons.Start))
            {
                RebindInput(Buttons.Start, function);
                rebound = true;
            }
            else if (_padState.IsButtonDown(Buttons.RightShoulder))
            {
                RebindInput(Buttons.RightShoulder, function);
                rebound = true;
            }
            else if (_padState.IsButtonDown(Buttons.RightTrigger))
            {
                RebindInput(Buttons.RightTrigger, function);
                rebound = true;
            }

            if (rebound)
            {
                _resourceManager.UpdatePreferenceKeybindData(_controls);
            }

            return rebound;
        }

        #region Rebind

        public void RebindInput(Keys key, ControlFunctions function)
        {
            var oldKey = _controls[function].Keys[0];

            foreach (var func in _controls.Keys)
            {
                if (_controls[func].Keys.Contains(key))
                {
                    _controls[func].UpdateKeyBinding(oldKey);
                    break;
                }
            }

            _controls[function].UpdateKeyBinding(key);
        }

        public void RebindInput(Buttons button, ControlFunctions function)
        {
            var oldButton = _controls[function].Buttons[0];

            foreach (var func in _controls.Keys)
            {
                if (_controls[func].Buttons.Contains(button))
                {
                    _controls[func].UpdateButtonBinding(oldButton);
                    break;
                }
            }

            _controls[function].UpdateButtonBinding(oldButton);
        }

        public void RebindInput(List<Keys> keys, ControlFunctions function)
        {
            InputEmpty = false;
            _controls[function].UpdateKeyBinding(keys);

            foreach (var func in _controls.Keys)
            {
                if (func == function)
                    continue;

                foreach (var key in keys)
                {
                    if (_controls[func].Keys.Contains(key))
                    {
                        _controls[func].Keys.Remove(key);

                        if (_controls.Count == 0)
                            InputEmpty = true;
                    }
                }
            }
        }

        public void RebindInput(List<Buttons> buttons, ControlFunctions function)
        {
            InputEmpty = false;
            _controls[function].UpdateButtonBinding(buttons);

            foreach (var func in _controls.Keys)
            {
                if (func == function)
                    continue;

                foreach (var button in buttons)
                {
                    if (_controls[func].Buttons.Contains(button))
                    {
                        _controls[func].Buttons.Remove(button);

                        if (_controls.Count == 0)
                            InputEmpty = true;
                    }
                }
            }
        }

        #endregion

        public bool InputEmpty { get; private set; }
    }
}
