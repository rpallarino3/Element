using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.NPCs;
using Element.Common.Enumerations.TileObjects;
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

        // should return a copy of the tile minus the shit that's inside of it
        public abstract Tile Copy();
        
        // need to rethink all of these

        // we need to layout all the possible ways we would want an object or npc to enter/leave a tile
        // enter:
        // simply move into the floor level of the tile from another adjacent tile (need to know if we can move into the tile)
        // move on top of the tile (technically would be in the tile above but need to say if this is ok, need to know if we can only move on top, not drop in)
        // drop into the tile from above (need to know if it would be ok for an object to 'fall' into the tile)
        // be placed into the tile from another, faraway tile (maybe a transition or another object)
        // exit:
        // move from the tile by moving out (need to know if we can move out of the tile)
        // push an object into the tile (some tiles may not allow objects to be pushed into them in order to restrict where objects can go)
        // fall through the tile (to the tile below)
        // be removed from the tile

        public abstract NpcAction GetMoveActionFromTile(Directions direction);
        public abstract bool? CanMoveInto(Directions direction);
        public abstract bool CanMoveOnTop(Directions direction); // are we sure we don't want nullable here?
        public abstract bool? CanPushInto(Directions direction);
        public abstract bool? CanPushOnTop(Directions direction);
        public abstract bool? CanLandOn(bool pushed);
        
        public virtual bool CanMoveOffTile(Directions dir)
        {
            // standard objects should have priority over floor objects since they are 'on top'
            if (_standardObject != null) 
                return _standardObject.CanWalkOff(dir);

            if (_floorObject != null)
                return _floorObject.CanWalkOff(dir);

            return true;
        }

        public void ReserveTile()
        {
            _reserved = true;
        }

        public void OpenTile()
        {
            _reserved = false;
        }

        public bool CanStandardObjectExecute(TileObjectActions action, Directions direction)
        {
            if (_standardObject == null)
                return false;

            return _standardObject.CanExecute(action, direction);
        }

        public bool CanFloorObjectExecute(TileObjectActions action, Directions direction)
        {
            if (_floorObject == null)
                return false;

            return _floorObject.CanExecute(action, direction);
        }

        public void StandardObjectExecute(TileObjectActions action, Directions direction)
        {
            if (_standardObject != null)
                _standardObject.Execute(action, direction);
        }

        public void FloorObjectExecute(TileObjectActions action, Directions direction)
        {
            if (_floorObject != null)
                _floorObject.Execute(action, direction);
        }

        public Transition Transition
        {
            get { return _transition; }
        }
    }
}
