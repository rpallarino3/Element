using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Animations;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.Menu;
using Element.Common.HelperClasses;
using Microsoft.Xna.Framework;

namespace Element.Common.Menus
{
    public class MenuButton
    {
        private Animator _animator;

        private MenuButton _upButton;
        private MenuButton _downButton;
        private MenuButton _leftButton;
        private MenuButton _rightButton;

        private Vector2 _location;
        private string _text;
        private ButtonStyles _style;
        private MenuPageEventArgs _args;
        private bool _selected;

        private ButtonType _type;
        private ButtonStates _state;

        #region Constructors

        public MenuButton(Vector2 location, string text, ButtonStyles style, MenuPageEventArgs args) : this(location, text, style, args, ButtonType.Standard) { }

        public MenuButton(Vector2 location, string text, ButtonStyles style, MenuPageEventArgs args, ButtonType type)
        {
            _location = location;
            _text = text;
            _style = style;
            _args = args;
            _type = type;

            _state = ButtonStates.Enabled;
            _animator = new Animator(ButtonAnimator.GetAnimationsFromStyle(_style), (int)ButtonStates.Enabled);
        }

        #endregion

        public void UpdateOwnLogic()
        {
            if (_animator.AnimationFinished)
            {
                if (_state == ButtonStates.Disabled || _state == ButtonStates.Enabled || _state == ButtonStates.Selected ||
                    _state == ButtonStates.HighlightDisabled || _state == ButtonStates.HighlightEnabled || _state == ButtonStates.HighlightSelected)
                {
                    _animator.AdvanceAnimationReplay();
                    return;
                }

                if (_state == ButtonStates.Select)
                {
                    if (_type == ButtonType.Standard)
                    {
                        _state = ButtonStates.Enabled;
                        _animator.SetNewAnimation(ButtonAnimator.ConvertEnumToIndex(ButtonStates.Enabled, _style));
                    }
                    else
                    {
                        _state = ButtonStates.Selected;
                        _animator.SetNewAnimation(ButtonAnimator.ConvertEnumToIndex(ButtonStates.Selected, _style));
                    }
                }

                if (_state == ButtonStates.Deselect || _state == ButtonStates.FadeIn)
                {
                    _state = ButtonStates.Enabled;
                    _animator.SetNewAnimation(ButtonAnimator.ConvertEnumToIndex(ButtonStates.Enabled, _style));
                }

                if (_state == ButtonStates.FadeOut)
                    _state = ButtonStates.Hidden;
            }
            else
            {
                _animator.AdvanceAnimation();
            }
        }

        public void Enable()
        {
            _state = ButtonStates.Enabled;
            _animator.SetNewAnimation(ButtonAnimator.ConvertEnumToIndex(ButtonStates.Enabled, _style));
        }

        // i guess disabling the button automatically deselects it?
        public void Disable()
        {
            _state = ButtonStates.Disabled;
            _animator.SetNewAnimation(ButtonAnimator.ConvertEnumToIndex(ButtonStates.Disabled, _style));
        }

        public void SelectButton()
        {
            if (_state != ButtonStates.Selected && _state != ButtonStates.Enabled)
                return;

            if (_state == ButtonStates.Selected)
            {
                _animator.SetNewAnimation(ButtonAnimator.ConvertEnumToIndex(ButtonStates.Deselect, _style));
                _state = ButtonStates.Deselect;
            }
            else
            {
                _animator.SetNewAnimation(ButtonAnimator.ConvertEnumToIndex(ButtonStates.Select, _style));
                _state = ButtonStates.Select;
            }
            
            OnSelected(_args);
        }

        public void SelectNoEvent()
        {
            if (_state != ButtonStates.Selected && _state != ButtonStates.Enabled)
                return;

            if (_state == ButtonStates.Selected)
            {
                _animator.SetNewAnimation(ButtonAnimator.ConvertEnumToIndex(ButtonStates.Deselect, _style));
                _state = ButtonStates.Deselect;
            }
            else
            {
                _animator.SetNewAnimation(ButtonAnimator.ConvertEnumToIndex(ButtonStates.Select, _style));
                _state = ButtonStates.Select;
            }
        }

        public void Highlight()
        {
            if (_state == ButtonStates.Enabled)
            {
                _state = ButtonStates.HighlightEnabled;
                _animator.SetNewAnimation(ButtonAnimator.ConvertEnumToIndex(ButtonStates.HighlightEnabled, _style));
            }
            else if (_state == ButtonStates.Disabled)
            {
                _state = ButtonStates.HighlightDisabled;
                _animator.SetNewAnimation(ButtonAnimator.ConvertEnumToIndex(ButtonStates.HighlightDisabled, _style));
            }
            else if (_state == ButtonStates.Selected)
            {
                _state = ButtonStates.HighlightSelected;
                _animator.SetNewAnimation(ButtonAnimator.ConvertEnumToIndex(ButtonStates.HighlightSelected, _style));
            }
        }

        public void DeHighlight()
        {
            if (_state == ButtonStates.Enabled)
            {
                _state = ButtonStates.Enabled;
                _animator.SetNewAnimation(ButtonAnimator.ConvertEnumToIndex(ButtonStates.HighlightEnabled, _style));
            }
            else if (_state == ButtonStates.Disabled)
            {
                _state = ButtonStates.Disabled;
                _animator.SetNewAnimation(ButtonAnimator.ConvertEnumToIndex(ButtonStates.HighlightDisabled, _style));
            }
            else if (_state == ButtonStates.Selected)
            {
                _state = ButtonStates.Selected;
                _animator.SetNewAnimation(ButtonAnimator.ConvertEnumToIndex(ButtonStates.HighlightSelected, _style));
            }
        }

        public void FadeIn()
        {
            _state = ButtonStates.FadeIn;
            _animator.SetNewAnimation(ButtonAnimator.ConvertEnumToIndex(ButtonStates.FadeIn, _style));
        }

        public void FadeOut()
        {
            _state = ButtonStates.FadeOut;
            _animator.SetNewAnimation(ButtonAnimator.ConvertEnumToIndex(ButtonStates.FadeOut, _style));
        }                

        public MenuButton GetNextButtonFromDirection(Directions direction)
        {
            if (direction == Directions.Up)
                return _upButton;
            else if (direction == Directions.Down)
                return _downButton;
            else if (direction == Directions.Left)
                return _leftButton;
            else if (direction == Directions.Right)
                return _rightButton;
            else
                return null;
        }

        public void ResetEventHandlers()
        {
            OnSelected = null;
        }

        #region Properties

        public Animator Animator
        {
            get { return _animator; }
        }

        public MenuButton UpButton
        {
            get { return _upButton; }
            set { _upButton = value; }
        }

        public MenuButton DownButton
        {
            get { return _downButton; }
            set { _downButton = value; }
        }

        public MenuButton LeftButton
        {
            get { return _leftButton; }
            set { _leftButton = value; }
        }

        public MenuButton RightButton
        {
            get { return _rightButton; }
            set { _rightButton = value; }
        }

        public Vector2 Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public ButtonStyles Style
        {
            get { return _style; }
        }

        public MenuPageEventArgs Args
        {
            get { return _args; }
            set { _args = value; }
        }

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        public ButtonStates State
        {
            get { return _state; }
        }

        #endregion

        public event MenuPageEvent OnSelected;
    }
}
