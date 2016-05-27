using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.Menu;
using Element.Common.Data;

namespace Element.Common.Menus.MenuPages
{
    public class OptionsMenuPage : MenuPage
    {
        private readonly Vector2 RESOLUTION_960x540_LOCATION = new Vector2(0, 0);
        private readonly Vector2 RESOLUTION_1280x720_LOCATION = new Vector2(0, 0);
        private readonly Vector2 RESOLUTION_1600x900_LOCATION = new Vector2(0, 0);
        private readonly Vector2 RESOLUTION_1920x1080_LOCATION = new Vector2(0, 0);
        private readonly Vector2 RESOLUTION_DEFAULT_LOCATION = new Vector2(0, 0);

        private readonly Vector2 VOLUME_UP_LOCATION = new Vector2(0, 0);
        private readonly Vector2 VOLUME_DOWN_LOCATION = new Vector2(0, 0);
        private readonly Vector2 VOLUME_DEFAULT_LOCATION = new Vector2(0, 0);

        private readonly Vector2 MOVE_UP_LOCATION = new Vector2(0, 0);
        private readonly Vector2 MOVE_DOWN_LOCATION = new Vector2(0, 0);
        private readonly Vector2 MOVE_LEFT_LOCATION = new Vector2(0, 0);
        private readonly Vector2 MOVE_RIGHT_LOCATION = new Vector2(0, 0);
        private readonly Vector2 CONFIRM_LOCATION = new Vector2(0, 0);
        private readonly Vector2 BACK_LOCATION = new Vector2(0, 0);
        private readonly Vector2 CAST_LOCATION = new Vector2(0, 0);
        private readonly Vector2 CYCLE_LOCATION = new Vector2(0, 0);
        private readonly Vector2 START_LOCATION = new Vector2(0, 0);
        private readonly Vector2 KEYBIND_DEFAULT_LOCATION = new Vector2(0, 0);

        private readonly Vector2 DEFAULTS_LOCATION = new Vector2(0, 0);
        private readonly Vector2 OK_LOCATION = new Vector2(0, 0);
        private readonly Vector2 APPLY_LOCATION = new Vector2(0, 0);
        private readonly Vector2 MENU_BACK_LOCATION = new Vector2(0, 0);

        private readonly string DEFAULT_TEXT = "Default";
        private readonly string VOLUME_UP_TEXT = "+";
        private readonly string VOLUME_DOWN_TEXT = "-";

        private readonly string DEFAULTS_TEXT = "Defaults";
        private readonly string OK_TEXT = "Ok";
        private readonly string APPLY_TEXT = "Apply";
        private readonly string BACK_TEXT = "Back";

        private MenuButton _resolution960Button;
        private MenuButton _resolution1280Button;
        private MenuButton _resolution1600Button;
        private MenuButton _resolution1920Button;
        private MenuButton _resolutionDefaultButton;

        private MenuButton _volumeUpButton;
        private MenuButton _volumeDownButton;
        private MenuButton _volumeDefaultButton;
        
        private MenuButton _moveUpButton;
        private MenuButton _moveDownButton;
        private MenuButton _moveLeftButton;
        private MenuButton _moveRightButton;
        private MenuButton _confirmButton;
        private MenuButton _backButton;
        private MenuButton _castButton;
        private MenuButton _cycleButton;
        private MenuButton _startButton;
        private MenuButton _keysDefaultButton;

        private MenuButton _defaultsButton;
        private MenuButton _okButton; // might not use this one either
        private MenuButton _applyButton; // might not use this button
        private MenuButton _menuBackButton;

        private MenuDialog _inputKeyDialog;

        private SwitchPageEventArgs _previousMenuArgs;
        private MenuButton _lastKeybindChanged;

        public OptionsMenuPage() : base()
        {
            _name = MenuPageNames.Options;
            _previousMenuArgs = new SwitchPageEventArgs(MenuPageNames.Start, _name);
            // the resolution menu buttons don't have text
            // the keybind buttons need to have their text set via preference data
            // the movement keybind buttons need to be disabled when a controller is connected?

            // might want to change these resolutions to only change on apply?
            _resolution960Button = new MenuButton(RESOLUTION_960x540_LOCATION, string.Empty, ButtonStyles.Resolution, new ResolutionChangeEventArgs(Resolutions.r960x540));
            _resolution1280Button = new MenuButton(RESOLUTION_1280x720_LOCATION, string.Empty, ButtonStyles.Resolution, new ResolutionChangeEventArgs(Resolutions.r1280x720));
            _resolution1600Button = new MenuButton(RESOLUTION_1600x900_LOCATION, string.Empty, ButtonStyles.Resolution, new ResolutionChangeEventArgs(Resolutions.r1600x900));
            _resolution1920Button = new MenuButton(RESOLUTION_1920x1080_LOCATION, string.Empty, ButtonStyles.Resolution, new ResolutionChangeEventArgs(Resolutions.r1920x1080));
            _resolutionDefaultButton = new MenuButton(RESOLUTION_DEFAULT_LOCATION, string.Empty, ButtonStyles.SmallBack, new ResetPreferencesEventArgs(PreferenceTypes.Resolution));

            _volumeUpButton = new MenuButton(VOLUME_UP_LOCATION, VOLUME_UP_TEXT, ButtonStyles.Volume, new VolumeChangeEventArgs(true));
            _volumeDownButton = new MenuButton(VOLUME_DOWN_LOCATION, VOLUME_DOWN_TEXT, ButtonStyles.Volume, new VolumeChangeEventArgs(false));
            _volumeDefaultButton = new MenuButton(VOLUME_DEFAULT_LOCATION, DEFAULT_TEXT, ButtonStyles.SmallBack, new ResetPreferencesEventArgs(PreferenceTypes.Volume));

            _moveUpButton = new MenuButton(MOVE_UP_LOCATION, string.Empty, ButtonStyles.Keybind, new KeybindChangeEventArgs(ControlFunctions.MoveUp, _moveUpButton));
            _moveDownButton = new MenuButton(MOVE_DOWN_LOCATION, string.Empty, ButtonStyles.Keybind, new KeybindChangeEventArgs(ControlFunctions.MoveDown, _moveDownButton));
            _moveLeftButton = new MenuButton(MOVE_LEFT_LOCATION, string.Empty, ButtonStyles.Keybind, new KeybindChangeEventArgs(ControlFunctions.MoveLeft, _moveLeftButton));
            _moveRightButton = new MenuButton(MOVE_RIGHT_LOCATION, string.Empty, ButtonStyles.Keybind, new KeybindChangeEventArgs(ControlFunctions.MoveRight, _moveRightButton));
            _confirmButton = new MenuButton(CONFIRM_LOCATION, string.Empty, ButtonStyles.Keybind, new KeybindChangeEventArgs(ControlFunctions.Confirm, _confirmButton));
            _backButton = new MenuButton(BACK_LOCATION, string.Empty, ButtonStyles.Keybind, new KeybindChangeEventArgs(ControlFunctions.Back, _backButton));
            _castButton = new MenuButton(CAST_LOCATION, string.Empty, ButtonStyles.Keybind, new KeybindChangeEventArgs(ControlFunctions.Cast, _castButton));
            _cycleButton = new MenuButton(CYCLE_LOCATION, string.Empty, ButtonStyles.Keybind, new KeybindChangeEventArgs(ControlFunctions.Cycle, _cycleButton));
            _startButton = new MenuButton(START_LOCATION, string.Empty, ButtonStyles.Keybind, new KeybindChangeEventArgs(ControlFunctions.Menu, _startButton));
            _keysDefaultButton = new MenuButton(KEYBIND_DEFAULT_LOCATION, DEFAULTS_TEXT, ButtonStyles.SmallBack, new ResetPreferencesEventArgs(PreferenceTypes.Keybinds));

            _defaultsButton = new MenuButton(DEFAULTS_LOCATION, DEFAULTS_TEXT, ButtonStyles.SmallBack, new ResetPreferencesEventArgs(PreferenceTypes.All));
            _menuBackButton = new MenuButton(MENU_BACK_LOCATION, BACK_TEXT, ButtonStyles.SmallBack, _previousMenuArgs); // need to change this if enter from exit

            #region Tie Together Buttons

            _resolution960Button.UpButton = _volumeDefaultButton;
            _resolution960Button.DownButton = _resolution1280Button;
            _resolution1280Button.UpButton = _resolution960Button;
            _resolution1280Button.DownButton = _resolution1600Button;
            _resolution1600Button.UpButton = _resolution1280Button;
            _resolution1600Button.DownButton = _resolution1920Button;
            _resolution1920Button.UpButton = _resolution1600Button;
            _resolution1920Button.DownButton = _resolutionDefaultButton;
            _resolutionDefaultButton.UpButton = _resolution1920Button;
            _resolutionDefaultButton.DownButton = _volumeDownButton;

            _volumeDownButton.RightButton = _volumeUpButton;
            _volumeUpButton.LeftButton = _volumeDownButton;

            _moveUpButton.UpButton = _menuBackButton;
            _moveUpButton.DownButton = _moveDownButton;
            _moveDownButton.UpButton = _moveUpButton;
            _moveDownButton.DownButton = _moveLeftButton;
            _moveLeftButton.UpButton = _moveDownButton;
            _moveLeftButton.DownButton = _moveRightButton;
            _moveRightButton.UpButton = _moveLeftButton;
            _moveRightButton.DownButton = _confirmButton;
            _confirmButton.UpButton = _moveRightButton;
            _confirmButton.DownButton = _backButton;
            _backButton.UpButton = _confirmButton;
            _backButton.DownButton = _castButton;
            _castButton.UpButton = _backButton;
            _castButton.DownButton = _cycleButton;
            _cycleButton.UpButton = _castButton;
            _cycleButton.DownButton = _startButton;
            _startButton.UpButton = _cycleButton;
            _startButton.DownButton = _keysDefaultButton;
            _keysDefaultButton.UpButton = _startButton;
            _keysDefaultButton.DownButton = _defaultsButton;
            _defaultsButton.UpButton = _keysDefaultButton;
            _defaultsButton.DownButton = _menuBackButton;
            _menuBackButton.UpButton = _defaultsButton;
            _menuBackButton.DownButton = _moveUpButton;

            _resolution960Button.RightButton = _moveUpButton;
            _resolution960Button.LeftButton = _moveUpButton;
            _resolution1280Button.RightButton = _moveDownButton;
            _resolution1280Button.LeftButton = _moveDownButton;
            _resolution1600Button.RightButton = _moveLeftButton;
            _resolution1600Button.LeftButton = _moveLeftButton;
            _resolution1920Button.RightButton = _moveRightButton;
            _resolution1920Button.LeftButton = _moveRightButton;
            _moveUpButton.LeftButton = _resolution960Button;
            _moveUpButton.RightButton = _resolution960Button;
            _moveDownButton.LeftButton = _resolution1280Button;
            _moveDownButton.RightButton = _resolution1280Button;
            _moveLeftButton.LeftButton = _resolution1600Button;
            _moveLeftButton.RightButton = _resolution1600Button;
            _moveRightButton.LeftButton = _resolution1920Button;
            _moveRightButton.RightButton = _resolution1920Button;

            _resolutionDefaultButton.RightButton = _confirmButton;
            _resolutionDefaultButton.LeftButton = _confirmButton;
            _confirmButton.LeftButton = _resolutionDefaultButton;
            _confirmButton.RightButton = _resolutionDefaultButton;
            _backButton.LeftButton = _resolutionDefaultButton;
            _backButton.RightButton = _resolutionDefaultButton;
            _castButton.LeftButton = _resolutionDefaultButton;
            _castButton.RightButton = _resolutionDefaultButton;

            _volumeDownButton.LeftButton = _startButton;
            _volumeUpButton.RightButton = _startButton;
            _cycleButton.LeftButton = _volumeUpButton;
            _cycleButton.RightButton = _volumeDownButton;
            _startButton.LeftButton = _volumeUpButton;
            _startButton.RightButton = _volumeDownButton;
            _volumeDefaultButton.RightButton = _keysDefaultButton;
            _volumeDefaultButton.LeftButton = _keysDefaultButton;
            _keysDefaultButton.LeftButton = _volumeDefaultButton;
            _keysDefaultButton.RightButton = _volumeDefaultButton;

            _defaultsButton.LeftButton = _volumeDefaultButton;
            _defaultsButton.RightButton = _volumeDefaultButton;
            _menuBackButton.LeftButton = _volumeDefaultButton;
            _menuBackButton.RightButton = _volumeDefaultButton;

            #endregion

            _lastKeybindChanged = _confirmButton;

            #region Events

            _resolution960Button.OnSelected += RaiseResolutionChangeEvent;
            _resolution960Button.OnSelected += SelectResolution;
            _resolution1280Button.OnSelected += RaiseResolutionChangeEvent;
            _resolution1280Button.OnSelected += SelectResolution;
            _resolution1600Button.OnSelected += RaiseResolutionChangeEvent;
            _resolution1600Button.OnSelected += SelectResolution;
            _resolution1920Button.OnSelected += RaiseResolutionChangeEvent;
            _resolution1920Button.OnSelected += SelectResolution;
            _resolutionDefaultButton.OnSelected += RaisePreferenceResetEvent;
            _resolutionDefaultButton.OnSelected += SelectResolution;

            _volumeUpButton.OnSelected += RaiseVolumeChangeEvent;
            _volumeDownButton.OnSelected += RaiseVolumeChangeEvent;
            _volumeDefaultButton.OnSelected += RaisePreferenceResetEvent;

            // maybe change all these to open dialog events?
            _moveUpButton.OnSelected += RaiseKeybindChangeEvent;
            _moveDownButton.OnSelected += RaiseKeybindChangeEvent;
            _moveLeftButton.OnSelected += RaiseKeybindChangeEvent;
            _moveRightButton.OnSelected += RaiseKeybindChangeEvent;
            _confirmButton.OnSelected += RaiseKeybindChangeEvent;
            _backButton.OnSelected += RaiseKeybindChangeEvent;
            _castButton.OnSelected += RaiseKeybindChangeEvent;
            _cycleButton.OnSelected += RaiseKeybindChangeEvent;
            _startButton.OnSelected += RaiseKeybindChangeEvent;
            _keysDefaultButton.OnSelected += RaisePreferenceResetEvent;
            _moveUpButton.OnSelected += OpenKeybindDialog;
            _moveDownButton.OnSelected += OpenKeybindDialog;
            _moveLeftButton.OnSelected += OpenKeybindDialog;
            _moveRightButton.OnSelected += OpenKeybindDialog;
            _confirmButton.OnSelected += OpenKeybindDialog;
            _backButton.OnSelected += OpenKeybindDialog;
            _castButton.OnSelected += OpenKeybindDialog;
            _cycleButton.OnSelected += OpenKeybindDialog;
            _startButton.OnSelected += OpenKeybindDialog;

            _defaultsButton.OnSelected += RaisePreferenceResetEvent;
            _defaultsButton.OnSelected += SelectResolution;

            _menuBackButton.OnSelected += RaiseSwitchPageEvent;

            #endregion


            _buttons.Add(_resolution960Button);
            _buttons.Add(_resolution1280Button);
            _buttons.Add(_resolution1600Button);
            _buttons.Add(_resolution1920Button);
            _buttons.Add(_resolutionDefaultButton);

            _buttons.Add(_volumeUpButton);
            _buttons.Add(_volumeDownButton);
            _buttons.Add(_volumeDefaultButton);

            _buttons.Add(_moveUpButton);
            _buttons.Add(_moveDownButton);
            _buttons.Add(_moveLeftButton);
            _buttons.Add(_moveRightButton);

            _buttons.Add(_confirmButton);
            _buttons.Add(_backButton);
            _buttons.Add(_castButton);
            _buttons.Add(_cycleButton);
            _buttons.Add(_startButton);
            _buttons.Add(_keysDefaultButton);

            _buttons.Add(_defaultsButton);
            _buttons.Add(_menuBackButton);
        }

        public override void UpdateWithPreferenceData(PreferenceData data)
        {
            throw new NotImplementedException();
        }

        public override void EnterMenu(MenuPageNames name, PreferenceData data)
        {
            UnhideAllButtons();

            if (name == MenuPageNames.ExitMenu)
                _previousMenuArgs = new SwitchPageEventArgs(MenuPageNames.ExitMenu, _name);
            else
                _previousMenuArgs = new SwitchPageEventArgs(MenuPageNames.Start, _name);

            _menuBackButton.Args = _previousMenuArgs; // to be honest i'm not sure if we even have to do this
            _currentButton = _menuBackButton;
            _currentButton.Highlight();
        }

        public override void ReturnToPreviousMenu()
        {
            if (!_dialogOpen)
                RaiseSwitchPageEvent(_previousMenuArgs);
        }

        public void CloseKeybindDialog()
        {
            _dialogOpen = false;
            _lastKeybindChanged.Highlight();
        }

        private void OpenKeybindDialog(MenuPageEventArgs e)
        {
            var args = e as KeybindChangeEventArgs;

            if (args == null)
                return;

            _lastKeybindChanged = args.Button;

            var keyDialog = new MenuDialog();

            keyDialog.AddTextLine("Press any key or button.");

            _currentDialog = keyDialog;
            _dialogOpen = true;
        }

        private void SelectResolution(MenuPageEventArgs e)
        {
            if (_resolution960Button != _currentButton && _resolution960Button.Selected)
                _resolution960Button.SelectNoEvent();
            else if (_resolution1280Button != _currentButton && _resolutionDefaultButton != _currentButton && _resolution1280Button.Selected) 
                _resolution1280Button.SelectNoEvent();
            else if (_resolution1600Button != _currentButton && _resolution1280Button.Selected)
                _resolution1600Button.SelectNoEvent();
            else if (_resolution1920Button != _currentButton && _resolution1920Button.Selected)
                _resolution1920Button.SelectNoEvent();
            else if ((_resolutionDefaultButton == _currentButton || _defaultsButton == _currentButton) && !_resolution1280Button.Selected)
                _resolution1280Button.SelectNoEvent();
        }
    }
}
