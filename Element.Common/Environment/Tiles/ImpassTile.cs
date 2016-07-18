using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.NPCs;

namespace Element.Common.Environment.Tiles
{
    public class ImpassTile : Tile
    {
        public override bool? CanMoveOn(Directions direction)
        {
            return false;
        }

        public override bool CanMoveOnTop(Directions direction)
        {
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
            return NpcAction.None;
        }
    }
}
