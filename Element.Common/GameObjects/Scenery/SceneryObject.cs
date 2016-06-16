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

        public SceneryObject(SceneryNames name, Vector2 location, int level, Vector2 size) : base(location, level)
        {
            _name = name;

            var animations = new Dictionary<int, Animation>();
            animations[0] = new Animation(0, 1, 1, size);
            _animator = new Animator(animations, 0);
        }

        public SceneryObject(SceneryNames name, Vector2 location, int level, Animator animator) : base(location, level)
        {
            _name = name;
            _animator = animator;
        }

        public SceneryNames Name
        {
            get { return _name; }
        }
    }
}
