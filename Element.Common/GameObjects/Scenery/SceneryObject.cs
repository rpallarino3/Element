using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Animations;

namespace Element.Common.GameObjects.Scenery
{
    public class SceneryObject : GameObject
    {
        private int _textureId;

        public SceneryObject(int textureId, Vector2 location, int level, Vector2 size) : base(location, level)
        {
            _textureId = textureId;

            var animations = new Dictionary<int, Animation>();
            animations[0] = new Animation(0, 1, 1, size);
            _animator = new Animator(animations, 0);
        }

        public SceneryObject(int textureId, Vector2 location, int level, Animator animator) : base(location, level)
        {
            _textureId = textureId;
            _animator = animator;
        }

        public int TextureId
        {
            get { return _textureId; }
        }
    }
}
