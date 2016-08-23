using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.GameObjects;
using Element.Common.Environment.Tiles;
using Element.Common.GameObjects.Actions;
using Element.Common.GameObjects.Npcs;
using Element.Common.HelperClasses;
using Element.Input;

namespace Element.Logic
{
    public static class PlayerLogicHandler
    {
        private static Npc _player;
        private static RegionNames _region;
        private static int _zone;

        private static bool _moving;
        private static ActionInFrontType _inFrontAction;
        
        static PlayerLogicHandler()
        {
            _region = RegionNames.None;
        }

        public static void UpdatePlayerLogic()
        {
            if (InputHandler.IsFunctionReady(ControlFunctions.Menu))
            {
                // do some menu shit here
                return;
            }

            var actions = new List<AvailableAction>();

            if (InputHandler.IsFunctionReady(ControlFunctions.Cast))
            {
                // either we get the priority from some static class or just put it here
                var castAction = new AvailableAction(GameObjectActionType.Cast, _player.FacingDirection, 0);
                actions.Add(castAction);
            }

            if (InputHandler.IsFunctionReady(ControlFunctions.Confirm))
            {
                var interactAction = new AvailableAction(GameObjectActionType.Interact, _player.FacingDirection, 1);
                actions.Add(interactAction);
            }

            if (InputHandler.IsFunctionReady(ControlFunctions.Grab))
            {
                var grabAction = new AvailableAction(GameObjectActionType.Grab, _player.FacingDirection, 2);
                actions.Add(grabAction);
            }
            else
            {
                var releaseAction = new AvailableAction(GameObjectActionType.ReleaseGrab, _player.FacingDirection, 3);
                actions.Add(releaseAction);
            }

            var longestTime = InputHandler.GetLongestTime();
            var longestDirection = InputHandler.GetLongestDirection();

            if (!longestDirection.HasValue) // this should also mean that longest time = 0
                _moving = false;
            else
            {
                if (_player.Climbing)
                {
                    var climbAction = new AvailableAction(GameObjectActionType.Climb, longestDirection.Value, 4);
                    actions.Add(climbAction);
                }
                else
                {
                    if (longestTime < GameConstants.TURN_THRESHOLD && !_moving)
                    {
                        var turnAction = new AvailableAction(GameObjectActionType.Turn, longestDirection.Value, 5);
                        actions.Add(turnAction);
                    }
                    else
                    {
                        _moving = true;
                        if (_player.Grabbing)
                        {
                            var pullAction = new AvailableAction(GameObjectActionType.Pull, longestDirection.Value, 5);
                            actions.Add(pullAction);
                        }
                        else
                        {
                            var pushAction = new AvailableAction(GameObjectActionType.Push, longestDirection.Value, 5);
                        }

                        var jumpAction = new AvailableAction(GameObjectActionType.Jump, longestDirection.Value, 6);
                        actions.Add(jumpAction);
                        var walkAction = new AvailableAction(GameObjectActionType.Walk, longestDirection.Value, 7);
                        actions.Add(walkAction);
                    }
                }
            }

            if (InputHandler.IsFunctionReady(ControlFunctions.Run))
                _player.SetRun(true);
            else
                _player.SetRun(false);

            if (InputHandler.IsFunctionReady(ControlFunctions.Cycle))
            {
                // do some cycle stuff here
            }

            _player.PassAvailableActions(actions);
            _player.UpdateLogic();
            SetConfirmAction();
            // maybe we need to get confirm action in front
            // i think this is all we should need here for now
        }

        private static void SetConfirmAction()
        {
            // we have none, interact, talk, check, grab/release
            if (!_player.CanExecute(GameObjectActionType.Interact, _player.FacingDirection))
                _inFrontAction = ActionInFrontType.None;

            var destinationTile = TrafficHandler.GetTileInDirection(_player.FacingDirection, _region, _zone, _player.Position, _player.Level);

            if (destinationTile == null)
            {
                _inFrontAction = ActionInFrontType.None;
                return;
            }

            _inFrontAction = destinationTile.GetActionInFront();
        }
        
        public static Npc Player
        {
            get { return _player; }
        }

        public static RegionNames Region
        {
            get { return _region; }
            set { _region = value; }
        }

        public static int Zone
        {
            get { return _zone; }
            set { _zone = value; }
        }
    }
}
