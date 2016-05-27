using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.Menu;
using Element.Common.Data;

namespace Element.Common.Menus.MenuPages
{
    public class FileSelectMenuPage : MenuPage
    {
        private readonly Vector2 FILE_0_BUTTON_LOCATION = new Vector2(0, 0);
        private readonly Vector2 FILE_1_BUTTON_LOCATION = new Vector2(0, 0);
        private readonly Vector2 FILE_2_BUTTON_LOCATION = new Vector2(0, 0);

        private readonly Vector2 BACK_LOCATION = new Vector2(0, 0);
        private readonly Vector2 ERASE_LOCATION = new Vector2(0, 0);

        private readonly string BACK_TEXT = "Back";
        private readonly string ERASE_TEXT = "Erase";

        private MenuButton _file0Button;
        private MenuButton _file1Button;
        private MenuButton _file2Button;
        private MenuButton _backButton;
        private MenuButton _eraseButton;

        private MenuDialog _confirmEraseDialog;

        private int _fileHighlightIndex;
        private int _utilityButtonHighlightIndex;
        private bool _onFileButtons;

        private SwitchPageEventArgs _previousMenuArgs;

        public FileSelectMenuPage() : base()
        {
            _name = MenuPageNames.FileSelect;
            _fileHighlightIndex = 0;
            _utilityButtonHighlightIndex = 0;

            _file0Button = new MenuButton(FILE_0_BUTTON_LOCATION, string.Empty, ButtonStyles.FileSelect, new StartOrLoadEventArgs(0));
            _file1Button = new MenuButton(FILE_1_BUTTON_LOCATION, string.Empty, ButtonStyles.FileSelect, new StartOrLoadEventArgs(1));
            _file2Button = new MenuButton(FILE_2_BUTTON_LOCATION, string.Empty, ButtonStyles.FileSelect, new StartOrLoadEventArgs(2));

            _backButton = new MenuButton(BACK_LOCATION, BACK_TEXT, ButtonStyles.SmallBack, new SwitchPageEventArgs(MenuPageNames.Start, _name));
            _eraseButton = new MenuButton(ERASE_LOCATION, ERASE_TEXT, ButtonStyles.SmallBack, new PageEventArgs());

            _file0Button.RightButton = _file1Button;
            _file0Button.LeftButton = _file2Button;
            _file1Button.RightButton = _file2Button;
            _file1Button.LeftButton = _file0Button;
            _file2Button.RightButton = _file0Button;
            _file2Button.LeftButton = _file1Button;

            _file0Button.DownButton = _backButton;
            _file1Button.DownButton = _backButton;
            _file2Button.DownButton = _backButton;

            _backButton.LeftButton = _eraseButton;
            _backButton.RightButton = _eraseButton;
            _eraseButton.LeftButton = _backButton;
            _eraseButton.RightButton = _backButton;

            _backButton.UpButton = _file1Button;
            _eraseButton.UpButton = _file1Button; // maybe need another menu page for file erase?

            _buttons.Add(_file0Button);
            _buttons.Add(_file1Button);
            _buttons.Add(_file2Button);
            _buttons.Add(_backButton);
            _buttons.Add(_eraseButton);

            _file0Button.OnSelected += RaiseLoadGameEvent;
            _file1Button.OnSelected += RaiseLoadGameEvent;
            _file2Button.OnSelected += RaiseLoadGameEvent;
            _backButton.OnSelected += RaiseSwitchPageEvent;
            _eraseButton.OnSelected += OnErase;

            _currentButton = _file0Button;
            _onFileButtons = true;

            _previousMenuArgs = new SwitchPageEventArgs(MenuPageNames.Start, _name);
        }

        private void OnErase(MenuPageEventArgs e)
        {
            _eraseButton.Disable();

            _file0Button.ResetEventHandlers();
            _file1Button.ResetEventHandlers();
            _file2Button.ResetEventHandlers();
            _file0Button.OnSelected += OnEraseFile;
            _file1Button.OnSelected += OnEraseFile;
            _file2Button.OnSelected += OnEraseFile;
            _file0Button.Args = new OpenDialogEventArgs();
            _file1Button.Args = new OpenDialogEventArgs();
            _file2Button.Args = new OpenDialogEventArgs();
            _file0Button.OnSelected += RaiseOpenDialogEvent;
            _file1Button.OnSelected += RaiseOpenDialogEvent;
            _file2Button.OnSelected += RaiseOpenDialogEvent;

            HighlightFileBasedOnIndex();
        }

        private void ResetFileButtons(MenuPageEventArgs e)
        {
            ResetFileButtons();
            HighlightFileBasedOnIndex();
        }

        private void ResetFileButtons()
        {
            _eraseButton.Enable();

            _file0Button.ResetEventHandlers();
            _file1Button.ResetEventHandlers();
            _file2Button.ResetEventHandlers();

            _file0Button.OnSelected += RaiseLoadGameEvent;
            _file1Button.OnSelected += RaiseLoadGameEvent;
            _file2Button.OnSelected += RaiseLoadGameEvent;
            _file0Button.Args = new StartOrLoadEventArgs(0);
            _file1Button.Args = new StartOrLoadEventArgs(1);
            _file2Button.Args = new StartOrLoadEventArgs(2);

            HighlightFileBasedOnIndex();
        }

        private void HighlightFileBasedOnIndex()
        {
            if (_fileHighlightIndex == 0)
                _currentButton = _file0Button;
            else if (_fileHighlightIndex == 1)
                _currentButton = _file1Button;
            else if (_fileHighlightIndex == 2)
                _currentButton = _file2Button;
            else
            {
                Console.WriteLine("WARN: STRANGE FILE HIGHLIGHT INDEX: " + _fileHighlightIndex);
                _currentButton = _file0Button;
                _fileHighlightIndex = 0;
            }

            _currentButton.Highlight();
            _onFileButtons = true;
        }

        private void HighlightUtilityBasedOnIndex()
        {
            if (_utilityButtonHighlightIndex == 0)
                _currentButton = _eraseButton;
            else if (_utilityButtonHighlightIndex == 1)
                _currentButton = _backButton;
            else
            {
                Console.WriteLine("WARN: STRANGE FILE UTILITY HIGHLIGHT INDEX: " + _utilityButtonHighlightIndex);
                _currentButton = _backButton;
                _utilityButtonHighlightIndex = 0;
            }

            _currentButton.Highlight();
            _onFileButtons = false;
        }

        private MenuButton GetFileButtonFromIndex()
        {
            if (_fileHighlightIndex == 0)
                return _file0Button;
            else if (_fileHighlightIndex == 1)
                return _file1Button;
            else if (_fileHighlightIndex == 2)
                return _file2Button;
            else
                return _file0Button;
        }

        private void OnEraseFile(MenuPageEventArgs e)
        {
            _dialogOpen = true;
            var dialog = new MenuDialog();
            dialog.AddTextLine("Are you sure?");

            var yesButton = new MenuButton(new Vector2(0, 0), "Erase", ButtonStyles.DialogTwo, new EraseFileEventArgs(_fileHighlightIndex));
            yesButton.OnSelected += RaiseEraseFileEvent;
            yesButton.OnSelected += ResetFileButtons;
            yesButton.OnSelected += RaiseCloseDialogEvent;

            var noButton = new MenuButton(new Vector2(0, 0), "Cancel", ButtonStyles.DialogTwo, new CloseDialogEventArgs());
            noButton.OnSelected += RaiseCloseDialogEvent;
            noButton.OnSelected += ResetFileButtons;

            yesButton.LeftButton = noButton;
            yesButton.RightButton = noButton;
            noButton.LeftButton = yesButton;
            noButton.RightButton = yesButton;

            dialog.AddButton(yesButton);
            dialog.AddButton(noButton);

            _currentDialog = dialog;
        }

        private void OnLoadFile(MenuPageEventArgs e)
        {
            _dialogOpen = true;
            var dialog = new MenuDialog();
            dialog.AddTextLine("Any unsaved progress will be lost.");

            var okButton = new MenuButton(new Vector2(0, 0), "Ok", ButtonStyles.DialogTwo, new StartOrLoadEventArgs(_fileHighlightIndex));
            okButton.OnSelected += RaiseLoadGameEvent;
            okButton.OnSelected += RaiseCloseDialogEvent;

            var cancelButton = new MenuButton(new Vector2(0, 0), "Cancel", ButtonStyles.DialogTwo, new CloseDialogEventArgs());
            cancelButton.OnSelected += RaiseCloseDialogEvent;
            cancelButton.OnSelected += ResetFileButtons;
        }

        public void EnterFromExit(bool save)
        {
            // should change what the buttons do
            ResetFileButtons();

            if (save)
            {
                _eraseButton.Disable();
                _file0Button.Args = new SaveGameEventArgs(0);
                _file1Button.Args = new SaveGameEventArgs(1);
                _file2Button.Args = new SaveGameEventArgs(2);

                _file0Button.OnSelected += RaiseSaveGameEvent;
                _file1Button.OnSelected += RaiseSaveGameEvent;
                _file2Button.OnSelected += RaiseSaveGameEvent;
            }
            else
            {
                // put another dialog here "unsaved data will be lost"
                _eraseButton.Enable();
                _file0Button.Args = new OpenDialogEventArgs();
                _file1Button.Args = new OpenDialogEventArgs();
                _file2Button.Args = new OpenDialogEventArgs();

                _file0Button.OnSelected += OnLoadFile;
                _file1Button.OnSelected += OnLoadFile;
                _file2Button.OnSelected += OnLoadFile;
                _file0Button.OnSelected += RaiseOpenDialogEvent;
                _file1Button.OnSelected += RaiseOpenDialogEvent;
                _file2Button.OnSelected += RaiseOpenDialogEvent;
            }
        }

        public override void UpdateWithPreferenceData(PreferenceData data)
        {
            throw new NotImplementedException();
        }

        public override void EnterMenu(MenuPageNames name, PreferenceData data)
        {
            UnhideAllButtons();

            _backButton.Enable();
            _eraseButton.Enable();

            HighlightFileBasedOnIndex();

            if (name == MenuPageNames.Start)
            {
                _previousMenuArgs = new SwitchPageEventArgs(MenuPageNames.Start, _name);
                _backButton.Args = _previousMenuArgs;
                ResetFileButtons();
            }
            else
            {
                _previousMenuArgs = new SwitchPageEventArgs(MenuPageNames.ExitMenu, _name);
                _backButton.Args = _previousMenuArgs;
            }
        }

        public override void ReturnToPreviousMenu()
        {
            if (_dialogOpen)
            {
                HighlightFileBasedOnIndex();
                RaiseCloseDialogEvent(new CloseDialogEventArgs());
                ResetFileButtons();
            }
            else // need to change this if we enter from exit
                RaiseSwitchPageEvent(_previousMenuArgs);
        }

        public override void MoveCursor(Directions dir)
        {
            if (_dialogOpen)
            {
                MoveCursorDialogOpen(dir);
                return;
            }

            if (dir == Directions.Up || dir == Directions.Down)
            {
                if (_onFileButtons)
                    HighlightUtilityBasedOnIndex();
                else
                    HighlightFileBasedOnIndex();
            }
            else if (dir == Directions.Left)
            {
                if (_onFileButtons)
                {
                    _fileHighlightIndex--;

                    if (_fileHighlightIndex < 0)
                        _fileHighlightIndex = 2;

                    HighlightFileBasedOnIndex();
                }
                else
                {
                    _utilityButtonHighlightIndex--;

                    if (_utilityButtonHighlightIndex < 0)
                        _utilityButtonHighlightIndex = 1;

                    HighlightUtilityBasedOnIndex();
                }
            }
            else if (dir == Directions.Right)
            {
                if (_onFileButtons)
                {
                    _fileHighlightIndex++;

                    if (_fileHighlightIndex > 2)
                        _fileHighlightIndex = 0;

                    HighlightFileBasedOnIndex();
                }
                else
                {
                    _utilityButtonHighlightIndex++;

                    if (_utilityButtonHighlightIndex > 1)
                        _utilityButtonHighlightIndex = 0;

                    HighlightUtilityBasedOnIndex();
                }
            }            
        }

        private void MoveCursorDialogOpen(Directions dir)
        {
            if (dir == Directions.Left)
            {
                _currentButton.DeHighlight();
                _currentButton = _currentButton.LeftButton;
                _currentButton.Highlight();
            }
            else if (dir == Directions.Right)
            {
                _currentButton.DeHighlight();
                _currentButton = _currentButton.RightButton;
                _currentButton.Highlight();
            }
        }
    }
}
