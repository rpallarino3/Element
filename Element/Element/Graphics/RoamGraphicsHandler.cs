using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Element.Common.Animations;
using Element.Common.Enumerations.Environment;
using Element.Common.Environment;
using Element.Common.HelperClasses;
using Element.ResourceManagement;
using Element.ResourceManagement.RegionGeneration;
using Element.Logic;

namespace Element.Graphics
{
    public class RoamGraphicsHandler
    {

        public void DrawRoam(SpriteBatch sb, LogicHandler logic, ResourceManager resourceManager)
        {
            var screenRatio = DataHelper.GetScreenRatioFromResolution();

            var cameraTopLeft = Camera.Location - GameConstants.SCREEN_SIZE_IN_GAME_UNITS / 2 + new Vector2(1, 1);
            var cameraRegion = Camera.Region;
            // need to think about how to do this, once the scenery is displayed i would close the branch and make a new one for player movement

            var cameraZone = Camera.Zone;
            var cameraZoneOffsets = RegionLayout.RegionInfo[cameraRegion].RegionOffsets.Where(ro => ro.AnchorZone == cameraZone);

            var drawInfo = new List<DrawInfo>();

            // do we need to do the player first?
            
            // maybe do the transition handler.showregionimage here
            if (!logic.RoamLogicHandler.Regions.ContainsKey(cameraRegion))
            {
                // draw black screen
                return;
            }

            // did we decide to check if the zone was present first before we drew it?

            var currentZone = logic.RoamLogicHandler.Regions[cameraRegion].Zones[cameraZone];
            CreateZoneDrawInfo(cameraRegion, currentZone, drawInfo, new Vector2(0, 0), resourceManager);

            foreach (var offset in cameraZoneOffsets)
            {
                var zone = logic.RoamLogicHandler.Regions[offset.OtherRegion].Zones[offset.OtherZone];
                CreateZoneDrawInfo(offset.OtherRegion, zone, drawInfo, offset.Offset, resourceManager);
            }

            // need to do cross region npcs here too

            drawInfo.Sort();
            DrawDrawInfo(sb, screenRatio, drawInfo, logic.DrawColor);
            // do whatever else we need to do here
        }

        private void CreateZoneDrawInfo(RegionNames region, Zone zone, List<DrawInfo> info, Vector2 offset, ResourceManager resourceManager)
        {
            foreach (var scenery in zone.SceneryObjects)
            {
                var textures = resourceManager.RegionContent[region].SceneryTextures[scenery.Name];
                var texture = GetTextureFromList(textures, scenery.Animator);
                var drawLocation = scenery.Location + scenery.Animator.DrawOffset + offset;
                var x = scenery.Animator.ImageSize.X * scenery.Animator.AnimationCounter;
                var y = (scenery.Animator.ImageSize.Y * scenery.Animator.CurrentAnimation.Row) % GameConstants.MAX_TEXTURE_SIZE.Y;
                var drawRectangle = new Rectangle((int)x, (int)y, (int)scenery.Animator.ImageSize.X, (int)scenery.Animator.ImageSize.Y);
                var drawInfo = new DrawInfo(scenery.OnFloor, texture, drawLocation, drawRectangle, scenery.Level);
                info.Add(drawInfo);
            }

            foreach (var tileObject in zone.TileObjects)
            {
                var textures = resourceManager.RegionContent[region].ObjectTextures[tileObject.Name];
                var texture = GetTextureFromList(textures, tileObject.Animator);
                var drawLocation = tileObject.Location + tileObject.Animator.DrawOffset + offset;
                var x = tileObject.Animator.ImageSize.X * tileObject.Animator.AnimationCounter;
                var y = (tileObject.Animator.ImageSize.Y * tileObject.Animator.CurrentAnimation.Row) % GameConstants.MAX_TEXTURE_SIZE.Y;
                var drawRectangle = new Rectangle((int)x, (int)y, (int)tileObject.Animator.ImageSize.X, (int)tileObject.Animator.ImageSize.Y);
                var drawInfo = new DrawInfo(false, texture, drawLocation, drawRectangle, tileObject.Level);
                info.Add(drawInfo);
            }

            foreach (var npc in zone.Npcs)
            {
                var textures = resourceManager.RegionContent[region].NpcTextures[npc.Name];
                var texture = GetTextureFromList(textures, npc.Animator);
                var drawLocation = npc.Location + npc.Animator.DrawOffset + offset;
                var x = npc.Animator.ImageSize.X * npc.Animator.AnimationCounter;
                var y = (npc.Animator.ImageSize.Y * npc.Animator.CurrentAnimation.Row) % GameConstants.MAX_TEXTURE_SIZE.Y;
                var drawRectangle = new Rectangle((int)x, (int)y, (int)npc.Animator.ImageSize.X, (int)npc.Animator.ImageSize.Y);
                var drawInfo = new DrawInfo(false, texture, drawLocation, drawRectangle, npc.Level);
                info.Add(drawInfo);
            }
        }

        private void DrawDrawInfo(SpriteBatch sb, Vector2 screenRatio, List<DrawInfo> drawInfo, Color color)
        {
            foreach (var item in drawInfo)
            {
                sb.Draw(
                    item.Texture, 
                    item.DrawLocation * screenRatio, 
                    item.DrawRectangle, 
                    color, 
                    GameConstants.DEFAULT_ROTATION, 
                    GameConstants.DEFAULT_IMAGE_ORIGIN,
                    screenRatio, 
                    SpriteEffects.None, 
                    GameConstants.DEFAULT_LAYER);
            }
        }

        private Texture2D GetTextureFromList(List<Texture2D> textures, Animator animator)
        {
            var imageHeight = animator.ImageSize.Y;
            var totalHeight = imageHeight * animator.CurrentAnimation.Row;
            var textureIndex = (int)totalHeight / (int)GameConstants.MAX_TEXTURE_SIZE.Y;

            return textures[textureIndex];
        }

        private bool IsOnScreen(Vector2 cameraTopLeft, Vector2 objLocation, Vector2 objSize)
        {
            if (objLocation.Y + objSize.Y <= cameraTopLeft.Y)
                return false;
            if (objLocation.Y >= cameraTopLeft.Y + GameConstants.SCREEN_SIZE_IN_GAME_UNITS.Y)
                return false;
            if (objLocation.X + objSize.X <= cameraTopLeft.X)
                return false;
            if (objLocation.X >= cameraTopLeft.X + GameConstants.SCREEN_SIZE_IN_GAME_UNITS.X)
                return false;

            return true;
        }
    }
}
