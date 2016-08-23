using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.GameObjects;
using Element.Common.GameObjects;
using Element.Common.GameObjects.Npcs;
using Element.Common.GameObjects.TileObjects;

namespace Element.Common.Environment.Tiles
{
    public abstract class Tile
    {
        protected StandardObject _standardObject;
        protected FloorObject _floorObject;
        protected Npc _npc;
        protected bool _reserved;
        protected Transition _transition;
        
        public abstract Tile Copy();

        // ****************** break here for game object stuff ****************

        /// <summary>
        /// Determines if the Npc/Standard object inside can move out of the tile.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns>true if yes, false if no</returns>
        public abstract bool CanMoveOff(Directions direction);
        public abstract bool? CanMoveOn(Directions direction);
        public abstract bool CanMoveOnTop(Directions direction);
        public abstract bool? CanDropInto(bool npc);
        public abstract bool CanDropOnTop(bool npc);
        public abstract bool? CanLandOn(Directions direction);
        public abstract bool CanLandOnTop(Directions direction);
        public abstract bool? CanPushAvailable(Directions direction, bool pulling);
        public abstract bool CanPushOut(Directions direction, bool pulling);
        public abstract bool? CanPushInto(Directions direction, bool pulling);
        public abstract bool CanPushOnTop(Directions direction, bool pulling);
        public abstract bool CanClimbOnTop(Directions direction);
        public abstract bool CanClimbOnBottom(Directions direction);
        public abstract bool CanMoveUpThrough();
        public abstract bool CanSlideDown(Directions direction); // this might be nullable
        public abstract bool CanFloatIn();

        // ********************************************************************

        public GameObject GetOccupyingObject()
        {
            if (_npc != null)
                return _npc;

            if (_standardObject != null)
                return _standardObject;

            return null;
        }

        public void ReserveTile()
        {
            _reserved = true;
        }

        public void OpenTile()
        {
            _reserved = false;
        }

        // going to need more methods here

        public bool CanNpcExecute(GameObjectActionType action, Directions direction)
        {
            if (_npc == null)
                return false;

            return _npc.CanExecute(action, direction);
        }

        public void NpcExecute(GameObjectActionType action, Directions direction)
        {
            if (_npc != null)
                _npc.Execute(action, direction);
        }

        public bool CanExecuteOnNpc(GameObjectActionType action, Directions direction)
        {
            if (_npc == null)
                return false;

            return _npc.CanExecuteOn(action, direction);
        }

        public void ExecuteOnNpc(GameObjectActionType action, Directions direction)
        {
            if (_npc != null)
                _npc.ExecuteOn(action, direction);
        }

        public bool CanStandardObjectExecute(GameObjectActionType action, Directions direction)
        {
            if (_standardObject == null)
                return false;

            return _standardObject.CanExecute(action, direction);
        }

        public void StandardObjectExecute(GameObjectActionType action, Directions direction)
        {
            if (_standardObject != null)
                _standardObject.Execute(action, direction);
        }

        public bool CanExecuteOnStandardObject(GameObjectActionType action, Directions direction)
        {
            if (_standardObject == null)
                return false;

            return _standardObject.CanExecuteOn(action, direction);
        }

        public void ExecuteOnStandardObject(GameObjectActionType action, Directions direction)
        {
            if (_standardObject != null)
                _standardObject.ExecuteOn(action, direction);
        }

        public bool CanFloorObjectExecute(GameObjectActionType action, Directions direction)
        {
            if (_floorObject == null)
                return false;

            return _floorObject.CanExecute(action, direction);
        }

        public void FloorObjectExecute(GameObjectActionType action, Directions direction)
        {
            if (_floorObject != null)
                _floorObject.Execute(action, direction);
        }

        public bool CanExecuteOnFloorObject(GameObjectActionType action, Directions direction)
        {
            if (_floorObject == null)
                return false;

            return _floorObject.CanExecuteOn(action, direction);
        }

        public void ExecuteOnFloorObject(GameObjectActionType action, Directions direction)
        {
            if (_floorObject != null)
                _floorObject.ExecuteOn(action, direction);
        }

        public ActionInFrontType GetActionInFront()
        {
            if (_npc != null)
                return _npc.GetInteractAction();

            if (_standardObject != null)
                return _standardObject.GetInteractAction();

            return ActionInFrontType.None;
        }

        public Transition Transition
        {
            get { return _transition; }
        }
    }
}
