using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.NPCs;

namespace Element.Common.Environment.Tiles
{
    public class GapTile : Tile
    {
        public override bool? CanBeMovedInto(Directions direction)
        {
            if (_standardObject != null)
                return false;

            if (_npc != null)
                return false;

            return null;
        }

        public override bool CanMoveOnTop(Directions direction)
        {
            if (_standardObject == null)
                return false;

            return _standardObject.CanWalkOn(direction);
        }

        public override Tile Copy()
        {
            throw new NotImplementedException();
        }

        public override NpcAction GetMoveActionFromTile(Directions direction)
        {
            if (_reserved)
                return NpcAction.None;

            if (_standardObject != null)
                return _standardObject.GetNpcActionFromMove(direction);

            return NpcAction.Jump;
        }
    }
}
