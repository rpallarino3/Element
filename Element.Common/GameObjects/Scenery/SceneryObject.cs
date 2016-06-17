using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Animations;
using Element.Common.Enumerations.Environment;

namespace Element.Common.GameObjects.Scenery
{
    public class SceneryObject : GameObject
    {
        private SceneryNames _name;
        private bool _onFloor;
                
        public SceneryObject(SceneryNames name, Vector2 location, int level, Animator animator, bool onFloor) : base(location, level)
        {
            _name = name;
            _animator = animator;
            _onFloor = onFloor;
        }

        public SceneryNames Name
        {
            get { return _name; }
        }

        public bool OnFloor
        {
            get { return _onFloor; }
        }
    }
}
