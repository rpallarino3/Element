using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.Menu;
using Element.Common.Data;
using Element.Common.HelperClasses;

namespace Element.Common.Menus.MenuPages
{
    public class OptionsMenuPage : MenuPage
    {
        private readonly Vector2 RESOLUTION_960x540_LOCATION = new Vector2(775, 150);
        private readonly Vector2 RESOLUTION_1280x720_LOCATION = new Vector2(775, 195);
        private readonly Vector2 RESOLUTION_1600x900_LOCATION = new Vector2(775, 240);
        private readonly Vector2 RESOLUTION_1920x1080_LOCATION = new Vector2(775, 285);
        private readonly Vector2 RESOLUTION_DEFAULT_LOCATION = new Vector2(1000, 345);

        private readonly Vector2 VOLUME_UP_LOCATION = new Vector2(1100, 450);
        private readonly Vector2 VOLUME_DOWN_LOCATION = new Vector2(775, 450);
        private readonly Vector2 VOLUME_DEFAULT_LOCATION = new Vector2(1000, 500);

        private readonly Vector2 MOVE_UP_LOCATION = new Vector2(220, 150);
        private readonly Vector2 MOVE_DOWN_LOCATION = new Vector2(220, 220);
        private readonly Vector2 MOVE_LEFT_LOCATION = new Vector2(220, 290);
        private readonly Vector2 MOVE_RIGHT_LOCATION = new Vector2(220, 360);
        private readonly Vector2 CONFIRM_LOCATION = new Vector2(500, 150);
        private readonly Vector2 BACK_LOCATION = new Vector2(500, 220);
        private readonly Vector2 CAST_LOCATION = new Vector2(500, 290);
        private readonly Vector2 CYCLE_LOCATION = new Vector2(500, 360);
        private readonly Vector2 START_LOCATION = new Vector2(500, 430);
        private readonly Vector2 KEYBIND_DEFAULT_LOCATION = new Vector2(500, 500);

        private readonly Vector2 DEFAULTS_LOCATION = new Vector2(850, 575);
        private readonly Vector2 MENU_BACK_LOCATION = new Vector2(1030, 575);

        private readonly string DEFAULT_TEXT = "Default";
        private readonly string VOLUME_UP_TEXT = "+";
        private readonly string VOLUME_DOWN_TEXT = "-";

        private readonly string DEFAULTS_TEXT = "Defaults";
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
            _resolution960Button = new MenuButton(RESOLUTION_960x540_LOCATION, string.Empty, ButtonStyles.Resolution, new ResolutionChangeEventArgs(Resolutions.r960x540), ButtonType.Radio);
            _resolution1280Button = new MenuButton(RESOLUTION_1280x720_LOCATION, string.Empty, ButtonStyles.Resolution, new ResolutionChangeEventArgs(Resolutions.r1280x720), ButtonType.Radio);
            _resolution1600Button = new MenuButton(RESOLUTION_1600x900_LOCATION, string.Empty, ButtonStyles.Resolution, new ResolutionChangeEventArgs(Resolutions.r1600x900), ButtonType.Radio);
            _resolution1920Button = new MenuButton(RESOLUTION_1920x1080_LOCATION, string.Empty, ButtonStyles.Resolution, new ResolutionChangeEventArgs(Resolutions.r1920x1080), ButtonType.Radio);
            _resolutionDefaultButton = new MenuButton(RESOLUTION_DEFAULT_LOCATION, string.Empty, ButtonStyles.SmallBack, new ResetPreferencesEventArgs(PreferenceTypes.Resolution), ButtonType.Radio);

            _volumeUpButton = new MenuButton(VOLUME_UP_LOCATION, VOLUME_UP_TEXT, ButtonStyles.Volume, new VolumeChangeEventArgs(true));
            _volumeDownButton = new MenuButton(VOLUME_DOWN_LOCATION, VOLUME_DOWN_TEXT, ButtonStyles.Volume, new VolumeChangeEventArgs(false));
            _volumeDefaultButton = new MenuButton(VOLUME_DEFAULT_LOCATION, DEFAULT_TEXT, ButtonStyles.SmallBack, new ResetPreferencesEventArgs(PreferenceTypes.Volume));
            
            _moveUpButton = new MenuButton(MOVE_UP_LOCATION, string.Empty, ButtonStyles.Keybind, null);
            _moveDownButton = new MenuButton(MOVE_DOWN_LOCATION, string.Empty, ButtonStyles.Keybind, null);
            _moveLeftButton = new MenuButton(MOVE_LEFT_LOCATION, string.Empty, ButtonStyles.Keybind, null);
            _moveRightButton = new MenuButton(MOVE_RIGHT_LOCATION, string.Empty, ButtonStyles.Keybind, null);
            _confirmButton = new MenuButton(CONFIRM_LOCATION, string.Empty, ButtonStyles.Keybind, null);
            _backButton = new MenuButton(BACK_LOCATION, string.Empty, ButtonStyles.Keybind, null);
            _castButton = new MenuButton(CAST_LOCATION, string.Empty, ButtonStyles.Keybind, null);
            _cycleButton = new MenuButton(CYCLE_LOCATION, string.Empty, ButtonStyles.Keybind, null);
            _startButton = new MenuButton(START_LOCATION, string.Empty, ButtonStyles.Keybind, null);
            _keysDefaultButton = new MenuButton(KEYBIND_DEFAULT_LOCATION, DEFAULTS_TEXT, ButtonStyles.SmallBack, new ResetPreferencesEventArgs(PreferenceTypes.Keybinds));

            _moveUpButton.Args = new KeybindChangeEventArgs(ControlFunctions.MoveUp, _moveUpButton);
            _moveDownButton.Args = new KeybindChangeEventArgs(ControlFunctions.MoveDown, _moveDownButton);
            _moveLeftButton.Args = new KeybindChangeEventArgs(ControlFunctions.MoveLeft, _moveLeftButton);
            _moveRightButton.Args = new KeybindChangeEventArgs(ControlFunctions.MoveRight, _moveRightButton);
            _confirmButton.Args = new KeybindChangeEventArgs(ControlFunctions.Confirm, _confirmButton);
            _backButton.Args = new KeybindChangeEventArgs(ControlFunctions.Back, _backButton);
            _castButton.Args = new KeybindChangeEventArgs(ControlFunctions.Cast, _castButton);
            _cycleButton.Args = new KeybindChangeEventArgs(ControlFunctions.Cycle, _cycleButton);
            _startButton.Args = new KeybindChangeEventArgs(ControlFunctions.Menu, _startButton);

            _defaultsButton = new MenuButton(DEFAULTS_LOCATION, DEFAULTS_TEXT, ButtonStyles.SmallBack, new ResetPreferencesEventArgs(PreferenceTypes.All));
            _menuBackButton = new MenuButton(MENU_BACK_LOCATION, BACK_TEXT, ButtonStyles.SmallBack, _previousMenuArgs); // need to change this if enter from exit

            #region Tie Together Buttons

            _moveUpButton.UpButton = _moveRightButton;
            _moveUpButton.DownButton = _moveDownButton;
            _moveUpButton.LeftButton = _resolution960Button;
            _moveUpButton.RightButton = _confirmButton;
            _moveDownButton.UpButton = _moveUpButton;
            _moveDownButton.DownButton = _moveLeftButton;
            _moveDownButton.LeftButton = _resolution1600Button;
            _moveDownButton.RightButton = _backButton;
            _moveLeftButton.UpButton = _moveDownButton;
            _moveLeftButton.DownButton = _moveRightButton;
            _moveLeftButton.LeftButton = _resolution1920Button;
            _moveLeftButton.RightButton = _castButton;
            _moveRightButton.UpButton = _moveLeftButton;
            _moveRightButton.DownButton = _moveUpButton;
            _moveRightButton.LeftButton = _resolutionDefaultButton;
            _moveRightButton.RightButton = _cycleButton;

            _confirmButton.UpButton = _keysDefaultButton;
            _confirmButton.DownButton = _backButton;
            _confirmButton.LeftButton = _moveUpButton;
            _confirmButton.RightButton = _resolution960Button;
            _backButton.UpButton = _confirmButton;
            _backButton.DownButton = _castButton;
            _backButton.LeftButton = _moveDownButton;
            _backButton.RightButton = _resolution1600Button;
            _castButton.UpButton = _backButton;
            _castButton.DownButton = _cycleButton;
            _castButton.LeftButton = _moveLeftButton;
            _castButton.RightButton = _resolution1920Button;
            _cycleButton.UpButton = _castButton;
            _cycleButton.DownButton = _startButton;
            _cycleButton.LeftButton = _moveRightButton;
            _cycleButton.RightButton = _resolutionDefaultButton;
            _startButton.UpButton = _cycleButton;
            _startButton.DownButton = _keysDefaultButton;
            _startButton.LeftButton = _volumeUpButton;
            _startButton.RightButton = _volumeDownButton;
            _keysDefaultButton.UpButton = _startButton;
            _keysDefaultButton.DownButton = _confirmButton;
            _keysDefaultButton.LeftButton = _volumeDefaultButton;
            _keysDefaultButton.RightButton = _volumeDefaultButton;

            _resolution960Button.UpButton = _defaultsButton;
            _resolution960Button.DownButton = _resolution1280Button;
            _resolution960Button.LeftButton = _confirmButton;
            _resolution960Button.RightButton = _moveUpButton;
            _resolution1280Button.UpButton = _resolution960Button;
            _resolution1280Button.DownButton = _resolution1600Button;
            _resolution1280Button.LeftButton = _confirmButton;
            _resolution1280Button.RightButton = _moveUpButton;
            _resolution1600Button.UpButton = _resolution1280Button;
            _resolution1600Button.DownButton = _resolution1920Button;
            _resolution1600Button.LeftButton = _backButton;
            _resolution1600Button.RightButton = _moveDownButton;
            _resolution1920Button.UpButton = _resolution1600Button;
            _resolution1920Button.DownButton = _resolutionDefaultButton;
            _resolution1920Button.LeftButton = _castButton;
            _resolution1920Button.RightButton = _moveLeftButton;
            _resolutionDefaultButton.UpButton = _resolution1920Button;
            _resolutionDefaultButton.DownButton = _volumeUpButton;
            _resolutionDefaultButton.LeftButton = _cycleButton;
            _resolutionDefaultButton.RightButton = _moveRightButton;

            _volumeUpButton.UpButton = _resolutionDefaultButton;
            _volumeUpButton.DownButton = _volumeDefaultButton;
            _volumeUpButton.LeftButton = _volumeDownButton;
            _volumeUpButton.RightButton = _startButton;
            _volumeDownButton.UpButton = _resolutionDefaultButton;
            _volumeDownButton.DownButton = _volumeDefaultButton;
            _volumeDownButton.LeftButton = _startButton;
            _volumeDownButton.RightButton = _volumeUpButton;
            _volumeDefaultButton.UpButton = _volumeUpButton;
            _volumeDefaultButton.DownButton = _menuBackButton;
            _volumeDefaultButton.LeftButton = _keysDefaultButton;
            _volumeDefaultButton.RightButton = _keysDefaultButton;

            _defaultsButton.LeftButton = _menuBackButton;
            _defaultsButton.RightButton = _menuBackButton;
            _defaultsButton.UpButton = _volumeDefaultButton;
            _defaultsButton.DownButton = _resolution960Button;
            _menuBackButton.LeftButton = _defaultsButton;
            _menuBackButton.RightButton = _defaultsButton;
            _menuBackButton.UpButton = _volumeDefaultButton;
            _menuBackButton.DownButton = _resolution960Button;

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
            // might want to do some null check here?
            _moveUpButton.Text = InputHelper.GetStringBasedOnConnectedController(data.Keybindings.FirstOrDefault(k => k.Key == ControlFunctions.MoveUp).Value[0],
                data.ButtonBindings.FirstOrDefault(k => k.Key == ControlFunctions.MoveUp).Value[0]);
            _moveDownButton.Text = InputHelper.GetStringBasedOnConnectedController(data.Keybindings.FirstOrDefault(k => k.Key == ControlFunctions.MoveDown).Value[0],
                data.ButtonBindings.FirstOrDefault(k => k.Key == ControlFunctions.MoveDown).Value[0]);
            _moveLeftButton.Text = InputHelper.GetStringBasedOnConnectedController(data.Keybindings.FirstOrDefault(k => k.Key == ControlFunctions.MoveLeft).Value[0],
                data.ButtonBindings.FirstOrDefault(k => k.Key == ControlFunctions.MoveLeft).Value[0]);
            _moveRightButton.Text = InputHelper.GetStringBasedOnConnectedController(data.Keybindings.FirstOrDefault(k => k.Key == ControlFunctions.MoveRight).Value[0],
                data.ButtonBindings.FirstOrDefault(k => k.Key == ControlFunctions.MoveRight).Value[0]);

            _confirmButton.Text = InputHelper.GetStringBasedOnConnectedController(data.Keybindings.FirstOrDefault(k => k.Key == ControlFunctions.Confirm).Value[0],
                data.ButtonBindings.FirstOrDefault(k => k.Key == ControlFunctions.Confirm).Value[0]);
            _backButton.Text = InputHelper.GetStringBasedOnConnectedController(data.Keybindings.FirstOrDefault(k => k.Key == ControlFunctions.Back).Value[0],
                data.ButtonBindings.FirstOrDefault(k => k.Key == ControlFunctions.Back).Value[0]);
            _castButton.Text = InputHelper.GetStringBasedOnConnectedController(data.Keybindings.FirstOrDefault(k => k.Key == ControlFunctions.Cast).Value[0],
                data.ButtonBindings.FirstOrDefault(k => k.Key == ControlFunctions.Cast).Value[0]);
            _cycleButton.Text = InputHelper.GetStringBasedOnConnectedController(data.Keybindings.FirstOrDefault(k => k.Key == ControlFunctions.Cycle).Value[0],
                data.ButtonBindings.FirstOrDefault(k => k.Key == ControlFunctions.Cycle).Value[0]);
            _startButton.Text = InputHelper.GetStringBasedOnConnectedController(data.Keybindings.FirstOrDefault(k => k.Key == ControlFunctions.Menu).Value[0],
                data.ButtonBindings.FirstOrDefault(k => k.Key == ControlFunctions.Menu).Value[0]);

            if (data.Resolution == Resolutions.r960x540)
                _resolution960Button.SelectNoEvent();
            else if (data.Resolution == Resolutions.r1280x720)
                _resolution1280Button.SelectNoEvent();
            else if (data.Resolution == Resolutions.r1600x900)
                _resolution1600Button.SelectNoEvent();
            else if (data.Resolution == Resolutions.r1920x1080)
                _resolution1920Button.SelectNoEvent();

        }

        public override void EnterMenu(MenuPageNames name, PreferenceData data)
        {
            UnhideAllButtons();
            UpdateWithPreferenceData(data);

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
            if (_resolution960Button != _currentButton && _resolution960Button.State == ButtonStates.Selected)
                _resolution960Button.SelectNoEvent();

            if (_resolution1280Button != _currentButton && _resolutionDefaultButton != _currentButton && _resolution1280Button.State == ButtonStates.Selected) 
                _resolution1280Button.SelectNoEvent();

            if (_resolution1600Button != _currentButton && _resolution1600Button.State == ButtonStates.Selected)
                _resolution1600Button.SelectNoEvent();

            if (_resolution1920Button != _currentButton && _resolution1920Button.State == ButtonStates.Selected)
                _resolution1920Button.SelectNoEvent();

            if ((_resolutionDefaultButton == _currentButton || _defaultsButton == _currentButton) && !(_resolution1280Button.State == ButtonStates.Selected))
                _resolution1280Button.SelectNoEvent();
        }
    }
}
