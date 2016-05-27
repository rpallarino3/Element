using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        
        private GameStates _queuedUpState;
        private bool _paused;

        public LogicHandler(ResourceManager resourceManager, InputHandler inputHandler)
        {
            _resourceManager = resourceManager;
            _inputHandler = inputHandler;
            _playerLogicHandler = new PlayerLogicHandler();
            _roamLogicHandler = new RoamLogicHandler(resourceManager);
            _startAndExitMenuLogicHandler = new StartAndExitMenuLogicHandler(resourceManager, inputHandler);
            _transitionHandler = new TransitionHandler(resourceManager, _playerLogicHandler, _roamLogicHandler, _startAndExitMenuLogicHandler);

            GameStateHelper.ChangeState(GameStates.StartMenu);
        }

        public void UpdateGameLogic()
        {
            if (_transitionHandler.Transitioning)
            {
                if (!_transitionHandler.ContinueTransition()) // maybe change how this game state stuff works
                    return;
            }
            
        }

        public bool Saving { get { return _transitionHandler.Saving; } }

        public StartAndExitMenuLogicHandler StartAndExitMenuLogicHandler { get { return _startAndExitMenuLogicHandler; } }
    }
}
