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
    public class JumpAction : GameObjectAction
    {
        public JumpAction() : base(GameObjectActionType.Jump)
        {

        }

        public override GameObjectActionType CanGameObjectExecute(GameObject gameObject, Directions direction, RegionNames region, int zone)
        {
            if (!gameObject.CanExecute(_type, direction))
                return GameObjectActionType.None;

            var currentTile = TrafficHandler.GetTile(region, zone, gameObject.Position, gameObject.Level);

            if (currentTile == null) // something is strange here
                return GameObjectActionType.TryJump;

            var canMoveOff = currentTile.CanMoveOff(direction);

            if (!canMoveOff)
                return GameObjectActionType.TryJump;

            var destinationTile = TrafficHandler.GetTileInDirection(direction, region, zone, gameObject.Position, gameObject.Level);

            if (destinationTile == null)
                return GameObjectActionType.TryJump;

            var canMoveOn = destinationTile.CanMoveOn(direction);

            if (canMoveOn.HasValue)
                return GameObjectActionType.TryJump;

            var destinationTileBelow = TrafficHandler.GetTileBelow(direction, region, zone, gameObject.Position, gameObject.Level);

            if (destinationTileBelow != null)
            {
                // i think here we only need to check if the tile below is empty?
                // or do we need to check for climb and shit as well?
                // i think it should be ok to not check for climb and slide
                var canWalkOnTop = destinationTileBelow.CanMoveOnTop(direction);

                if (canWalkOnTop)
                    return GameObjectActionType.TryJump;
            }

            // at this point we know we are good to check the landing tile for a jump

            var landingTile = TrafficHandler.GetTileInDirection(direction, region, zone, gameObject.Position, gameObject.Level, 2);

            if (landingTile == null)
                return GameObjectActionType.TryJump;

            var canLandOn = landingTile.CanLandOn(direction);

            if (canLandOn.HasValue)
                return canLandOn.Value ? GameObjectActionType.Jump : GameObjectActionType.TryJump;

            var landingTileBelow = TrafficHandler.GetTileBelow(direction, region, zone, gameObject.Position, gameObject.Level, 2);

            if (landingTileBelow == null)
                return GameObjectActionType.TryJump;

            var canLandOnTop = landingTileBelow.CanLandOnTop(direction);

            return canLandOnTop ? GameObjectActionType.Jump : GameObjectActionType.TryJump;
        }

        public override void ExecuteOnGameOject(GameObject gameObject, Directions direction)
        {
            throw new NotImplementedException();
        }
    }
}
