using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.GameObjects;

namespace Element.Common.Environment.Tiles
{
    public class GapTile : Tile
    {
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

            if (_transition != null) // need to come back into here and revise this
                return true;

            return null;
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

            return null;
        }

        public override bool CanLandOnTop(Directions direction)
        {
            if (_standardObject == null)
                return false;

            return _standardObject.CanExecuteOn(GameObjectActionType.LandOn, direction);
        }

        public override bool CanMoveOff(Directions direction)
        {
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

            return null;
        }

        public override bool CanMoveOnTop(Directions direction)
        {
            if (_standardObject == null)
                return false;

            return _standardObject.CanExecuteOn(GameObjectActionType.WalkOnTop, direction);
        }

        public override bool CanMoveUpThrough()
        {
            if (_reserved)
                return false;

            if (_npc != null)
                return false;

            if (_standardObject != null)
                return false;

            return true;
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

            // i guess we assume that the pulling npc is in this tile?
            if (_npc != null && !pulling)
                return false;

            if (_standardObject != null)
                return false;

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
            return true;
        }

        // i think this will generally be used from above looking down at this tile
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
