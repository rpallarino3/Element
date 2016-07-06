﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.NPCs;

namespace Element.Common.Environment.Tiles
{
    public class ImpassTile : Tile
    {
        public override bool? CanBeMovedInto(Directions direction)
        {
            return false;
        }

        public override bool CanMoveOnTop(Directions direction)
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
