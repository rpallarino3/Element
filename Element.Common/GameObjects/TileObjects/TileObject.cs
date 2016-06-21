using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.Environment;

namespace Element.Common.GameObjects.TileObjects
{
    public class TileObject : GameObject
    {
        protected ObjectNames _name;

        public TileObject(Vector2 location, int level) : base(location, level)
        {

        }

        public ObjectNames Name
        {
            get { return _name; }
        }
    }
}
