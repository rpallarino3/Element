using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Animations;

namespace Element.Common.GameObjects
{
    public abstract class GameObject
    {
        protected Animator _animator;
        protected Vector2 _location;
        protected Vector2 _tileLocation;
        protected int _level;
        protected bool _locked;
        protected int _id;

        public GameObject(Vector2 location, int level)
        {
            _location = location;
            _level = level;
        }

        public Animator Animator
        {
            get { return _animator; }
        }

        public Vector2 Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public Vector2 TileLocation
        {
            get { return _tileLocation; }
            set { _tileLocation = value; }
        }

        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }
        
        public bool Locked
        {
            get { return _locked; }
            set { _locked = value; }
        }
    }
}
