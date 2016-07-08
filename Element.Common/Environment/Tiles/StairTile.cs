using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.NPCs;
using Element.Common.Enumerations.TileObjects;

namespace Element.Common.Environment.Tiles
{
    public class StairTile : Tile
    {
        public override bool? CanMoveInto(Directions direction)
        {
            if (_reserved)
                return false;
            if (_npc != null)
                return false;

            return true;
        }

        public override bool CanMoveOnTop(Directions direction)
        {
            return false; // i think this is right
        }

        public override bool? CanPushInto(Directions direction)
        {
            return false;
        }

        public override bool? CanPushOnTop(Directions direction)
        {
            return false;
        }

        public override bool? CanLandOn(bool pushed)
        {
            return false;
        }

        public override Tile Copy()
        {
            throw new NotImplementedException();
        }

        public override NpcAction GetMoveActionFromTile(Directions direction)
        {
            throw new NotImplementedException();
        }
    }
}
