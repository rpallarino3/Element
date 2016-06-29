using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;

namespace Element.Common.GameObjects.TileObjects
{
    public abstract class FloorObject : TileObject
    {
        protected ObjectNames _name;

        public FloorObject(Vector2 location, int level) : base(location, level)
        {

        }

        // generally can't walk on if you can jump over
        public virtual bool CanJumpOver(Directions dir)
        {
            return true;
        }

        public ObjectNames Name
        {
            get { return _name; }
        }
    }
}
