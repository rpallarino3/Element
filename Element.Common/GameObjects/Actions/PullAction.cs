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
    public class PullAction : GameObjectAction
    {
        public PullAction() : base(GameObjectActionType.Pull)
        {

        }

        public override GameObjectActionType CanGameObjectExecute(GameObject gameObject, Directions direction, RegionNames region, int zone)
        {
            if (!gameObject.CanExecute(_type, direction))
                return GameObjectActionType.None;

            var currentTile = TrafficHandler.GetTile(region, zone, gameObject.Position, gameObject.Level);
            var pulledObjectTile = TrafficHandler.GetTileInDirection(gameObject.FacingDirection, region, zone, gameObject.Position, gameObject.Level);
            var pullingObjDestinationTile = TrafficHandler.GetTileInDirection(direction, region, zone, gameObject.Position, gameObject.Level);

            if (currentTile == null)
                return GameObjectActionType.TryWalk;

            if (pulledObjectTile == null)
                return GameObjectActionType.TryWalk;

            var canMoveOff = currentTile.CanMoveOff(direction); // this should be the direction we are pulling in
            var objToPull = pullingObjDestinationTile.CanPushAvailable(direction, true);

            if (!canMoveOff)
            {
                if (!objToPull.HasValue || objToPull.Value)
                    return GameObjectActionType.TryPull;

                return GameObjectActionType.TryWalk;
            }

            if (!objToPull.HasValue)
                return GameObjectActionType.TryPull;

            if (!objToPull.Value)
                return GameObjectActionType.TryWalk;

            // at this point we know there is something to pull

            var canMoveOnDestination = pullingObjDestinationTile.CanMoveOn(direction);

            if (canMoveOnDestination.HasValue && !canMoveOnDestination.Value)
            {
                if (objToPull.Value)
                    return GameObjectActionType.TryPull;

                return GameObjectActionType.TryWalk;
            }

            var canPullOut = pulledObjectTile.CanPushOut(direction, true);
            var canPullInto = currentTile.CanPushInto(direction, true);
            
            if (!canPullOut)
                return GameObjectActionType.TryPull;

            if (!canMoveOnDestination.HasValue)
            {
                var pullingObjDestinationTileBelow = TrafficHandler.GetTileBelow(direction, region, zone, gameObject.Position, gameObject.Level);

                if (pullingObjDestinationTileBelow == null)
                    return GameObjectActionType.TryPull;

                var canMoveOnTop = pullingObjDestinationTileBelow.CanMoveOnTop(direction);

                if (!canMoveOff)
                    return GameObjectActionType.TryPull;
            }

            // at this point we know that the pulling object is good to be moved into the tile behind them

            if (canPullInto.HasValue)
                return canPullInto.Value ? GameObjectActionType.Pull : GameObjectActionType.TryPull;

            var currentTileBelow = TrafficHandler.GetTileBelow(region, zone, gameObject.Position, gameObject.Level);

            if (currentTileBelow == null)
                return GameObjectActionType.TryPull;

            var canPullOnTop = currentTileBelow.CanPushOnTop(direction, true);

            if (canPullOnTop)
                return GameObjectActionType.Pull;

            return GameObjectActionType.TryPull;
        }
        
        public override void ExecuteOnGameOject(GameObject gameObject, Directions direction)
        {
            throw new NotImplementedException();
        }
    }
}
