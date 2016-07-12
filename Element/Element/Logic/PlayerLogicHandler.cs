using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.NPCs;
using Element.Common.Enumerations.TileObjects;
using Element.Common.Environment.Tiles;
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

        private static Tile _currentTile;
        private static Tile _currentTileBelow;
        private static Tile _firstTile;
        private static Tile _firstTileBelow;
        private static Tile _secondTile;
        private static Tile _secondTileBelow;
        
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


            if (longestDirection == null && longestTime == 0) // these should always be true together?
                return false;
            

            // eh not sure if this is right
            if (_player.Climbing)
                return CheckClimbAction(longestDirection.Value, longestTime);

            if (longestDirection != _player.FacingDirection && longestTime <= GameConstants.TURN_THRESHOLD)
            {
                if (_player.CanExecute(NpcAction.Turn, longestDirection.Value)) // should only be able to turn if standing
                {
                    _player.ExecuteAction(NpcAction.Turn, longestDirection.Value);
                    return false;
                }
            }

            _currentTile = TrafficHandler.GetTile(_region, _zone, _player.TileLocation, _player.Level);

            // if (currentTile.Transition != null)
            // if (currentTile.Transition.Direction == direction || it is a fall in transition)
            // var exit = ExecuteTransition()
            // if (exit) return true;

            // is longest direction necessarily the direction we want?
            // yes we want longest direction and not facing direction

            var movementAction = GetMovementAction(longestDirection.Value); // maybe use the TileMapHandler so we can reuse some of the methods

            if (_player.CanExecute(movementAction, longestDirection.Value))
                return false;

            _player.ExecuteAction(movementAction, longestDirection.Value);

            return false;
        }

        private static bool CheckClimbAction(Directions longestDirection, int longestTime)
        {
            var action = TrafficHandler.GetClimbAction(longestDirection, _player.Region, _player.Zone, _player.Location, _player.Level);
            return false;
        }

        private static NpcAction GetMovementAction(Directions direction)
        {
            if (!_currentTile.CanMoveOffTile(direction))
                return NpcAction.None;

            if (_player.Grabbing)
                _firstTile = TrafficHandler.GetTileInDirection(_player.FacingDirection, _region, _zone, _player.TileLocation, _player.Level);
            else
                _firstTile = TrafficHandler.GetTileInDirection(direction, _region, _zone, _player.TileLocation, _player.Level);

            if (_firstTile == null)
                return NpcAction.TryWalk;

            var action = _firstTile.GetMoveActionFromTile(direction);

            if (action == NpcAction.Push)
                return CheckPush(direction);
            else if (action == NpcAction.Jump)
                return CheckJump(direction);
            else if (action == NpcAction.Fall)
            {
                return NpcAction.Fall;
            }
            else
                return action;            
        }

        private static NpcAction CheckPush(Directions direction)
        {
            bool pulling = false;

            if (TrafficHandler.GetOppositeDirection(_player.FacingDirection) == direction)
                pulling = true;
            else if (direction != _player.FacingDirection)
                return NpcAction.None; // trying to move in one of the non pulling/pushing directions

            if (!_firstTile.CanStandardObjectExecute(TileObjectActions.Pull, direction))
                return GetPushPull(pulling, true);
            
            var playerMoveTile = TrafficHandler.GetTileInDirection(direction, _region, _zone, _player.TileLocation, _player.Level);
            
            if (playerMoveTile == null)
                return GetPushPull(pulling, true);

            var canMoveInto = playerMoveTile.CanBeMovedInto(direction);
            
            if (canMoveInto.HasValue && !canMoveInto.Value)
                return GetPushPull(pulling, true);
            if (!canMoveInto.HasValue)
            {
                var playerMoveTileBelow = TrafficHandler.GetTileBelow(direction, _region, _zone, _player.TileLocation, _player.Level);

                if (playerMoveTileBelow == null)
                    return GetPushPull(pulling, true);

                if (!playerMoveTileBelow.CanMoveOnTop(direction)) // need to rethink all of these can move on top things
                    return GetPushPull(pulling, true);
            }
            
            bool canPush;

            if (pulling)
                canPush = TrafficHandler.CheckDownForPush(direction, _region, _zone, _player.TileLocation, _player.Level, 0);
            else
                canPush = TrafficHandler.CheckDownForPush(direction, _region, _zone, _player.TileLocation, _player.Level, 2);

            if (!canPush)
                return GetPushPull(pulling, true);
            
            //Tile objectDestinationTile;

            //if (pulling)
            //    objectDestinationTile = _currentTile;
            //else
            //    objectDestinationTile = TrafficHandler.GetTileInDirection(direction, _region, _zone, _player.TileLocation, _player.Level, 2);

            //if (objectDestinationTile == null)
            //    return GetPushPull(pulling, true);

            //var canMoveObj = objectDestinationTile.CanPushInto(direction);

            //if (canMoveObj.HasValue && !canMoveObj.Value)
            //    return GetPushPull(pulling, true);
            //if (!canMoveObj.HasValue) // hrm need to check all the way down at some point
            //{
            //    Tile objectDestinationTileBelow;

            //    if (pulling)
            //        objectDestinationTileBelow = TrafficHandler.GetTileBelow(_region, _zone, _player.TileLocation, _player.Level);
            //    else
            //        objectDestinationTileBelow = TrafficHandler.GetTileBelow(direction, _region, _zone, _player.TileLocation, _player.Level, 2);

            //    if (objectDestinationTileBelow == null)
            //        return GetPushPull(pulling, true);

            //    if (!objectDestinationTileBelow.CanMoveOnTop(direction))
            //        return GetPushPull(pulling, true);
            //}

            return GetPushPull(pulling, false);
        }

        private static NpcAction GetPushPull(bool pulling, bool trying)
        {
            if (trying)
            {
                if (pulling)
                    return NpcAction.TryPull;
                else
                    return NpcAction.TryPush;
            }
            else
            {
                if (pulling)
                    return NpcAction.Pull;
                else
                    return NpcAction.Push;
            }
        }

        private static NpcAction CheckJump(Directions direction)
        {
            _secondTile = TrafficHandler.GetTileInDirection(direction, _region, _zone, _player.TileLocation, _player.Level, 2);

            if (_secondTile == null)
                return NpcAction.None;

            var canLand = _secondTile.CanMoveOnTile(direction);

            if (canLand.HasValue)
            {
                if (canLand.Value)
                    return NpcAction.Jump;
                else
                    return NpcAction.None;
            }

            _secondTileBelow = TrafficHandler.GetTileBelow(direction, _region, _zone, _player.TileLocation, _player.Level, 2);

            if (_secondTileBelow == null)
                return NpcAction.None;

            if (!_secondTileBelow.CanMoveOnTop(direction))
                return NpcAction.None;

            return NpcAction.Jump;
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
