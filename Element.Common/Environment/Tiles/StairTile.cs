using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.GameBasics;

namespace Element.Common.Environment.Tiles
{
    public class StairTile : Tile
    {
        // stair tiles come in pairs of 2
        // stairs are on both levels (1 stair tile on each level)
        // the top stairs on the bottom level is horizontal blocked
        // the bottom stairs on the top level is horizontal blocked
        // the top stair will change objects level based on directon, same with the bottom
        private bool _topStair;
        private bool _horizontalBlocked;

        public override bool CanClimbOnBottom(Directions direction)
        {
            return false;
        }

        public override bool CanClimbOnTop(Directions direction)
        {
            return false;
        }

        public override bool? CanDropInto(bool npc)
        {
            return false;
        }

        public override bool CanDropOnTop(bool npc)
        {
            return false;
        }

        public override bool CanFloatIn()
        {
            return false;
        }

        public override bool? CanLandOn(Directions direction)
        {
            return false;
        }

        public override bool CanLandOnTop(Directions direction)
        {
            return false;
        }

        public override bool CanMoveOff(Directions direction)
        {
            if (_horizontalBlocked)
            {
                if (direction == Directions.Left || direction == Directions.Right)
                    return false;
            }

            return true;
        }

        public override bool? CanMoveOn(Directions direction)
        {
            if (_horizontalBlocked)
            {
                if (direction == Directions.Left || direction == Directions.Right)
                    return false;
            }

            return true;
        }

        public override bool CanMoveOnTop(Directions direction)
        {
            return false;
        }

        public override bool CanMoveUpThrough()
        {
            return false;
        }

        public override bool? CanPushAvailable(Directions direction, bool pulling)
        {
            return false;
        }

        public override bool? CanPushInto(Directions direction, bool pulling)
        {
            return false;
        }

        public override bool CanPushOnTop(Directions direction, bool pulling)
        {
            return false;
        }

        public override bool CanPushOut(Directions direction, bool pulling)
        {
            return false;
        }

        public override bool CanSlideDown(Directions direction)
        {
            return false;
        }

        public override Tile Copy()
        {
            throw new NotImplementedException();
        }        
    }
}
