﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;

namespace Element.Common.GameObjects.TileObjects
{
    public abstract class TileObject : GameObject
    {
        protected ObjectNames _name;

        public TileObject(Vector2 location, int level) : base(location, level)
        {

        }

        public virtual bool CanWalkOn(Directions dir)
        {
            return true;
        }

        public virtual bool CanWalkOff(Directions dir)
        {
            return true;
        }

        public ObjectNames Name
        {
            get { return _name; }
        }
    }
}
