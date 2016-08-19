using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.GameObjects;

namespace Element.Common.GameObjects.Actions
{
    public abstract class GameObjectAction
    {
        protected GameObjectActionType _type;

        public GameObjectAction(GameObjectActionType type)
        {
            _type = type;
        }

        public abstract GameObjectActionType CanGameObjectExecute(GameObject gameObject, Directions direction, RegionNames region, int zone);
        public abstract void ExecuteOnGameOject(GameObject gameObject, Directions direction); // not sure we need this
    }
}
