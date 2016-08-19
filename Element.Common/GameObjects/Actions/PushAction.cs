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
    public class PushAction : GameObjectAction
    {
        public PushAction() : base(GameObjectActionType.Push)
        {
        }

        public override GameObjectActionType CanGameObjectExecute(GameObject gameObject, Directions direction, RegionNames region, int zone)
        {
            // i guess we assume that all objects that can push can also walk?
            if (!gameObject.CanExecute(_type, direction))
                return GameObjectActionType.None;

            var currentTile = TrafficHandler.GetTile(region, zone, gameObject.Position, gameObject.Level);            
            var destinationTile = TrafficHandler.GetTileInDirection(direction, region, zone, gameObject.Position, gameObject.Level);

            if (currentTile == null)
                return GameObjectActionType.TryWalk; // what exactly do we want to return here? we could return none, but i think this should be fine

            if (destinationTile == null)
                return GameObjectActionType.TryWalk;

            var canMoveOff = currentTile.CanMoveOff(direction);
            var objToPush = destinationTile.CanPushAvailable(direction, false);
            
            if (!canMoveOff)
            {
                if (!objToPush.HasValue || objToPush.Value)
                    return GameObjectActionType.TryPush;

                return GameObjectActionType.TryWalk;
            }

            if (!objToPush.HasValue) // this means that there is an object there that we can't push
                return GameObjectActionType.TryPush;

            if (!objToPush.Value)
                return GameObjectActionType.TryWalk;

            // at this point we know there is an object to push

            var canPushOut = destinationTile.CanPushOut(direction, false);

            if (!canPushOut)
                return GameObjectActionType.TryPush;

            // at this point we know there is an object to push and that we can push it out and move out of current tiles
            var landingTile = TrafficHandler.GetTileInDirection(direction, region, zone, gameObject.Position, gameObject.Level, 2);

            if (landingTile == null)
                return GameObjectActionType.TryPush;

            var canPushInto = landingTile.CanPushInto(direction, false);

            if (canPushInto.HasValue)
                return canPushInto.Value ? GameObjectActionType.Push : GameObjectActionType.TryPush;

            var landingTileBelow = TrafficHandler.GetTileBelow(direction, region, zone, gameObject.Position, gameObject.Level, 2);

            if (landingTileBelow == null)
                return GameObjectActionType.TryPush;

            var canPushOnTop = landingTileBelow.CanPushOnTop(direction, false);

            if (canPushOnTop)
                return GameObjectActionType.Push;

            // now we need to check all the way down to see if we can push over
            // we know there is an object in the tile already
            var occupyingObj = destinationTile.GetOccupyingObject();
            var canPushOver = TrafficHandler.CanObjectBeOver(direction, region, zone, occupyingObj.Position, occupyingObj.Level, occupyingObj, 2);

            if (canPushOver)
                return GameObjectActionType.Push;

            return GameObjectActionType.TryPush;
        }

        public override void ExecuteOnGameOject(GameObject gameObject, Directions direction)
        {
            throw new NotImplementedException();
        }
    }
}
