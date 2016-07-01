using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.NPCs;
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
        private static NpcAction _actionInFront;
        
        static PlayerLogicHandler()
        {
            _region = RegionNames.None;
            _actionInFront = NpcAction.None;
        }

        public static void UpdatePlayerLogic()
        {
            // should check if the player is 'locked' in an object at some point
            // should also remember to recheck in front if we turn

            var exit = CheckMenu();
            if (exit)
            {
                _actionInFront = NpcAction.None;
                return;
            }

            exit = CheckCast();
            if (exit)
            {
                _actionInFront = NpcAction.None;
                return;
            }

            exit = CheckConfirm();
            if (exit)
            {
                _actionInFront = NpcAction.None;
                return;
            }

            exit = CheckGrab();
            if (exit)
            {
                _actionInFront = NpcAction.None; // maybe change this to release, not sure if we will ever get here
                return;
            }

            exit = CheckMove();
            if (exit)
            {
                _actionInFront = NpcAction.None;
                return;
            }

            exit = CheckCycle();
            if (exit) return;
        }

        private static bool CheckMenu()
        {
            return false;
        }

        private static bool CheckCast()
        {
            if (!InputHandler.IsFunctionReady(ControlFunctions.Cast))
                return false;

            if (!_player.CanExecuteInFacingDirection(NpcAction.Cast))
                return false;

            _player.ExecuteActionInFacingDirection(NpcAction.Cast);
            Cast();

            return true;
        }

        private static void Cast()
        {

        }

        private static bool CheckConfirm()
        {
            CheckInFrontForConfirmAction(); // so we know this will always get evaluated
            if (!InputHandler.IsFunctionReady(ControlFunctions.Confirm))
                return false;

            // we know that the player can execute the action or it will be none which does nothing

            _player.ExecuteActionInFacingDirection(_actionInFront); // probably won't do much
            ExecuteConfirmAction();
            return true;
        }

        private static void CheckInFrontForConfirmAction()
        {
            _actionInFront = NpcAction.None; // figure out the action here

            if (!_player.CanExecuteInFacingDirection(_actionInFront))
                _actionInFront = NpcAction.None;            
        }

        private static void ExecuteConfirmAction()
        {

        }

        private static bool CheckGrab()
        {
            if (InputHandler.IsFunctionReady(ControlFunctions.Grab))
                return false;

            _player.ReleaseGrab();

            return false; // return false for now, could be true
        }

        private static bool CheckMove()
        {
            var longestDirection = InputHandler.GetLongestDirection();
            var longestTime = InputHandler.GetLongestTime();

            _player.SetRun(InputHandler.IsFunctionReady(ControlFunctions.Run));

            if (longestTime == 0 || longestDirection == null)
                return false;

            if (longestDirection != _player.FacingDirection && longestTime <= GameConstants.TURN_THRESHOLD)
            {
                if (_player.CanExecute(NpcAction.Turn, longestDirection.Value)) // should only be able to turn if standing
                {
                    _player.ExecuteAction(NpcAction.Turn, longestDirection.Value);
                    return false;
                }
            }

            // check for transition here too
            // is longest direction necessarily the direction we want?
            // yes we want longest direction and not facing direction

            var movementAction = GetMovementAction(longestDirection.Value); // maybe use the TileMapHandler so we can reuse some of the methods

            if (_player.CanExecute(movementAction, longestDirection.Value))
                return false;

            _player.ExecuteAction(movementAction, longestDirection.Value);

            return false;
        }

        private static NpcAction GetMovementAction(Directions direction)
        {
            var currentTile = TrafficHandler.GetTile(_region, _zone, _player.TileLocation, _player.Level);
            var destinationTile = TrafficHandler.GetTileInDirection(direction, _region, _zone, _player.TileLocation, _player.Level);
            var destinationTileBelow = TrafficHandler.GetTileBelow(direction, _region, _zone, _player.TileLocation, _player.Level);
            var landingTile = TrafficHandler.GetTileInDirection(direction, _region, _zone, _player.TileLocation, _player.Level, 2);
            var landingTileBloew = TrafficHandler.GetTileBelow(direction, _region, _zone, _player.TileLocation, _player.Level, 2);

            if (!currentTile.CanMoveOffTile(direction))
                return NpcAction.None;

            var destinationAction = destinationTile.GetMoveActionFromTile(direction);

            if (destinationAction == NpcAction.Push)
            {
                // push
                return NpcAction.Push;
            }
            else if (destinationAction == NpcAction.Jump)
            {
                // jump, check below for climb down
                return NpcAction.Jump;
            }
            else
                return destinationAction;            
        }

        private static bool CheckCycle()
        {
            return false;
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
