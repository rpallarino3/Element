using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.GameObjects;

namespace Element.Common.GameObjects.Actions
{
    public class CastAction : GameObjectAction
    {
        public CastAction() : base(GameObjectActionType.Cast)
        {

        }

        public override GameObjectActionType CanGameObjectExecute(GameObject gameObject, Directions direction, RegionNames region, int zone)
        {
            if (!gameObject.CanExecute(GameObjectActionType.Cast, direction))
                return GameObjectActionType.None;

            return GameObjectActionType.Cast;
        }

        public override void ExecuteOnGameOject(GameObject gameObject, Directions direction)
        {
            throw new NotImplementedException();
        }
    }
}
