using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.GameObjects.Npcs;
using Element.Common.GameObjects.Scenery;
using Element.Common.GameObjects.Tile;
using Element.Common.GameObjects.TileObjects;

namespace Element.Common.Environment
{
    public class Zone
    {
        private Vector2 _size;
        private int _levels;
        private Tile[, ,] _tileMap;
        private List<Npc> _npcs;
        private List<TileObject> _tileObjects;
        private List<SceneryObject> _sceneryObjects;
        private List<Rectangle> _cameraCollisionBoxes;

        public Zone(Vector2 size, int levels)
        {
            _size = size;
            _levels = levels;
            _tileMap = new Tile[(int)size.X, (int)size.Y, levels];
            _npcs = new List<Npc>();
            _tileObjects = new List<TileObject>();
            _sceneryObjects = new List<SceneryObject>();
            _cameraCollisionBoxes = new List<Rectangle>();
        }

        public void AddTileSection(int x, int y, int width, int height, int level, Tile tile)
        {
            for (int i = x; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    _tileMap[i, j, level] = tile.Copy();
                }
            }
        }
        
        public Tile[, ,] TileMap
        {
            get { return _tileMap; }
        }

        public List<Npc> Npcs
        {
            get { return _npcs; }
        }

        public List<TileObject> TileObjects
        {
            get { return _tileObjects; }
        }

        public List<SceneryObject> SceneryObjects
        {
            get { return _sceneryObjects; }
        }

        public List<Rectangle> CameraCollisionBoxes
        {
            get { return _cameraCollisionBoxes; }
        }

        public Vector2 Size
        {
            get { return _size; }
        }

        public int Levels
        {
            get { return _levels; }
        }
    }
}
