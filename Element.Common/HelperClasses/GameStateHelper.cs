using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.GameBasics;

namespace Element.Common.HelperClasses
{
    public static class GameStateHelper
    {
        private static GameStates _currentState;

        public static void ChangeState(GameStates newState)
        {
            StateChange(new StateChangeEventArgs(_currentState, newState));
            _currentState = newState;
        }

        public static GameStates CurrentState
        {
            get { return _currentState; }
        }

        public static event StateChangeEvent StateChange;
    }

    public class StateChangeEventArgs
    {
        public StateChangeEventArgs(GameStates prevState, GameStates newState)
        {
            PreviousState = prevState;
            NewState = newState;
        }

        public GameStates PreviousState { get; set; }
        public GameStates NewState { get; set; }
    }

    public delegate void StateChangeEvent(StateChangeEventArgs e);
}
