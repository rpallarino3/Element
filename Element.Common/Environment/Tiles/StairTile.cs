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
        public override bool? CanMoveOn(Directions direction)
        {
            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            return true;
        }

        public override bool CanMoveOnTop(Directions direction)
        {
            // not exactly sure how this is supposed to work
            return false;
        }

        public override bool? CanPushInto(Directions direction)
        {
            return false;
        }

        public override bool? CanPushOnTop(Directions direction)
        {
            return false;
        }

        public override bool? CanPushOver()
        {
            return false;
        }

        public override bool CanMoveVerticallyThrough()
        {
            return false;
        }

        public override bool CanLandOnTop(Directions direction)
        {
            return false;
        }

        public override bool? CanLandOn(Directions direction)
        {
            return false;
        }

        public override bool? CanBePlacedIn()
        {
            return false;
        }

        public override bool? CanDropInto()
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
