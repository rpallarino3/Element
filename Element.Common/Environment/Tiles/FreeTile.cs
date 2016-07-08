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

        public override bool? CanMoveInto(Directions direction)
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
            if (_standardObject == null)
                return false;

            return _standardObject.CanExecute(TileObjectActions.WalkOnTop, direction);
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
            if (_standardObject == null)
                return null;

            return _standardObject.CanExecute(TileObjectActions.PushOnTop, direction);
        }

        public override bool? CanLandOn(bool pushed)
        {
            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            if (_standardObject != null)
                return _standardObject.CanExecute(TileObjectActions.LandOnTop, Directions.Up); // direction doesn't matter here

            if (_floorObject != null)
                return _floorObject.CanExecute(TileObjectActions.LandOnTop, Directions.Up);

            if (pushed)
                return _pushable;
            else
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
