using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.NPCs;
using Element.Common.Enumerations.Sound;
using Element.ResourceManagement.Scenery;
using Element.ResourceManagement.Sound;

namespace Element.ResourceManagement.ContentLoaders
{
    public static class RegionContentLoader
    {
        // need to know the npcs to load, the scenery to load, the objects to load
        public static List<RegionContent> LoadContentForRegions(IServiceProvider serviceProvider, string rootDirectory, List<RegionNames> regions )
        {
            var regionContentList = new List<RegionContent>();

            foreach (var region in regions)
            {
                var regionContent = new RegionContent();
                var contentManager = new ContentManager(serviceProvider, rootDirectory);

                regionContent.Region = region;
                regionContent.NpcTextures = NpcContentLoader.LoadNpcsForRegion(contentManager, region);
                regionContent.ObjectTextures = LoadObjectTextures(contentManager, region); // don't need an object mapper because each object has it's own class
                regionContent.SceneryTextures = SceneryMapper.LoadSceneryTextures(contentManager, region);
                regionContent.SoundEffects = SoundMapper.LoadSoundsForRegion(contentManager, region);
                regionContent.ContentManager = contentManager;

                regionContentList.Add(regionContent);
            }

            return regionContentList;
        }

        private static Dictionary<ObjectNames, List<Texture2D>> LoadObjectTextures(ContentManager contentManager, RegionNames region)
        {
            var textureDictionary = new Dictionary<ObjectNames, List<Texture2D>>();



            return textureDictionary;
        }
    }
}
