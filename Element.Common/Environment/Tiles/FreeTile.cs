using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.NPCs;
using Element.Common.Enumerations.TileObjects;
using Element.Common.GameObjects.TileObjects;

namespace Element.Common.Environment.Tiles
{
    public class FreeTile : Tile
    {
        private bool _pushable; // might want to put some sort of 'LockPush' field here as well
        private bool _lockPush;

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

            if (_floorObject != null)
                return _floorObject.CanExecuteOn(GameObjectActionType.DropOnto, Directions.Up); // direction is irrelevant

            return true;
        }

        public override bool CanDropOnTop(bool npc)
        {
            if (_standardObject == null)
                return false;

            return _standardObject.CanExecuteOn(GameObjectActionType.DropOnto, Directions.Up); // direction is irrelevant
        }

        public override bool CanFloatIn()
        {
            return false;
        }

        public override bool? CanLandOn(Directions direction)
        {
            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            if (_standardObject != null)
                return false;

            if (_floorObject != null)
                return _floorObject.CanExecuteOn(GameObjectActionType.LandOn, direction);

            return true;
        }

        public override bool CanLandOnTop(Directions direction)
        {
            if (_standardObject == null)
                return false;

            return _standardObject.CanExecuteOn(GameObjectActionType.LandOn, direction); // might want to change to LandOnTop
        }

        public override bool CanMoveOff(Directions direction)
        {
            // this is NOT used for when player is climbing on a standard object

            if (_floorObject != null)
                return _floorObject.CanExecuteOn(GameObjectActionType.WalkOff, direction);

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

            if (_floorObject != null)
                return _floorObject.CanExecuteOn(GameObjectActionType.WalkOn, direction);

            return true;
        }

        public override bool CanMoveOnTop(Directions direction)
        {
            if (_standardObject == null)
                return false;

            return _standardObject.CanExecuteOn(GameObjectActionType.WalkOnTop, direction);
        }

        public override bool CanMoveUpThrough()
        {
            return false;
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
            if (_reserved)
                return false;

            if (_npc != null && !pulling)
                return false;

            if (_standardObject != null)
                return false;

            if (_floorObject != null)
            {
                if (pulling)
                    return _floorObject.CanExecuteOn(GameObjectActionType.PullOn, direction);
                else
                    return _floorObject.CanExecuteOn(GameObjectActionType.PushOn, direction);
            }

            return _pushable; // might want to include lock push here somewhere
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
            if (_floorObject != null)
            {
                if (pulling)
                    return _floorObject.CanExecuteOn(GameObjectActionType.PullOff, direction);
                else
                    return _floorObject.CanExecuteOn(GameObjectActionType.PushOff, direction);
            }

            return !_lockPush;
        }

        public override bool CanSlideDown(Directions direction)
        {
            // not sure exactly what this entails. need to think about how we are using this
            if (_standardObject == null)
                return false;

            return _standardObject.CanExecuteOn(GameObjectActionType.Slide, direction); // this might not be right
        }
        
        public override Tile Copy()
        {
            throw new NotImplementedException();
        }
    }
}
