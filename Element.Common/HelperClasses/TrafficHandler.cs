using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Environment;
using Element.Common.Environment.Tiles;
using Element.Common.GameObjects.Npcs;
using Element.Common.GameObjects.TileObjects;
using Microsoft.Xna.Framework;

namespace Element.Common.HelperClasses
{
    public static class TrafficHandler
    {
        private static Dictionary<RegionNames, Region> _regions;

        public static Tile GetTile(RegionNames region, int zone, Vector2 position, int level)
        {
            return _regions[region].Zones[zone].TileMap[(int)position.X, (int)position.Y, level];
        }

        public static Tile GetTileInDirection(Directions direction, RegionNames region, int zone, Vector2 position, int level)
        {
            return GetTileInDirection(direction, region, zone, position, level, 1);
        }

        public static Tile GetTileInDirection(Directions direction, RegionNames region, int zone, Vector2 position, int level, int distance)
        {
            return null;
        }

        public static Tile GetTileBelow(RegionNames region, int zone, Vector2 position, int level)
        {
            return GetTileBelow(Directions.Up, region, zone, position, level, 0);
        }

        public static Tile GetTileBelow(Directions direction, RegionNames region, int zone, Vector2 position, int level)
        {
            return GetTileBelow(direction, region, zone, position, level, 1);
        }

        public static Tile GetTileBelow(Directions direction, RegionNames region, int zone, Vector2 position, int level, int distance)
        {
            return null;
        }

        public static void ReserveTile(Npc npc, RegionNames region, int zone, Vector2 position, int level)
        {

        }

        public static void ReserveTile(TileObject tileObject, RegionNames region, int zone, Vector2 position, int level)
        {

        }

        public static void ReserveTilesInDirection(Npc npc, Directions direction, int distance)
        {
        }

        public static void ReserveTilesInDirection(TileObject tileObject, Directions direction, int distance)
        {

        }

        public static void ReserveTilesBelowInDirection(Npc npc, Directions direction)
        {

        }

        public static void ReserveTilesBelowInDirection(TileObject tileObject, Directions direction)
        {

        }

        public static void MoveNpcInDirection(Npc npc, Directions direction)
        {

        }

        public static void MoveObjectInDirection(TileObject tileObject, Directions direction)
        {

        }

        public static void SpawnObjectAtLocation(TileObject spawnObject, RegionNames region, int zone, Vector2 position, int level)
        {

        }

        public static Dictionary<RegionNames, Region> Regions
        {
            get { return _regions; }
            set { _regions = value; }
        }
    }
}
