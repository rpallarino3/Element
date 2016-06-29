using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.NPCs;

namespace Element.Common.GameObjects.TileObjects
{
    public abstract class StandardObject : TileObject
    {
        protected ObjectNames _name;

        public StandardObject(Vector2 location, int level) : base(location, level)
        {

        }

        public virtual NpcAction GetNpcActionFromMove(Directions dir)
        {
            return NpcAction.None;
        }

        public ObjectNames Name
        {
            get { return _name; }
        }
    }
}
