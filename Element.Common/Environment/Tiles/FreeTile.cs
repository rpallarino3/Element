using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.NPCs;
using Element.Common.Enumerations.TileObjects;

namespace Element.Common.Environment.Tiles
{
    public class FreeTile : Tile
    {
        public override bool? CanBeMovedInto(Directions direction)
        {
            if (_standardObject != null)
                return false;

            if (_npc != null)
                return false;

            if (_floorObject.CanExecute(TileObjectActions.WalkOn, direction))
                return true;
            else
                return false;
        }

        public override bool CanMoveOnTop(Directions direction)
        {
            if (_standardObject == null)
                return false;

            return _standardObject.CanWalkOn(direction); // is this the one i want?
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
