using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.NPCs;
using Element.Common.Enumerations.TileObjects;
using Element.Common.GameObjects.TileObjects;

namespace Element.Common.Environment.Tiles
{
    public class FreeTile : Tile
    {
        private bool _pushable;

        public override bool? CanMoveOn(Directions direction)
        {
            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            if (_standardObject != null)
                return _standardObject.CanExecute(TileObjectActions.WalkOn, direction);

            if (_floorObject != null)
                return _floorObject.CanExecute(TileObjectActions.WalkOn, direction);

            return true;
        }

        public override bool CanMoveOnTop(Directions direction)
        {
            if (_standardObject != null)
                return _standardObject.CanExecute(TileObjectActions.WalkOnTop, direction);

            return false;
        }

        public override bool? CanPushInto(Directions direction)
        {
            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            if (_standardObject != null)
                return false;

            if (_floorObject != null)
                return _floorObject.CanExecute(TileObjectActions.PushOnTop, direction);

            return _pushable;
        }

        public override bool? CanPushOnTop(Directions direction)
        {
            if (_standardObject != null)
                return _standardObject.CanExecute(TileObjectActions.PushOnTop, direction); // i think this is the action we want

            return false;
        }

        public override bool? CanPushOver(Directions direction)
        {
            // i think if the tile is reserved by anything we don't want to push into
            // we could do more investigation as to what the reserving object is
            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            if (_standardObject != null)
                return _standardObject.CanExecute(TileObjectActions.PushOnTop, direction);

            if (_floorObject != null)
                return _floorObject.CanExecute(TileObjectActions.PushOnTop, direction);

            return _pushable;
        }

        public override bool CanMoveVerticallyThrough()
        {
            return false;
        }

        public override bool CanLandOnTop(Directions direction)
        {
            if (_standardObject != null)
                return _standardObject.CanExecute(TileObjectActions.LandOnTop, direction);

            return false;
        }

        public override bool? CanLandOn(Directions direction)
        {
            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            // i think if there is a standard object inside we don't allow jumping into
            if (_standardObject != null)
                return false;

            if (_floorObject != null)
                return _floorObject.CanExecute(TileObjectActions.LandOnTop, direction);

            return true;
        }

        public override bool? CanBePlacedIn()
        {
            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            if (_standardObject != null)
                return false;

            if (_floorObject != null) // i don't know what the difference is between walk on top and walk on for floor objects
                return _floorObject.CanExecute(TileObjectActions.FallOnTop, Directions.Up);
            return true;
        }

        public override bool? CanDropInto()
        {
            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            if (_standardObject != null) // might need to come up with new actions for this stuff
                return _standardObject.CanExecute(TileObjectActions.FallOnTop, Directions.Up);

            if (_floorObject != null)
                return _floorObject.CanExecute(TileObjectActions.FallOnTop, Directions.Up);

            return true;
        }

        public override Tile Copy()
        {
            throw new NotImplementedException();
        }

        public override NpcAction GetMoveActionFromTile(Directions direction)
        {
            // i don't know if this is necessarily true
            if (_reserved)
                return NpcAction.None;

            if (_standardObject != null)
                return _standardObject.GetNpcActionFromMove(direction);

            if (_floorObject != null)
            {
                if (_floorObject.CanWalkOn(direction))
                    return NpcAction.Walk;
                else if (_floorObject.CanJumpOver(direction))
                    return NpcAction.Jump;
            }

            return NpcAction.Walk;
        }
    }
}
