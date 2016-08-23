using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.GameObjects;
using Element.Common.HelperClasses;

namespace Element.Common.GameObjects.Actions
{
    public class GrabAction : GameObjectAction
    {
        public GrabAction() : base(GameObjectActionType.Grab)
        {

        }

        public override GameObjectActionType CanGameObjectExecute(GameObject gameObject, Directions direction, RegionNames region, int zone)
        {
            if (!gameObject.CanExecute(GameObjectActionType.Grab, direction))
                return GameObjectActionType.None;

            var destinationTile = TrafficHandler.GetTileInDirection(direction, region, zone, gameObject.Position, gameObject.Level);

            if (destinationTile == null)
                return GameObjectActionType.None;

            var canExecuteGrab = destinationTile.CanExecuteOnStandardObject(GameObjectActionType.Grab, direction);

            return canExecuteGrab ? GameObjectActionType.Grab : GameObjectActionType.None;
        }

        public override void ExecuteOnGameOject(GameObject gameObject, Directions direction)
        {
            throw new NotImplementedException();
        }
    }
}
