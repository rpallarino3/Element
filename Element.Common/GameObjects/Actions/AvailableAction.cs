using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.GameObjects;

namespace Element.Common.GameObjects.Actions
{
    public class AvailableAction : IComparable<AvailableAction>
    {
        private GameObjectActionType _actionType; // is this what we want or do we want a type?
        private Directions _direction;
        private int _priority;

        public AvailableAction(GameObjectActionType actionType, Directions direction, int priority)
        {
            _actionType = actionType;
            _direction = direction;
            _priority = priority;
        }

        public int CompareTo(AvailableAction other)
        {
            if (_priority < other.Priority)
                return -1;
            else if (_priority == other.Priority) // this would be strange
                return 0;
            else
                return 1;
        }

        public GameObjectActionType ActionType
        {
            get { return _actionType; }
        }

        public Directions Direction
        {
            get { return _direction; }
        }

        public int Priority
        {
            get { return _priority; }
        }
    }
}
