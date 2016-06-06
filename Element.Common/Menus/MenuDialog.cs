using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Element.Common.Menus
{
    public class MenuDialog
    {
        private string _text;
        private int _lines;

        private List<MenuButton> _buttons;

        public MenuDialog()
        {
            _text = string.Empty;
            _lines = 0;

            _buttons = new List<MenuButton>();
        }

        public void AddTextLine(string text) // might need to add some logic to keep adding lines if text is too long
        {
            _text += text + "\n";
            _lines++;
        }

        public void AddButton(MenuButton button)
        {
            _buttons.Add(button); // i know this all is somewhat silly at this point but just leave it for now

            if (_buttons.Count == 1)
                button.Location = new Vector2(65, 70);
            else if (_buttons.Count == 2)
            {
                _buttons[0].Location = new Vector2(10, 70);
                _buttons[1].Location = new Vector2(120, 70);
            }
            else
            {
                // account for this later I suppose
            }
        }

        public string Text
        {
            get { return _text; }
        }

        public List<MenuButton> Buttons
        {
            get { return _buttons; }
        }

        public int Lines
        {
            get { return _lines; }
        }
    }
}
