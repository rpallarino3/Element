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
    public class WalkAction : GameObjectAction
    {
        public WalkAction() : base(GameObjectActionType.Walk)
        {

        }

        public override GameObjectActionType CanGameObjectExecute(GameObject gameObject, Directions direction, RegionNames region, int zone)
        {
            if (!gameObject.CanExecute(_type, direction))
                return GameObjectActionType.None;
                        
            var currentTile = TrafficHandler.GetTile(region, zone, gameObject.Position, gameObject.Level);

            if (currentTile == null) // something is wrong if this is true
                return GameObjectActionType.TryWalk;

            var canMoveOff = currentTile.CanMoveOff(direction);

            if (!canMoveOff)
                return GameObjectActionType.TryWalk;

            var destinationTile = TrafficHandler.GetTileInDirection(direction, region, zone, gameObject.Position, gameObject.Level);

            if (destinationTile == null)
                return GameObjectActionType.TryWalk;

            var canMoveOn = destinationTile.CanMoveOn(direction);

            if (canMoveOn.HasValue && canMoveOn.Value)
                return GameObjectActionType.Walk;

            if (canMoveOn.HasValue && !canMoveOn.Value)
            {
                // this is where we need to check climb
                if (!gameObject.CanExecute(GameObjectActionType.ClimbOnBottom, direction))
                    return GameObjectActionType.TryWalk;

                var canClimbOnBottom = destinationTile.CanClimbOnBottom(direction);

                // this might not be correct, is the object guaranteed to not be climbing?
                // i think it would be, as you would not be able to execute the walk if you were in climb (first line in method)
                return canClimbOnBottom ? GameObjectActionType.ClimbOn : GameObjectActionType.TryWalk; 
            }

            // otherwise we check below and stuff for slide, etc.
            var destinationTileBelow = TrafficHandler.GetTileBelow(direction, region, zone, gameObject.Position, gameObject.Level);

            if (destinationTileBelow == null)
                return GameObjectActionType.TryWalk;

            var canWalkOnTop = destinationTileBelow.CanMoveOnTop(direction);

            if (canWalkOnTop)
                return GameObjectActionType.Walk;

            // at this point we need to check for slide
            if (!gameObject.CanExecute(GameObjectActionType.Slide, direction))
                return GameObjectActionType.TryWalk;

            var currentTileBelow = TrafficHandler.GetTileBelow(region, zone, gameObject.Position, gameObject.Level);

            if (currentTileBelow == null)
                return GameObjectActionType.TryWalk;

            var canSlideDown = currentTileBelow.CanSlideDown(direction);

            if (!canSlideDown)
                return GameObjectActionType.TryWalk;

            // is this where we need to check all the way down?
            var canFallDown = TrafficHandler.CanObjectBeOver(direction, region, zone, gameObject.Position, gameObject.Level, gameObject);

            if (canFallDown)
                return GameObjectActionType.StartSlide;

            return GameObjectActionType.TryWalk;
        }

        public override void ExecuteOnGameOject(GameObject gameObject, Directions direction)
        {
            throw new NotImplementedException();
        }
    }
}
