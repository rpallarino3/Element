using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.Menu;
using Element.Common.Data;

namespace Element.Common.Menus.MenuPages
{
    public class TitleMenuPage : MenuPage
    {
        private readonly Vector2 TITLE_BUTTON_LOCATION = new Vector2(340, 450);

        private MenuButton _titleButton;

        public TitleMenuPage() : base()
        {
            _name = MenuPageNames.Title;
            _titleButton = new MenuButton(TITLE_BUTTON_LOCATION, string.Empty, ButtonStyles.Title, new SwitchPageEventArgs(MenuPageNames.Start, _name));
            _buttons.Add(_titleButton);

            _currentButton = _titleButton;

            _titleButton.OnSelected += RaiseSwitchPageEvent;
        }

        public override void UpdateWithPreferenceData(PreferenceData data)
        {
            // does nothing here
        }

        public override void EnterMenu(MenuPageNames name, PreferenceData data)
        {
            UnhideAllButtons();
            UpdateWithPreferenceData(data);

            _currentButton = _titleButton;
            _currentButton.Highlight();
        }

        public override void ReturnToPreviousMenu()
        {
            // does nothing
        }
    }
}
