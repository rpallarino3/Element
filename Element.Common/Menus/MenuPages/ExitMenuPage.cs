using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.Menu;
using Element.Common.Data;

namespace Element.Common.Menus.MenuPages
{
    public class ExitMenuPage : MenuPage
    {
        private readonly Vector2 RESUME_GAME_LOCATION = new Vector2(515, 205);
        private readonly Vector2 SAVE_GAME_LOCATION = new Vector2(515, 270);
        private readonly Vector2 LOAD_GAME_LOCATION = new Vector2(515, 335);
        private readonly Vector2 OPTIONS_LOCAITON = new Vector2(515, 400);
        private readonly Vector2 EXIT_GAME_LOCATION = new Vector2(515, 465);

        private readonly string RESUME_TEXT = "Resume";
        private readonly string SAVE_TEXT = "Save Game";
        private readonly string LOAD_TEXT = "Load Game";
        private readonly string OPTIONS_TEXT = "Options";
        private readonly string EXIT_TEXT = "Exit Game";

        private MenuButton _resumeButton;
        private MenuButton _saveButton;
        private MenuButton _loadButton;
        private MenuButton _optionsButton;
        private MenuButton _exitButton;

        public ExitMenuPage() : base()
        {
            _name = MenuPageNames.ExitMenu;
            _resumeButton = new MenuButton(RESUME_GAME_LOCATION, RESUME_TEXT, ButtonStyles.ExitBasic, new ResumeGameEventArgs());
            _saveButton = new MenuButton(SAVE_GAME_LOCATION, SAVE_TEXT, ButtonStyles.ExitBasic, new SwitchPageEventArgs(MenuPageNames.FileSelect, _name, true));
            _loadButton = new MenuButton(LOAD_GAME_LOCATION, LOAD_TEXT, ButtonStyles.ExitBasic, new SwitchPageEventArgs(MenuPageNames.FileSelect, _name, false));
            _optionsButton = new MenuButton(OPTIONS_LOCAITON, OPTIONS_TEXT, ButtonStyles.ExitBasic, new SwitchPageEventArgs(MenuPageNames.Options, _name));
            _exitButton = new MenuButton(EXIT_GAME_LOCATION, EXIT_TEXT, ButtonStyles.ExitBasic, new ExitGameEventArgs()); // could possibly return to start menu

            _resumeButton.UpButton = _exitButton;
            _resumeButton.DownButton = _saveButton;
            _saveButton.UpButton = _resumeButton;
            _saveButton.DownButton = _loadButton;
            _loadButton.UpButton = _saveButton;
            _loadButton.DownButton = _optionsButton;
            _optionsButton.UpButton = _loadButton;
            _optionsButton.DownButton = _exitButton;
            _exitButton.UpButton = _optionsButton;
            _exitButton.DownButton = _resumeButton;

            _resumeButton.OnSelected += RaiseResumeGameEvent;
            _saveButton.OnSelected += RaiseSwitchPageEvent;
            _loadButton.OnSelected += RaiseSwitchPageEvent;
            _optionsButton.OnSelected += RaiseSwitchPageEvent;
            _exitButton.OnSelected += RaiseExitGameEvent;

            _buttons.Add(_resumeButton);
            _buttons.Add(_saveButton);
            _buttons.Add(_loadButton);
            _buttons.Add(_optionsButton);
            _buttons.Add(_exitButton);
        }

        public override void UpdateWithPreferenceData(PreferenceData data)
        {
            // does nothing
        }

        public override void EnterMenu(MenuPageNames name, PreferenceData data)
        {
            UnhideAllButtons();

            _currentButton = _resumeButton;
            _resumeButton.Highlight();
        }

        public override void ReturnToPreviousMenu()
        {
            RaiseResumeGameEvent(new ResumeGameEventArgs());
        }
    }
}
