using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.GameBasics;
using Element.Common.GameObjects;

namespace Element.Common.GameObjects.Tile
{
    public abstract class Tile : GameObject
    {
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
