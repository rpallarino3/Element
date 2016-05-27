using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.ResourceManagement;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Environment;
using Element.Common.HelperClasses;

namespace Element.Logic
{
    public class TransitionHandler
    {
        private ResourceManager _resourceManager;
        private PlayerLogicHandler _playerLogicHandler;
        private RoamLogicHandler _roamLogicHandler;
        private StartAndExitMenuLogicHandler _startAndExitMenuLogicHandler;

        private bool _transitioning;
        private bool? _stateTransition;
        private Transition _transition;

        private readonly Color WHITE_COLOR = Color.White;
        private readonly Color DIM_COLOR = new Color(100, 100, 100);
        private List<Color> _fadeColors;

        private Color _drawColor;
        private int _fadeCounter;
        private bool _fading;
        private bool _fadeOut;

        private bool _saving;
        private bool _loading;
        private bool _waitOnLoad;

        public TransitionHandler(ResourceManager resourceManager, PlayerLogicHandler playerLogicHandler, RoamLogicHandler roamLogicHandler,
            StartAndExitMenuLogicHandler startAndExitMenuLogicHandler)
        {
            _resourceManager = resourceManager;
            _playerLogicHandler = playerLogicHandler;
            _roamLogicHandler = roamLogicHandler;
            _startAndExitMenuLogicHandler = startAndExitMenuLogicHandler;

            _transitioning = false;
            _fadeCounter = 0;
            _drawColor = WHITE_COLOR;

            _fadeColors = new List<Color>();
            _fadeColors.Add(Color.White);
            _fadeColors.Add(new Color(225, 225, 225));
            _fadeColors.Add(new Color(225, 225, 225));
            _fadeColors.Add(new Color(200, 200, 200));
            _fadeColors.Add(new Color(200, 200, 200));
            _fadeColors.Add(new Color(175, 175, 175));
            _fadeColors.Add(new Color(175, 175, 175));
            _fadeColors.Add(new Color(150, 150, 150));
            _fadeColors.Add(new Color(150, 150, 150));
            _fadeColors.Add(new Color(125, 125, 125));
            _fadeColors.Add(new Color(125, 125, 125));
            _fadeColors.Add(new Color(100, 100, 100));
            _fadeColors.Add(new Color(100, 100, 100));
            _fadeColors.Add(new Color(75, 75, 75));
            _fadeColors.Add(new Color(75, 75, 75));
            _fadeColors.Add(new Color(50, 50, 50));
            _fadeColors.Add(new Color(50, 50, 50));
            _fadeColors.Add(new Color(25, 25, 25));
            _fadeColors.Add(new Color(25, 25, 25));
            _fadeColors.Add(Color.Black);

            _resourceManager.BgThread.SaveRequested += SaveRequested;
            _resourceManager.BgThread.SaveCompleted += SaveCompleted;
            _resourceManager.BgThread.AssetLoadRequested += LoadRequested;
            _resourceManager.BgThread.AllAssetsLoaded += LoadCompleted;

            _roamLogicHandler.InitiateTransition += TransitionRequested;
            _startAndExitMenuLogicHandler.InitiateTransition += TransitionRequested;
        }

        private void TransitionRequested(TransitionEventArgs e)
        {
            InitiateTransition(e.Transition);
        }

        public void InitiateTransition(Transition transition)
        {
            if (transition is StateTransition)
                ExecuteStateTransition((StateTransition)transition);
            else if (transition is RoamTransition)
                ExecuteRoamTransition((RoamTransition)transition);
            else if (transition is CombinationTransition)
                ExecuteComboTransition((CombinationTransition)transition);
        }

        private void ExecuteComboTransition(CombinationTransition transition)
        {
            _transitioning = true;
            _transition = transition;
            _stateTransition = null;

            if (transition.Fade)
            {
                FadeOut();
            }
        }

        private void ExecuteStateTransition(StateTransition transition)
        {
            _transitioning = true;
            _transition = transition;
            _stateTransition = true;

            if (transition.Fade)
            {
                FadeOut();
            }
            else if (transition.Dim)
            {
                _drawColor = DIM_COLOR;
            }
            else
            {
                _drawColor = WHITE_COLOR;
            }
        }

        private void ExecuteRoamTransition(RoamTransition transition)
        {
            _transitioning = true;
            _transition = transition;
            _stateTransition = false;

            if (transition.Fade)
            {
                FadeOut();
            }
        }

        public bool ContinueTransition(ref GameStates gameState)
        {
            if (_fading)
            {
                ContinueFade();
                return false;
            }
            else if (_waitOnLoad)
            {
                return EvaluateLoadWait();
            }
            else
            {
                if (_stateTransition == null)
                    return ContinueCombinationTransition(ref gameState);
                if (_stateTransition.Value == true)
                    return ContinueStateTransition(ref gameState);
                else
                    return ContinueRoamTransition(ref gameState);
            }
        }

        private bool ContinueCombinationTransition(ref GameStates gameState)
        {
            // the only time we have a combo transition is on start -> roam
            var transition = (CombinationTransition)_transition;

            if (_fadeOut)
            {
                CreateSaveLoadRequests(transition.DestinationRegion);

                _waitOnLoad = true;
                RegionLoadToDisplay = transition.DestinationRegion;

                _playerLogicHandler.Region = transition.DestinationRegion;
                _playerLogicHandler.Zone = transition.DestinationZone;
                _playerLogicHandler.Player.Location = transition.DestinationCoords;
                _playerLogicHandler.Player.Level = transition.DestinationLevel;

                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ContinueStateTransition(ref GameStates gameState)
        {
            var transition = (StateTransition)_transition;

            if (transition.Dim)
            {
                // dim generally means that we are not going to have to load any resources
                // the resources we need for a state change should already be loaded
                // don't want to dim automatically
                if (transition.DestinationState == GameStates.Roam)
                {
                    gameState = ((StateTransition)_transition).DestinationState;
                    _drawColor = WHITE_COLOR;
                }
                else if (transition.DestinationState == GameStates.ExitMenu)
                {
                    gameState = ((StateTransition)_transition).DestinationState;
                }
                else if (transition.DestinationState == GameStates.Chat)
                {
                    gameState = ((StateTransition)_transition).DestinationState;
                }

                return true;
            }
            else if (((StateTransition)_transition).Fade)
            {
                if (_fadeOut)
                {
                    if (transition.DestinationState == GameStates.Menu)
                    {
                        // can only come to menu from roam
                        // i guess just set up what menu to go to here?
                    }
                    else if (transition.DestinationState == GameStates.Roam)
                    {
                        // fading into a roam WITHOUT setting position can come from nothing atm.
                        // there might be something in the future that would want to come through this path though
                        // do we actually need to do anything here?
                    }
                    else if (transition.DestinationState == GameStates.StartMenu)
                    {
                        // unload all assets and reset game
                    }

                    gameState = ((StateTransition)_transition).DestinationState;
                    FadeIn();
                    return false;
                }
                else
                {
                    // this means that we are done fading in
                    return true;
                }
            }
            else
            {
                // this generally shouldn't happen unless we don't want to dim to chat?
                gameState = ((StateTransition)_transition).DestinationState;
                return true;
            }
            
        }

        private bool ContinueRoamTransition(ref GameStates gameState)
        {
            var transition = ((RoamTransition)_transition);

            if (transition.Fade)
            {
                if (_fadeOut)
                {
                    if (transition.DestinationRegion != _playerLogicHandler.Region)
                    {
                        if (_roamLogicHandler.IsRegionLoaded(transition.DestinationRegion))
                        {
                            _waitOnLoad = true;
                            RegionLoadToDisplay = transition.DestinationRegion;
                        }

                        CreateSaveLoadRequests(transition.DestinationRegion);

                        _playerLogicHandler.Region = transition.DestinationRegion;
                        _playerLogicHandler.Zone = transition.DestinationZone;
                        _playerLogicHandler.Player.Location = transition.DestinationCoords;
                        _playerLogicHandler.Player.Level = transition.DestinationLevel;
                        // need to fade in at some point
                    }
                    else
                    {
                        // possibly just a zone transition
                        // do we set the player zone from here?
                        // yes you set it here and then you 
                        _playerLogicHandler.Zone = transition.DestinationZone;
                        _playerLogicHandler.Player.Location = transition.DestinationCoords;
                        _playerLogicHandler.Player.Level = transition.DestinationLevel;
                        // set the player position here too
                    }
                    FadeIn();
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (transition.DestinationRegion != _playerLogicHandler.Region)
                {
                    if (_roamLogicHandler.IsRegionLoaded(transition.DestinationRegion)) // something is wrong if this is true
                    {
                        _waitOnLoad = true;
                        RegionLoadToDisplay = transition.DestinationRegion;
                        FadeIn();
                    }

                    CreateSaveLoadRequests(transition.DestinationRegion);

                    _playerLogicHandler.Region = transition.DestinationRegion;
                    _playerLogicHandler.Zone = transition.DestinationZone;
                    _playerLogicHandler.Player.Location = transition.DestinationCoords;
                    _playerLogicHandler.Player.Level = transition.DestinationLevel;
                }
                else
                {
                    // zone transition without a fade (within same region)
                    // i'm not sure if this will ever happen but put the logic here in case we want it
                    _playerLogicHandler.Zone = transition.DestinationZone;
                    _playerLogicHandler.Player.Location = transition.DestinationCoords;
                    _playerLogicHandler.Player.Level = transition.DestinationLevel;
                }
            }

            return false;
        }

        private bool EvaluateLoadWait()
        {
            if (!_loading)
            {
                if (_fading)
                    return false;

                FadeOut();
                _waitOnLoad = false;
            }

            return false;
        }

        private void CreateSaveLoadRequests(RegionNames region)
        {
            var regionLoadMessage = MessageCrafter.CreateRegionLoadMessage(region);
            _resourceManager.BgThread.AddMessageToQueue(regionLoadMessage);

            // get the regions we are unloading, get their region data, and put into save message
            var regionsToSave = new Dictionary<RegionNames, Region>();
            foreach (var otherRegion in regionLoadMessage.RegionsToUnload)
            {
                regionsToSave[otherRegion] = _roamLogicHandler.Regions[region];
            }

            _resourceManager.BgThread.AddMessageToQueue(MessageCrafter.CreateSaveLoadMessage(regionsToSave));
        }

        private void ContinueFade()
        {
            if (_fadeOut)
            {
                _fadeCounter++;
                _drawColor = _fadeColors[_fadeCounter];

                if (_fadeCounter == _fadeColors.Count - 1)
                {
                    _fading = false;
                }
            }
            else
            {
                _fadeCounter--;
                _drawColor = _fadeColors[_fadeCounter];

                if (_fadeCounter == 0)
                {
                    _fading = false;
                }
            }
        }

        private void FadeOut()
        {
            _fading = true;
            _fadeOut = true;
            _fadeCounter = 0;
        }

        private void FadeIn()
        {
            _fading = true;
            _fadeOut = false;
            _fadeCounter = _fadeColors.Count - 1;
        }

        private void LoadRequested()
        {
            _loading = true;
        }

        private void LoadCompleted()
        {
            _loading = false;
        }

        private void SaveRequested()
        {
            _saving = true;
        }

        private void SaveCompleted()
        {
            _saving = false;
        }

        public Color DrawColor { get { return _drawColor; } }
        public bool Transitioning { get { return _transitioning; } }
        public bool Saving { get { return _saving; } }
        public bool Loading { get { return _loading; } }
        public bool WaitOnLoad { get { return _waitOnLoad; } }
        public RegionNames RegionLoadToDisplay { get; private set; }
    }
}
