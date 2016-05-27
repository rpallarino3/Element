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

        public InputHandler(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
            _controls = new Dictionary<ControlFunctions, Control>();

            var keybinds = resourceManager.PreferenceData.Keybindings;
            var buttonBinds = resourceManager.PreferenceData.ButtonBindings;

            _controls.Add(ControlFunctions.MoveUp, new Control(new List<Keys>(keybinds[ControlFunctions.MoveUp]), new List<Buttons>(buttonBinds[ControlFunctions.MoveUp]), false, true));
            _controls.Add(ControlFunctions.MoveDown, new Control(new List<Keys>(keybinds[ControlFunctions.MoveDown]), new List<Buttons>(buttonBinds[ControlFunctions.MoveDown]), false, true));
            _controls.Add(ControlFunctions.MoveLeft, new Control(new List<Keys>(keybinds[ControlFunctions.MoveLeft]), new List<Buttons>(buttonBinds[ControlFunctions.MoveLeft]), false, true));
            _controls.Add(ControlFunctions.MoveRight, new Control(new List<Keys>(keybinds[ControlFunctions.MoveRight]), new List<Buttons>(buttonBinds[ControlFunctions.MoveRight]), false, true));
            _controls.Add(ControlFunctions.Confirm, new Control(new List<Keys>(keybinds[ControlFunctions.Confirm]), new List<Buttons>(buttonBinds[ControlFunctions.Confirm]), true, true));
            _controls.Add(ControlFunctions.Back, new Control(new List<Keys>(keybinds[ControlFunctions.Back]), new List<Buttons>(buttonBinds[ControlFunctions.Back]), true, true));
            _controls.Add(ControlFunctions.Cast, new Control(new List<Keys>(keybinds[ControlFunctions.Cast]), new List<Buttons>(buttonBinds[ControlFunctions.Cast]), true, true));
            _controls.Add(ControlFunctions.Cycle, new Control(new List<Keys>(keybinds[ControlFunctions.Cycle]), new List<Buttons>(buttonBinds[ControlFunctions.Cycle]), true, true));
            _controls.Add(ControlFunctions.Menu, new Control(new List<Keys>(keybinds[ControlFunctions.Menu]), new List<Buttons>(buttonBinds[ControlFunctions.Menu]), true, true));
        }

        public void UpdateInputs(GamePadState gamePadState, KeyboardState keyboardState)
        {
            _padState = gamePadState;
            _keyboardState = keyboardState;

            foreach (ControlFunctions cf in _controls.Keys)
            {
                _controls[cf].UpdateReady(gamePadState, keyboardState);
            }
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

        public bool InputEmpty { get; private set; }
    }
}
