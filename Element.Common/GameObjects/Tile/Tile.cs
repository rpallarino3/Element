using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.GameBasics;
using Element.Common.GameObjects;

namespace Element.Common.GameObjects.Tile
{
    public abstract class Tile
    {
        // should return a copy of the tile minus the shit that's inside of it
        public abstract Tile Copy();

        public virtual bool CanMoveOnTile(Directions dir)
        {
            return true;
        }

        public virtual bool CanMoveOffTile(Directions dir)
        {
            return true;
        }
    }
}
