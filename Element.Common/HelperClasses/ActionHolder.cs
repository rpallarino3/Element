using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.GameObjects;
using Element.Common.GameObjects.Actions;

namespace Element.Common.HelperClasses
{
    public static class ActionHolder
    {
        private static Dictionary<GameObjectActionType, GameObjectAction> _actionDictionary;

        static ActionHolder()
        {
            _actionDictionary = new Dictionary<GameObjectActionType, GameObjectAction>();
            _actionDictionary.Add(GameObjectActionType.Cast, new CastAction());
            _actionDictionary.Add(GameObjectActionType.Climb, new ClimbAction());
            _actionDictionary.Add(GameObjectActionType.Grab, new GrabAction());
            _actionDictionary.Add(GameObjectActionType.Jump, new JumpAction());
            _actionDictionary.Add(GameObjectActionType.Pull, new PullAction());
            _actionDictionary.Add(GameObjectActionType.Push, new PushAction());
            _actionDictionary.Add(GameObjectActionType.ReleaseGrab, new ReleaseGrabAction());
            _actionDictionary.Add(GameObjectActionType.Turn, new TurnAction());
            _actionDictionary.Add(GameObjectActionType.Walk, new WalkAction());
        }

        // cast, interact, grab, release grab, climb, turn, pull, push, jump, walk
        public static GameObjectAction GetAction(GameObjectActionType actionType)
        {
            if (_actionDictionary.ContainsKey(actionType))
                return _actionDictionary[actionType];
            else
            {
                Console.WriteLine("WARNING! ActionType without action requested: " + actionType);
                return null;
            }            
        }
    }
}
