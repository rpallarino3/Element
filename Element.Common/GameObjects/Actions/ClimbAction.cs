using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.GameObjects;
using Element.Common.GameObjects.Npcs;
using Element.Common.HelperClasses;

namespace Element.Common.GameObjects.Actions
{
    public class ClimbAction : GameObjectAction
    {
        public ClimbAction() : base(GameObjectActionType.Climb) // eh this type doesn't exactly matter here
        {

        }

        // i think we have to assume that if they could climb on they could at some point be able to execute a bunch of different kind of climbs
        public override GameObjectActionType CanGameObjectExecute(GameObject gameObject, Directions direction, RegionNames region, int zone)
        {
            // so the object has been able to climb on? so we can assume they have the ability to do all the other climbing variants
            if (!gameObject.CanExecute(GameObjectActionType.Climb, direction))
                return GameObjectActionType.TryClimb;

            var climbIndex = gameObject.ClimbIndex;
            var currentTile = TrafficHandler.GetTile(region, zone, gameObject.Position, gameObject.Level);
            var climbTile = TrafficHandler.GetTileInDirection(gameObject.FacingDirection, region, zone, gameObject.Position, gameObject.Level);

            if (currentTile == null || climbTile == null) // this would be strange
                return GameObjectActionType.TryClimb; // i think this is what we want

            if (direction == Directions.Up)
            {
                if (climbIndex == ClimbIndex.Bottom)
                {
                    var canClimbOnTop = climbTile.CanClimbOnTop(direction);
                    return canClimbOnTop ? GameObjectActionType.Climb : GameObjectActionType.TryClimb;
                }
                else if (climbIndex == ClimbIndex.Top)
                {
                    var tileAboveClimber = TrafficHandler.GetTileAbove(region, zone, gameObject.Position, gameObject.Level);
                    var tileAboveClimb = TrafficHandler.GetTileAbove(gameObject.FacingDirection, region, zone, gameObject.Position, gameObject.Level);

                    if (tileAboveClimber == null || tileAboveClimb == null)
                        return GameObjectActionType.TryClimb;

                    var canClimbIntoAbove = tileAboveClimber.CanMoveUpThrough();

                    if (!canClimbIntoAbove)
                        return GameObjectActionType.TryClimb;

                    var canClimbOnBottomAbove = tileAboveClimb.CanClimbOnBottom(direction);

                    if (canClimbOnBottomAbove)
                        return GameObjectActionType.ClimbOnBottom;

                    var canMoveInAboveClimb = tileAboveClimb.CanMoveOn(direction);

                    if (canMoveInAboveClimb.HasValue || canMoveInAboveClimb.Value)
                    {
                        if (!gameObject.CanExecute(GameObjectActionType.ClimbOff, direction))
                            return GameObjectActionType.TryClimb;

                        return GameObjectActionType.ClimbOff;
                    }

                    return GameObjectActionType.TryClimb;
                }
            }
            else if (direction == Directions.Down)
            {
                if (climbIndex == ClimbIndex.Bottom)
                {
                    // remeber to check if climb off as well as if climb down
                    var tileBelowClimber = TrafficHandler.GetTileBelow(region, zone, gameObject.Position, gameObject.Level);
                    var tileBelowClimb = TrafficHandler.GetTileBelow(gameObject.FacingDirection, region, zone, gameObject.Position, gameObject.Level);

                    if (tileBelowClimber == null || tileBelowClimb == null)
                        return GameObjectActionType.TryClimb;

                    var canDropInto = currentTile.CanDropInto(gameObject is Npc);

                    if (canDropInto.HasValue)
                    {
                        if (canDropInto.Value)
                        {
                            if (!gameObject.CanExecute(GameObjectActionType.ClimbOff, direction))
                                return GameObjectActionType.TryClimb;

                            return GameObjectActionType.ClimbOff;
                        }

                        return GameObjectActionType.TryClimb; // this would be a strange scenario I think?
                    }

                    var canDropOnTop = tileBelowClimber.CanDropOnTop(gameObject is Npc);

                    if (canDropOnTop)
                    {
                        if (!gameObject.CanExecute(GameObjectActionType.ClimbOff, direction))
                            return GameObjectActionType.TryClimb;

                        return GameObjectActionType.ClimbOff;
                    }

                    // now we need to check if the tile below is empty
                    var canDropIntoBelow = tileBelowClimber.CanDropInto(gameObject is Npc);

                    if (canDropIntoBelow.HasValue && !canDropIntoBelow.Value)
                        return GameObjectActionType.TryClimb;

                    var canClimbDown = tileBelowClimb.CanClimbOnTop(direction);

                    return canClimbDown ? GameObjectActionType.Climb : GameObjectActionType.TryClimb;
                }
                else if (climbIndex == ClimbIndex.Top)
                {
                    var canClimbOnBottom = climbTile.CanClimbOnBottom(direction);

                    return canClimbOnBottom ? GameObjectActionType.Climb : GameObjectActionType.TryClimb;
                }
            }
            else // climbing left or right
            {
                var adjacentClimberTile = TrafficHandler.GetTileInDirection(direction, region, zone, gameObject.Position, gameObject.Level);
                var adjacentClimbTile = TrafficHandler.GetTileInDirection(direction, region, zone, gameObject.Position + new Microsoft.Xna.Framework.Vector2(0, -1), gameObject.Level);

                if (adjacentClimberTile == null || adjacentClimbTile == null)
                    return GameObjectActionType.TryClimb;

                var canMoveIntoAdjacent = adjacentClimberTile.CanDropInto(gameObject is Npc); // not sure if this is right

                if (canMoveIntoAdjacent.HasValue && !canMoveIntoAdjacent.Value)
                    return GameObjectActionType.TryClimb;

                if (climbIndex == ClimbIndex.Bottom)
                {
                    var canClimbOnBottom = adjacentClimbTile.CanClimbOnBottom(direction);
                    return canClimbOnBottom ? GameObjectActionType.Climb : GameObjectActionType.TryClimb;
                }
                else if (climbIndex == ClimbIndex.Top)
                {
                    var canClimbOnTop = adjacentClimbTile.CanClimbOnTop(direction);
                    return canClimbOnTop ? GameObjectActionType.Climb : GameObjectActionType.TryClimb;
                }
            }

            return GameObjectActionType.TryClimb;
        }

        public override void ExecuteOnGameOject(GameObject gameObject, Directions direction)
        {
            throw new NotImplementedException();
        }
    }
}
