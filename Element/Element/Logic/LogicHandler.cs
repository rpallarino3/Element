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
    public static class LogicHandler
    {
        public static void SetStartingState()
        {
            GameStateHelper.ChangeState(GameStates.Start);
        }

        public static void UpdateGameLogic()
        {
            if (TransitionHandler.Transitioning)
            {
                TransitionHandler.ContinueTransition();
            }
            else if (GameStateHelper.CurrentState == GameStates.Start)
            {
                GameStateHelper.ChangeState(GameStates.StartMenu);
            }
            else if (GameStateHelper.CurrentState == GameStates.StartMenu || GameStateHelper.CurrentState == GameStates.ExitMenu)
            {
                StartAndExitMenuLogicHandler.UpdateGameLogic();
            }
            else if (GameStateHelper.CurrentState == GameStates.Roam)
            {
                RoamLogicHandler.UpdateLogic();
            }
            else if (GameStateHelper.CurrentState == GameStates.Chat)
            {

            }            
        }
        
        public static Color DrawColor { get { return TransitionHandler.DrawColor; } }        
    }
}
