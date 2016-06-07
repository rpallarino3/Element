using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.Menu;
using Element.Common.Data;

namespace Element.Common.Menus.MenuPages
{
    public class StartMenuPage : MenuPage
    {
        private readonly Vector2 START_FILE_SELECT_LOCATION = new Vector2(390, 150);
        private readonly Vector2 START_OPTIONS_LOCATION = new Vector2(390, 280);
        private readonly Vector2 START_EXIT_LOCATION = new Vector2(390, 410);

        private readonly string FILE_SELECT = "File Select";
        private readonly string OPTIONS = "Options";
        private readonly string EXIT = "Exit";

        private MenuButton _fileSelect;
        private MenuButton _options;
        private MenuButton _exit;

        public StartMenuPage() : base()
        {
            _name = MenuPageNames.Start;
            _fileSelect = new MenuButton(START_FILE_SELECT_LOCATION, FILE_SELECT, ButtonStyles.Basic, new SwitchPageEventArgs(MenuPageNames.FileSelect, _name));
            _options = new MenuButton(START_OPTIONS_LOCATION, OPTIONS, ButtonStyles.Basic, new SwitchPageEventArgs(MenuPageNames.Options, _name));
            _exit = new MenuButton(START_EXIT_LOCATION, EXIT, ButtonStyles.Basic, new ExitGameEventArgs());

            _fileSelect.DownButton = _options;
            _fileSelect.UpButton = _exit;

            _options.UpButton = _fileSelect;
            _options.DownButton = _exit;

            _exit.UpButton = _options;
            _exit.DownButton = _fileSelect;

            _buttons.Add(_fileSelect);
            _buttons.Add(_options);
            _buttons.Add(_exit);

            _fileSelect.OnSelected += RaiseSwitchPageEvent;
            _options.OnSelected += RaiseSwitchPageEvent;
            _exit.OnSelected += RaiseExitGameEvent;
        }

        public override void UpdateWithPreferenceData(PreferenceData data)
        {
            // does nothing
        }

        public override void EnterMenu(MenuPageNames name, PreferenceData data)
        {
            UnhideAllButtons();
            UpdateWithPreferenceData(data);

            if (name == MenuPageNames.FileSelect)
                _currentButton = _fileSelect;
            else if (name == MenuPageNames.Options)
                _currentButton = _options;
            else if (name == MenuPageNames.Title)
                _currentButton = _fileSelect;
            else if (name == MenuPageNames.ExitMenu)
                _currentButton = _fileSelect;

            _currentButton.Highlight();
        }

        public override void ReturnToPreviousMenu()
        {
            RaiseSwitchPageEvent(new SwitchPageEventArgs(MenuPageNames.Title, _name));
        }
    }
}
