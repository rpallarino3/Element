using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.Menu;
using Element.Common.Environment;
using Element.Common.HelperClasses;
using Element.Common.Menus;
using Element.Common.Menus.MenuPages;
using Element.Input;
using Element.ResourceManagement;

namespace Element.Logic
{
    /// <summary>
    /// This whole class is a fucking mess. There is definitely a better way to do this but this should work ok.
    /// </summary>
    public class StartAndExitMenuLogicHandler
    {
        private const int INPUT_DELAY = 5;
        private const int SWAP_WAIT_FRAMES = 20;

        private ResourceManager _resourceManager;
        private InputHandler _inputHandler;

        private Dictionary<MenuPageNames, MenuPage> _menuPages;
        private MenuPage _activeMenuPage;

        private int _inputCounter;

        private bool _requestKeybindChange;
        private ControlFunctions _functionToChange;

        private bool _dialogOpen;

        private bool _switchPages;
        private SwitchPageEventArgs _switchPageArgs;

        public StartAndExitMenuLogicHandler(ResourceManager resourceManager, InputHandler inputHandler)
        {
            GameStateHelper.StateChange += StateChange;

            _resourceManager = resourceManager;
            _inputHandler = inputHandler;

            _inputCounter = 0;

            CreateMenuPages();

            _activeMenuPage = _menuPages[MenuPageNames.Title];

            // need to have some way to update with preference data when we first enter a page
        }

        private void CreateMenuPages()
        {
            var titlePage = new TitleMenuPage();
            var startPage = new StartMenuPage();
            var fileSelectPage = new FileSelectMenuPage();
            var optionsPage = new OptionsMenuPage();
            var exitMenuPage = new ExitMenuPage();

            #region TieEvents

            titlePage.OpenDialog += OpenDialog;
            startPage.OpenDialog += OpenDialog;
            fileSelectPage.OpenDialog += OpenDialog;
            optionsPage.OpenDialog += OpenDialog;
            exitMenuPage.OpenDialog += OpenDialog;

            titlePage.CloseDialog += CloseDialog;
            startPage.CloseDialog += CloseDialog;
            fileSelectPage.CloseDialog += CloseDialog;
            optionsPage.CloseDialog += CloseDialog;
            exitMenuPage.CloseDialog += CloseDialog;

            titlePage.SwitchPage += SwitchMenuPage;
            startPage.SwitchPage += SwitchMenuPage;
            fileSelectPage.SwitchPage += SwitchMenuPage;
            optionsPage.SwitchPage += SwitchMenuPage;
            exitMenuPage.SwitchPage += SwitchMenuPage;

            titlePage.SaveGame += SaveGame;
            startPage.SaveGame += SaveGame;
            fileSelectPage.SaveGame += SaveGame;
            optionsPage.SaveGame += SaveGame;
            exitMenuPage.SaveGame += SaveGame;

            titlePage.LoadGame += StartOrLoadGame;
            startPage.LoadGame += StartOrLoadGame;
            fileSelectPage.LoadGame += StartOrLoadGame;
            optionsPage.LoadGame += StartOrLoadGame;
            exitMenuPage.LoadGame += StartOrLoadGame;

            titlePage.EraseFile += EraseFile;
            startPage.EraseFile += EraseFile;
            fileSelectPage.EraseFile += EraseFile;
            optionsPage.EraseFile += EraseFile;
            exitMenuPage.EraseFile += EraseFile;

            titlePage.KeybindChange += KeybindChange;
            startPage.KeybindChange += KeybindChange;
            fileSelectPage.KeybindChange += KeybindChange;
            optionsPage.KeybindChange += KeybindChange;
            exitMenuPage.KeybindChange += KeybindChange;

            titlePage.ResetPreferences += ResetPreferences;
            startPage.ResetPreferences += ResetPreferences;
            fileSelectPage.ResetPreferences += ResetPreferences;
            optionsPage.ResetPreferences += ResetPreferences;
            exitMenuPage.ResetPreferences += ResetPreferences;

            titlePage.VolumeChange += VolumeChange;
            startPage.VolumeChange += VolumeChange;
            fileSelectPage.VolumeChange += VolumeChange;
            optionsPage.VolumeChange += VolumeChange;
            exitMenuPage.VolumeChange += VolumeChange;

            titlePage.ResolutionChange += ResolutionChange;
            startPage.ResolutionChange += ResolutionChange;
            fileSelectPage.ResolutionChange += ResolutionChange;
            optionsPage.ResolutionChange += ResolutionChange;
            exitMenuPage.ResolutionChange += ResolutionChange;

            titlePage.ExitGame += ExitGame;
            startPage.ExitGame += ExitGame;
            fileSelectPage.ExitGame += ExitGame;
            optionsPage.ExitGame += ExitGame;
            exitMenuPage.ExitGame += ExitGame;

            titlePage.StateTransition += StateTransition;
            startPage.StateTransition += StateTransition;
            fileSelectPage.StateTransition += StateTransition;
            optionsPage.StateTransition += StateTransition;
            exitMenuPage.StateTransition += StateTransition;

            #endregion

            titlePage.UpdateWithPreferenceData(DataHelper.PreferenceData);
            startPage.UpdateWithPreferenceData(DataHelper.PreferenceData);
            fileSelectPage.UpdateWithPreferenceData(DataHelper.PreferenceData);
            optionsPage.UpdateWithPreferenceData(DataHelper.PreferenceData);
            exitMenuPage.UpdateWithPreferenceData(DataHelper.PreferenceData);

            _menuPages.Add(MenuPageNames.Title, titlePage);
            _menuPages.Add(MenuPageNames.Start, startPage);
            _menuPages.Add(MenuPageNames.FileSelect, fileSelectPage);
            _menuPages.Add(MenuPageNames.Options, optionsPage);
            _menuPages.Add(MenuPageNames.ExitMenu, exitMenuPage);
            
        }

        public void UpdateGameLogic()
        {
            // might want to add something in here to wait for saves/loads that are requested from menus

            foreach (var button in _activeMenuPage.Buttons)
                button.UpdateOwnLogic();

            if (_dialogOpen)
            {
                foreach (var button in _activeMenuPage.CurrentDialog.Buttons)
                    button.UpdateOwnLogic();
            }

            if (_switchPages)
            {
                if (!_activeMenuPage.DoneFading())
                    return;

                if (_activeMenuPage.Name == _switchPageArgs.Page)
                {
                    _switchPages = false;
                    _activeMenuPage.EnterMenu(_switchPageArgs.PreviousPage, DataHelper.PreferenceData);

                    if (_activeMenuPage.Name == MenuPageNames.FileSelect)
                    {
                        if (_switchPageArgs.EnterFromExit != null)
                            ((FileSelectMenuPage)_activeMenuPage).EnterFromExit(_switchPageArgs.EnterFromExit.Value);
                    }

                    return;
                }

                _activeMenuPage = _menuPages[_switchPageArgs.Page];

                foreach (var button in _activeMenuPage.Buttons)
                    button.FadeIn();

                return; // i think we want to do this
            }

            if (_inputCounter < INPUT_DELAY)
            {
                _inputCounter++; // only reset this when we actually do something
                return;
            }

            if (_requestKeybindChange)
            {
                // this will update the preference data if a key is redbound
                var changed = _inputHandler.RequestSingleKeyRebind(_functionToChange);

                if (changed)
                {
                    ((OptionsMenuPage)_menuPages[MenuPageNames.Options]).CloseKeybindDialog();
                    ((OptionsMenuPage)_menuPages[MenuPageNames.Options]).UpdateWithPreferenceData(DataHelper.PreferenceData);
                    _dialogOpen = false;
                    _requestKeybindChange = false;
                }
            }
            else if (_activeMenuPage.Name == MenuPageNames.Title)
            {
                if (_inputHandler.RequestSingleKeypress())
                {
                    // this should cause the switch page event to go
                    _activeMenuPage.SelectButton();
                }
            }
            else
            {
                // take the input and manipulate the buttons and shit
            }
        }

        #region Event Handlers

        #region Menu Changes

        private void OpenDialog(MenuPageEventArgs e)
        {
            var args = e as OpenDialogEventArgs;

            if (args == null)
                return;

            _dialogOpen = true;
        }

        private void CloseDialog(MenuPageEventArgs e)
        {
            var args = e as CloseDialogEventArgs;

            if (args == null)
                return;

            _dialogOpen = false;
            // leave it to the menus to highlight the proper button
            // really doesn't matter if buttons behind highlight slightly before
        }

        private void SwitchMenuPage(MenuPageEventArgs e)
        {
            var args = e as SwitchPageEventArgs;

            if (args == null)
                return;

            foreach (var button in _activeMenuPage.Buttons)
                button.FadeOut();

            _switchPages = true;
            _switchPageArgs = args;
        }

        #endregion

        #region Data Changes

        private void SaveGame(MenuPageEventArgs e)
        {
            var args = e as SaveGameEventArgs;

            if (args == null)
                return;

            // this should only come from the exit menu?
        }

        private void StartOrLoadGame(MenuPageEventArgs e)
        {
            // at this point we would already have the data loaded
            var args = e as StartOrLoadEventArgs;

            if (args == null)
                return;

            var fileNumber = args.FileNumber;
            
            // i guess here is where we just initiate a state transition and start a new game?
        }

        private void EraseFile(MenuPageEventArgs e)
        {
            var args = e as EraseFileEventArgs;

            if (args == null)
                return;

            _resourceManager.EraseFile(args.FileNumber);
        }

        #endregion

        #region Preference Changes

        private void KeybindChange(MenuPageEventArgs e)
        {
            var args = e as KeybindChangeEventArgs;

            if (args == null)
                return;

            _requestKeybindChange = true;
            _functionToChange = args.Function;
        }

        private void ResetPreferences(MenuPageEventArgs e)
        {
            var args = e as ResetPreferencesEventArgs;

            if (args == null)
                return;

            if (args.Type == PreferenceTypes.All)
            {
                ResolutionChanged(new ResolutionChangeEventArgs(PreferenceValidator.DefaultResolution));
                _resourceManager.ResetPreferenceData();
            }
            else if (args.Type == PreferenceTypes.Keybinds)
            {
                _resourceManager.ResetPreferenceKeybinds();
                ((OptionsMenuPage)_menuPages[MenuPageNames.Options]).UpdateWithPreferenceData(DataHelper.PreferenceData);
            }
            else if (args.Type == PreferenceTypes.Resolution)
            {
                ResolutionChange(new ResolutionChangeEventArgs(PreferenceValidator.DefaultResolution));
            }
            else if (args.Type == PreferenceTypes.Volume)
            {
                _resourceManager.UpdatePreferenceVolumeData(PreferenceValidator.DefaultVolume);
            }
        }

        private void VolumeChange(MenuPageEventArgs e)
        {
            var args = e as VolumeChangeEventArgs;

            if (args == null)
                return;

            _resourceManager.UpdatePreferenceVolumeData(args.Up);
        }

        private void ResolutionChange(MenuPageEventArgs e)
        {
            var args = e as ResolutionChangeEventArgs;

            if (args == null)
                return;

            ResolutionChanged(args);
            _resourceManager.UpdatePreferenceResolutionData(args.Resolution);
        }

        #endregion

        #region State Changes

        private void ExitGame(MenuPageEventArgs e)
        {
            var args = e as ExitGameEventArgs;

            if (args == null)
                return;

            ExitGame(args);
        }

        private void StateTransition(MenuPageEventArgs e)
        {

        }

        #endregion

        #endregion

        private void StartNewGame()
        {
            var transition = new CombinationTransition();

            var args = new TransitionEventArgs();
            args.Transition = transition;

            InitiateTransition(args);
        }

        private void LoadGame(int index)
        {
            // if this comes from the start menu we should just get teh save data from the saveloadhandler that has already been loaded
            // put in a state transition and a load transition?
            var transition = new CombinationTransition();

            var args = new TransitionEventArgs();
            args.Transition = transition;

            InitiateTransition(args);
        }

        public void StateChange(StateChangeEventArgs e)
        {
            if (e.NewState == GameStates.StartMenu)
            {
                // i think we can start all the buttons fading and update the title with preference data
            }
            else if (e.NewState == GameStates.ExitMenu)
            {
                // do something here about entering the menu
            }
        }

        public event TransitionEvent InitiateTransition;
        public event MenuPageEvent ResolutionChanged;
        public event MenuPageEvent BeginExit;
    }
}
