using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.TileObjects;

namespace Element.Common.GameObjects.TileObjects
{
    public abstract class TileObject : GameObject
    {
        protected ObjectNames _name;

        public TileObject(Vector2 location, int level) : base(location, level)
        {

        }

        public abstract void Execute(TileObjectActions action, Directions direction);
        public abstract bool CanExecute(TileObjectActions action, Directions dir);

        public ObjectNames Name
        {
            get { return _name; }
        }
    }
}
