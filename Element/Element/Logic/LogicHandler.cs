using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.ResourceManagement;
using Element.Input;
using Element.Common.Enumerations.GameBasics;
using Element.Common.HelperClasses;

namespace Element.Logic
{
    public class LogicHandler
    {
        private ResourceManager _resourceManager;
        private InputHandler _inputHandler;
        private TransitionHandler _transitionHandler;
        private PlayerLogicHandler _playerLogicHandler;
        private RoamLogicHandler _roamLogicHandler;
        private StartAndExitMenuLogicHandler _startAndExitMenuLogicHandler;
        
        public LogicHandler(ResourceManager resourceManager, InputHandler inputHandler)
        {
            _resourceManager = resourceManager;
            _inputHandler = inputHandler;
            _playerLogicHandler = new PlayerLogicHandler();
            _roamLogicHandler = new RoamLogicHandler(resourceManager);
            _startAndExitMenuLogicHandler = new StartAndExitMenuLogicHandler(resourceManager, inputHandler);
            _transitionHandler = new TransitionHandler(resourceManager, _playerLogicHandler, _roamLogicHandler, _startAndExitMenuLogicHandler);

            GameStateHelper.ChangeState(GameStates.Start);
        }

        public void UpdateGameLogic()
        {
            if (_transitionHandler.Transitioning)
            {
                _transitionHandler.ContinueTransition();
            }
            else if (GameStateHelper.CurrentState == GameStates.Start)
            {
                GameStateHelper.ChangeState(GameStates.StartMenu);
            }
            else if (GameStateHelper.CurrentState == GameStates.StartMenu)
            {
                _startAndExitMenuLogicHandler.UpdateGameLogic();
            }
            
        }

        public bool Saving { get { return _transitionHandler.Saving; } }

        public Color DrawColor { get { return _transitionHandler.DrawColor; } }

        public StartAndExitMenuLogicHandler StartAndExitMenuLogicHandler { get { return _startAndExitMenuLogicHandler; } }
    }
}
