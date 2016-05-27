using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Element.Common.Misc
{
    public class Control
    {

        private bool _functionReady;
        private bool _released;
        private List<Keys> _keys; // might want to redo controls to only take OR keys
        private List<Buttons> _buttons;
        private bool _releaseBeforeRepress;
        private bool _allowOr;

        public Control(List<Keys> keys, List<Buttons> buttons, bool releaseBeforeRepress, bool allowOr)
        {
            _functionReady = false;
            _released = false;
            _keys = keys;
            _buttons = buttons;
            _releaseBeforeRepress = releaseBeforeRepress;
            _allowOr = allowOr;
        }

        public void UpdateKeyBinding(Keys key)
        {
            _keys.Clear();
            _keys.Add(key);
        }

        public void UpdateButtonBinding(Buttons button)
        {
            _buttons.Clear();
            _buttons.Add(button);
        }

        public void UpdateKeyBinding(List<Keys> newKeys)
        {
            _keys = newKeys;
        }

        public void UpdateButtonBinding(List<Buttons> newButtons)
        {
            _buttons = newButtons;
        }

        public void UpdateReady(GamePadState gamePadState, KeyboardState keyboardState)
        {
            bool allButtonsDown = true;
            bool allKeysDown = true;

            bool atLeastOneButtonDown = false;
            bool atLeastOneKeyDown = false;

            foreach (Buttons b in _buttons)
            {
                if (!gamePadState.IsButtonDown(b))
                {
                    allButtonsDown = false;
                    break;
                }

                atLeastOneButtonDown = true;
            }

            foreach (Keys k in _keys)
            {
                if (!keyboardState.IsKeyDown(k))
                {
                    allKeysDown = false;
                    break;
                }

                atLeastOneKeyDown = true;
            }

            if (_allowOr)
            {
                if (!atLeastOneButtonDown && !atLeastOneButtonDown)
                {
                    _functionReady = false;
                    _released = true;
                }
                else
                {
                    if (_releaseBeforeRepress)
                    {
                        if (_released)
                        {
                            _released = false;
                            _functionReady = true;
                        }
                        else
                        {
                            _functionReady = false;
                        }
                    }
                    else
                    {
                        _functionReady = true;
                    }
                    _released = false;
                }
            }
            else if (!allKeysDown && !allButtonsDown)
            {
                _functionReady = false;
                _released = true;
            }
            else
            {
                if (_releaseBeforeRepress)
                {
                    if (_released)
                    {
                        _released = false;
                        _functionReady = true;
                    }
                    else
                    {
                        _functionReady = false;
                    }
                }
                else
                {
                    _functionReady = true;
                }
                _released = false;
            }
        }

        public void ClearFunction()
        {
            _functionReady = false;
        }

        public bool FunctionReady
        {
            get { return _functionReady; }
        }

        public List<Keys> Keys
        {
            get { return _keys; }
            set { _keys = value; }
        }

        public List<Buttons> Buttons
        {
            get { return _buttons; }
            set { _buttons = value; }
        }
    }
}
