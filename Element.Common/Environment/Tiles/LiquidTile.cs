using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.GameObjects;

namespace Element.Common.Environment.Tiles
{
    public class LiquidTile : Tile
    {
        private bool _floor;
        private bool _drained;

        public override bool CanClimbOnBottom(Directions direction)
        {
            if (_standardObject == null)
                return false;

            return _standardObject.CanExecuteOn(GameObjectActionType.ClimbOnBottom, direction);
        }

        public override bool CanClimbOnTop(Directions direction)
        {
            if (_standardObject == null)
                return false;

            return _standardObject.CanExecuteOn(GameObjectActionType.ClimbOnTop, direction);
        }

        public override bool? CanDropInto(bool npc)
        {
            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            if (_standardObject != null)
                return false;

            if (_floor)
            {
                if (npc)
                    return _drained;

                if (_floorObject != null)
                    return _floorObject.CanExecuteOn(GameObjectActionType.DropOnto, Directions.Up); // direction is irrelevant

                return true;
            }
            else
            {
                if (npc)
                    return _drained ? (bool?)null : false;

                return null;
            }
        }

        public override bool CanDropOnTop(bool npc)
        {
            // this is fine, if there is water above, that tile will catch npcs trying to drop into it
            if (_standardObject == null)
                return false;

            return _standardObject.CanExecuteOn(GameObjectActionType.DropOnto, Directions.Up); // direction is irrelevant
        }

        public override bool CanFloatIn()
        {
            return !_drained;
        }

        public override bool? CanLandOn(Directions direction)
        {
            // not exactly sure when you would encounter a situation where you would be jumping onto a non drained water tile
            // we are doing CanLandOn so we are assuming that you are jumping from a level tile to this tile.
            // not sure there would ever be a situation where something would be jumping onto a non drained tile (or even have that situation arise)
            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            if (_standardObject != null)
                return false;
            
            if (_floor)
            {
                if (_floorObject != null)
                    return _floorObject.CanExecuteOn(GameObjectActionType.LandOn, direction);

                if (_drained)
                    return true;
                else
                    return false;
            }
            else
            {
                if (_drained)
                    return null;
                else
                    return false;
            }
        }

        public override bool CanLandOnTop(Directions direction)
        {
            // if there is water above, that tile will can't npcs trying to jump into it
            if (_standardObject == null)
                return false;

            return _standardObject.CanExecuteOn(GameObjectActionType.LandOn, direction); // might want to change to LandOnTop
        }

        public override bool CanMoveOff(Directions direction)
        {
            if (_floor)
            {
                if (_floorObject != null)
                    return _floorObject.CanExecuteOn(GameObjectActionType.WalkOff, direction);
            }

            return true;
        }

        public override bool? CanMoveOn(Directions direction)
        {
            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            if (_standardObject != null)
                return false;

            if (_floor)
            {
                if (_floorObject != null)
                    return _floorObject.CanExecuteOn(GameObjectActionType.WalkOn, direction);

                // not sure when this would ever not be drained
                if (_drained)
                    return true;
                else
                    return false;
            }
            else
            {
                // again not sure when this would ever not be drained
                if (_drained)
                    return null;
                else
                    return false;
            }
        }

        public override bool CanMoveOnTop(Directions direction)
        {
            if (_standardObject == null)
                return false;

            return _standardObject.CanExecuteOn(GameObjectActionType.WalkOnTop, direction);
        }

        public override bool CanMoveUpThrough()
        {
            if (_floor)
                return false;

            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            if (_standardObject != null)
                return false;

            return true; // hrm not sure if we check drained here
        }

        public override bool? CanPushAvailable(Directions direction, bool pulling)
        {
            if (_standardObject == null)
                return false;

            if (pulling)
            {
                if (_standardObject.CanExecuteOn(GameObjectActionType.Pull, direction))
                    return true;
            }
            else
            {
                if (_standardObject.CanExecuteOn(GameObjectActionType.Push, direction))
                    return true;
            }

            return null;
        }

        public override bool? CanPushInto(Directions direction, bool pulling)
        {
            // tile should always have to be drained for this to happen
            if (_reserved)
                return false;

            if (_npc != null  && !pulling)
                return false;

            if (_standardObject != null)
                return false;

            if (_floor)
            {
                if (_floorObject != null)
                {
                    if (pulling)
                        return _floorObject.CanExecuteOn(GameObjectActionType.PullOn, direction);
                    else
                        return _floorObject.CanExecuteOn(GameObjectActionType.PushOn, direction);
                }

                return true;
            }
            else
                return null;
        }

        public override bool CanPushOnTop(Directions direction, bool pulling)
        {
            if (_standardObject == null)
                return false;

            if (pulling)
                return _standardObject.CanExecuteOn(GameObjectActionType.PullOnTop, direction);
            else
                return _standardObject.CanExecuteOn(GameObjectActionType.PushOnTop, direction);
        }

        public override bool CanPushOut(Directions direction, bool pulling)
        {
            if (_floor)
            {
                if (_floorObject != null)
                {
                    if (pulling)
                        return _floorObject.CanExecuteOn(GameObjectActionType.PullOff, direction);
                    else
                        return _floorObject.CanExecuteOn(GameObjectActionType.PushOff, direction);
                }
            }

            return true;
        }

        public override bool CanSlideDown(Directions direction)
        {
            if (_standardObject == null)
                return false;

            return _standardObject.CanExecuteOn(GameObjectActionType.Slide, direction);
        }

        public override Tile Copy()
        {
            throw new NotImplementedException();
        }
    }
}
