using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.NPCs;

namespace Element.Common.Environment.Tiles
{
    public class FreeTile : Tile
    {
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
