using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Animations;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.GameObjects;

namespace Element.Common.GameObjects
{
    public abstract class GameObject
    {
        protected Animator _animator;
        protected Vector2 _position;
        protected Vector2 _tileLocation;
        protected int _level;
        protected bool _locked;
        protected int _id;
        protected RegionNames _region;
        protected int _zone;
        protected Directions _facingDirection;
        protected ClimbIndex _climbIndex;

        public GameObject(Vector2 position, int level)
        {
            _position = position;
            _level = level;
        }

        // maybe make some of these virtual
        public virtual bool CanExecute(GameObjectActionType action, Directions direction)
        {
            return false;
        }

        public virtual bool CanExecuteOn(GameObjectActionType action, Directions direction)
        {
            return false;
        }

        public virtual void Execute(GameObjectActionType action, Directions direction) { }
        public virtual void ExecuteOn(GameObjectActionType action, Directions direction) { }

        public Animator Animator
        {
            get { return _animator; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
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

        public RegionNames Region
        {
            get { return _region; }
        }

        public int Zone
        {
            get { return _zone; }
        }

        public Directions FacingDirection
        {
            get { return _facingDirection; }
        }

        public ClimbIndex ClimbIndex
        {
            get { return _climbIndex; }
        }
    }
}
