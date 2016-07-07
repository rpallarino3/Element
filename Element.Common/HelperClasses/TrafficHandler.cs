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

        #region Push

        public static bool CheckDownForPush(Directions direction, RegionNames region, int zone, Vector2 position, int level, int distance)
        {
            var tile = GetTileInDirection(direction, region, zone, position, level, distance);

            if (tile == null)
                return false;

            var canPush = tile.CanPushInto(direction);
            
            if (canPush.HasValue)
                return canPush.Value;
            
            // we already know that the tile is in bounds because we got it
            var offset = GetOffsetFromDirection(direction, distance);
            
            return CheckPushThenFallAllTheWayDown(direction, region, zone, position + offset, level);
        }

        #endregion

        // this one would imply we are falling
        public static bool CheckFallAllTheWayDown(RegionNames region, int zone, Vector2 position, int level)
        {
            var tileMap = _regions[region].Zones[zone].TileMap;
            for (int i = level; i >= 0; i--)
            {
                var tile = tileMap[(int)position.X, (int)position.Y + 2 * (level - i), i];

                if (tile == null) // might be double checking here but should do in case we call this method from something else
                    return false;

                var canLand = tile.CanLandOn();

                if (canLand.HasValue)
                    return canLand.Value;
            }

            return false;
        }

        // eh this isn't exactly right, the top tile is slightly different?
        // at this point we have already checked the top tile to make sure we can push into
        public static bool CheckPushThenFallAllTheWayDown(Directions direction, RegionNames region, int zone, Vector2 position, int level)
        {
            if (level - 1 < 0)
                return false;

            var tileMap = _regions[region].Zones[zone].TileMap;
            for (int i = level - 1; i >= 0; i--)
            {
                var tile = tileMap[(int)position.X, (int)position.Y + 2 * (level - i), i];

                if (tile == null) // might be double checking here but should do in case we call this method from something else
                    return false;

                if (level - 1 == i)
                {
                    // this means we check to make sure we can move into top
                    var canPush = tile.CanMoveOnTop(direction);

                    if (canPush.HasValue)
                        return canPush.Value;
                }
                else
                {

                }

                var canLand = tile.CanLandOn(); 

                if (canLand.HasValue)
                    return canLand.Value;
            }

            return false;
        }

        public static bool IsInBounds(Tile[,,] tileMap, Vector2 position, int level)
        {
            int width = tileMap.GetLength(GameConstants.WIDTH_INDEX);
            int height = tileMap.GetLength(GameConstants.HEIGHT_INDEX);
            int levels = tileMap.GetLength(GameConstants.LEVEL_INDEX);

            if ((int)position.X < 0 || (int)position.X >= width) return false;
            if ((int)position.Y < 0 || (int)position.Y >= height) return false;
            if (level < 0 || level >= levels) return false;

            return true;
        }

        public static Vector2 GetOffsetFromDirection(Directions direction)
        {
            if (direction == Directions.Up) return new Vector2(0, -1);
            else if (direction == Directions.Down) return new Vector2(0, 1);
            else if (direction == Directions.Left) return new Vector2(-1, 0);
            else if (direction == Directions.Right) return new Vector2(1, 0);

            return new Vector2(0, 0);
        }

        public static Vector2 GetOffsetFromDirection(Directions direction, int distance)
        {
            if (direction == Directions.Up) return new Vector2(0, -1 * distance);
            else if (direction == Directions.Down) return new Vector2(0, 1 * distance);
            else if (direction == Directions.Left) return new Vector2(-1 * distance, 0);
            else if (direction == Directions.Right) return new Vector2(1 * distance, 0);

            return new Vector2(0, 0);
        }

        public static Directions GetOppositeDirection(Directions direction)
        {
            if (direction == Directions.Up) return Directions.Down;
            else if (direction == Directions.Down) return Directions.Up;
            else if (direction == Directions.Left) return Directions.Right;
            else return Directions.Left;
        }

        #region GetTile

        public static Tile GetTile(RegionNames region, int zone, Vector2 position, int level)
        {
            var tileMap = _regions[region].Zones[zone].TileMap;
            var inBounds = IsInBounds(tileMap, position, level);

            if (inBounds)
                return tileMap[(int)position.X, (int)position.Y, level];

            var offsets = _regions[region].Offsets;

            foreach (var offset in offsets)
            {
                var updatedTileMap = _regions[offset.OtherRegion].Zones[offset.OtherZone].TileMap;
                var updatedPosition = position - offset.Offset;
                int updatedLevel = level - offset.LevelOffset;
                inBounds = IsInBounds(updatedTileMap, updatedPosition, updatedLevel);

                if (inBounds)
                    return updatedTileMap[(int)updatedPosition.X, (int)updatedPosition.Y, updatedLevel];
            }

            return null;
        }        

        public static Tile GetTileInDirection(Directions direction, RegionNames region, int zone, Vector2 position, int level)
        {
            return GetTileInDirection(direction, region, zone, position, level, 1);
        }

        public static Tile GetTileInDirection(Directions direction, RegionNames region, int zone, Vector2 position, int level, int distance)
        {
            var updatedPosition = position + GetOffsetFromDirection(direction, distance);
            return GetTile(region, zone, updatedPosition, level);
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
            var updatedPosition = position + GetOffsetFromDirection(direction, distance) + new Vector2(0, 2);
            var updatedLevel = level - 1;
            return GetTile(region, zone, updatedPosition, updatedLevel);
        }

        public static Tile GetTileLevelsBelow(Directions direction, RegionNames region, int zone, Vector2 position, int level, int depth)
        {
            var updatedPosition = position + GetOffsetFromDirection(direction) + new Vector2(0, 2 * depth);
            var updatedLevel = level - depth;
            return GetTile(region, zone, updatedPosition, updatedLevel);
        }

        #endregion

        #region Reserve

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

        #endregion

        #region MoveObjects

        public static void MoveNpcInDirection(Npc npc, Directions direction)
        {

        }

        public static void MoveObjectInDirection(TileObject tileObject, Directions direction)
        {

        }

        public static void SpawnObjectAtLocation(TileObject spawnObject, RegionNames region, int zone, Vector2 position, int level)
        {

        }

        #endregion

        public static Dictionary<RegionNames, Region> Regions
        {
            get { return _regions; }
            set { _regions = value; }
        }
    }
}
