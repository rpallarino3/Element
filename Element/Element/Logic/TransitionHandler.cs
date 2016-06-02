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
            
            _resourceManager.SaveCompleted += SaveCompleted; // this isn't what we want i don't think

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
        }

        private void ExecuteStateTransition()
        {
            var transition = _transition as StateTransition;

            if (transition == null) // do we fade in here? no not necessariliy, this should never happen though
                return;

            // state transition only has the state we are going to.
            // this state is pretty much guaranteed to not be roam unless coming from exit menu resume?
            // don't have to set transitioning to false since it will be handled elsewhere
            GameStateHelper.ChangeState(transition.DestinationState);
        }

        private void ExecuteRoamTransition()
        {
            var transition = _transition as RoamTransition;

            if (transition == null)
                return;

            // this is where this gets somewhat complicated
            // where are we keeping the current region/etc?
            // i guess it's fine to leave this at this point for now
        }

        private void ExecuteChatTransition()
        {
            var transition = _transition as ChatTransition;

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
                FadeIn(); // we would always want to fade in here?
            }
        }

        private void CheckLoad()
        {
            if (_loading)
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

        private void SaveCompleted(SaveEventArgs e)
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
