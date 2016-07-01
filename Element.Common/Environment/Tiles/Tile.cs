using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.NPCs;
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

        // should return a copy of the tile minus the shit that's inside of it
        public abstract Tile Copy();
        public abstract NpcAction GetMoveActionFromTile(Directions direction);

        public virtual bool CanMoveOnTile(Directions dir)
        {
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

        public virtual bool CanMoveOffTile(Directions dir)
        {
            // standard objects should have priority over floor objects since they are 'on top'
            if (_standardObject != null) 
                return _standardObject.CanWalkOff(dir);

            if (_floorObject != null)
                return _floorObject.CanWalkOff(dir);

            return true;
        }
    }
}
