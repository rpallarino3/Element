using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.ResourceManagement;
using Element.ResourceManagement.RegionGeneration;
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
            
            _roamLogicHandler.InitiateTransition += TransitionRequested;
            _startAndExitMenuLogicHandler.InitiateTransition += TransitionRequested;
        }

        private void TransitionRequested(Transition transition)
        {
            if (transition == null)
                return;

            _transition = transition;

            if (transition.Fade) // fading is required between all things that would require us to wait on load? set the transitioning field here
                FadeOut();
            else
                ExecuteTransition();
        }

        public void ExecuteTransition()
        {
            if (_transition is StateTransition)
                ExecuteStateTransition();
            else if (_transition is RoamTransition)
                ExecuteRoamTransition();
            else if (_transition is ChatTransition)
                ExecuteChatTransition();
            else if (_transition is CameraTransition)
                ExecuteCameraTransition();
        }

        private void ExecuteStateTransition()
        {
            var transition = _transition as StateTransition;

            if (transition == null) // do we fade in here? no not necessariliy, this should never happen though
                return;

            // state transition only has the state we are going to.
            // this state is pretty much guaranteed to not be roam unless coming from exit menu resume or maybe from chat?
            // don't have to set transitioning to false since it will be handled elsewhere
            GameStateHelper.ChangeState(transition.DestinationState);
        }

        private void ExecuteRoamTransition()
        {
            var transition = _transition as RoamTransition;

            if (transition == null)
                return;
            
            if (transition.DestinationRegion == _roamLogicHandler.CurrentPlayerRegion && GameStateHelper.CurrentState == GameStates.Roam)
            {
                _roamLogicHandler.UpdatePlayerPositionWithTransition(transition);
                return;
            }

            var adjacentRegions = RegionLayout.RegionInfo[transition.DestinationRegion].AdjacentRegions;
            //var adjacentRegions = RegionMapper.GetAdjacentRegions(transition.DestinationRegion); // this is going to become regions to load
            var currentlyLoadedRegions = new List<RegionNames>(_roamLogicHandler.Regions.Keys.ToList()); // this is going to become regions to unload?

            List<RegionNames> regionsToLoad = new List<RegionNames>();
            List<RegionNames> regionsToUnload = new List<RegionNames>();

            // load the destination region if it isn't already loaded (this will happen sometimes when loading files)
            if (!currentlyLoadedRegions.Contains(transition.DestinationRegion))
                regionsToLoad.Add(transition.DestinationRegion);

            // load everything this is adjacent to the region we are going to but isn't currently loaded
            foreach (var region in adjacentRegions)
            {
                if (!currentlyLoadedRegions.Contains(region))
                    regionsToLoad.Add(region);
            }

            // unload everything that is current loaded but no adjacent to the region we are currently going to
            foreach (var region in currentlyLoadedRegions)
            {
                if (!adjacentRegions.Contains(region))
                    regionsToUnload.Add(region);
            }

            _resourceManager.RequestRegionLoadUnload(regionsToLoad, regionsToUnload);

            // we can wait on load if we are loading new regions and fading
            if (transition.Fade)
                _waitOnLoad = true;

            _roamLogicHandler.UpdatePlayerPositionWithTransition(transition);

            if (GameStateHelper.CurrentState != GameStates.Roam)
                GameStateHelper.ChangeState(GameStates.Roam);
        }
        
        private void ExecuteChatTransition()
        {
            var transition = _transition as ChatTransition;

            if (transition == null)
                return;
        }

        private void ExecuteCameraTransition()
        {
            var transition = _transition as CameraTransition;

            if (transition == null)
                return;
        }

        public void ContinueTransition()
        {
            if (_fading)
            {
                ContinueFade();
                return;
            }
            else if (_waitOnLoad)
            {
                CheckLoad();
            }
            else
            {
                ExecuteTransition();

                if (!_waitOnLoad)
                    FadeIn(); // we would always want to fade in here?
            }
        }

        private void CheckLoad()
        {
            if (!_resourceManager.IsBackgroundThreadClear())
                return;

            _waitOnLoad = false;
            FadeIn();
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
                    _transitioning = false; // when we are done fading in the transition is over
                }
            }
        }

        private void FadeOut()
        {
            _transitioning = true;
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

        public Color DrawColor { get { return _drawColor; } }
        public bool Transitioning { get { return _transitioning; } }
        public bool ShowRegionImage {  get { return (_waitOnLoad && !_fading); } } // this doesn't neccessarily mean we want to show image, need to think about this more
        public RegionNames RegionLoadToDisplay { get; private set; }
    }
}
