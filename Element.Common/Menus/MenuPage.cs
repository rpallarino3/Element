using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.Menu;
using Element.Common.Data;

namespace Element.Common.Menus
{
    public abstract class MenuPage
    {
        protected MenuPageNames _name;
        protected List<MenuButton> _buttons;
        protected MenuButton _currentButton;
        protected MenuDialog _currentDialog;
        protected bool _dialogOpen;

        public MenuPage()
        {
            _buttons = new List<MenuButton>();
        }

        #region Methods

        public abstract void UpdateWithPreferenceData(PreferenceData data);
        public abstract void EnterMenu(MenuPageNames name, PreferenceData data);
        public abstract void ReturnToPreviousMenu();

        public virtual void SelectButton()
        {
            if (_dialogOpen && _currentDialog.Buttons.Count == 0)
                return;

            _currentButton.SelectButton();
        }

        public virtual void MoveCursor(Directions dir)
        {
            MenuButton newButton = null;

            if (dir == Directions.Left && _currentButton.LeftButton != null)
                newButton = _currentButton.LeftButton;
            else if (dir == Directions.Right && _currentButton.RightButton != null)
                newButton = _currentButton.RightButton;
            else if (dir == Directions.Up && _currentButton.UpButton != null)
                newButton = _currentButton.UpButton;
            else if (dir == Directions.Down && _currentButton.DownButton != null)
                newButton = _currentButton.DownButton;

            if (newButton == null)
                return;

            _currentButton.DeHighlight();
            _currentButton = newButton;
            _currentButton.Highlight();
        }

        public bool DoneFading()
        {
            foreach (var button in _buttons)
            {
                if (button.State == ButtonStates.FadeIn || button.State == ButtonStates.FadeOut)
                    return false;
            }

            return true;
        }

        protected void UnhideAllButtons()
        {
            foreach (var button in _buttons)
                button.Enable();
        }

        protected void RaiseOpenDialogEvent(MenuPageEventArgs e)
        {
            if (e == null)
                return;

            OpenDialog(e);
        }

        protected void RaiseCloseDialogEvent(MenuPageEventArgs e)
        {
            if (e == null)
                return;

            CloseDialog(e);
        }

        protected void RaiseSwitchPageEvent(MenuPageEventArgs e)
        {
            if (e == null)
                return;

            SwitchPage(e);
        }

        protected void RaiseEraseFileEvent(MenuPageEventArgs e)
        {
            if (e == null)
                return;

            EraseFile(e);
        }

        protected void RaiseSaveGameEvent(MenuPageEventArgs e)
        {
            if (e == null)
                return;

            SaveGame(e);
        }

        protected void RaiseLoadGameEvent(MenuPageEventArgs e)
        {
            if (e == null)
                return;

            LoadGame(e);
        }

        protected void RaiseKeybindChangeEvent(MenuPageEventArgs e)
        {
            if (e == null)
                return;

            KeybindChange(e);
        }

        protected void RaiseVolumeChangeEvent(MenuPageEventArgs e)
        {
            if (e == null)
                return;

            VolumeChange(e);
        }

        protected void RaiseResolutionChangeEvent(MenuPageEventArgs e)
        {
            if (e == null)
                return;

            ResolutionChange(e);
        }

        protected void RaiseExitGameEvent(MenuPageEventArgs e)
        {
            if (e == null)
                return;

            ExitGame(e);
        }

        protected void RaisePreferenceResetEvent(MenuPageEventArgs e)
        {
            if (e == null)
                return;

            ResetPreferences(e);
        }

        protected void RaiseResumeGameEvent(MenuPageEventArgs e)
        {
            if (e == null)
                return;

            ResumeGame(e);
        }

        #endregion

        public MenuPageNames Name
        {
            get { return _name; }
        }

        public List<MenuButton> Buttons
        {
            get { return _buttons; }
        }

        public MenuButton CurrentButton
        {
            get { return _currentButton; }
        }

        public MenuDialog CurrentDialog
        {
            get { return _currentDialog; }
        }

        public bool DialogOpen
        {
            get { return _dialogOpen; }
        }

        #region Events

        public event MenuPageEvent OpenDialog;
        public event MenuPageEvent CloseDialog;
        public event MenuPageEvent SwitchPage;
        public event MenuPageEvent SaveGame;
        public event MenuPageEvent LoadGame;
        public event MenuPageEvent EraseFile;
        public event MenuPageEvent KeybindChange;
        public event MenuPageEvent ResetPreferences;
        public event MenuPageEvent VolumeChange;
        public event MenuPageEvent ResolutionChange;
        public event MenuPageEvent ExitGame;
        public event MenuPageEvent ResumeGame;

        #endregion
    }
}
