using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.NPCs;
using Element.Common.Enumerations.Sound;
using Element.ResourceManagement.Sound;

namespace Element.ResourceManagement.ContentLoaders
{
    public static class RegionContentLoader
    {
        private static Dictionary<RegionNames, List<NpcNames>> _regionNpcs;
        private static Dictionary<RegionNames, List<ObjectNames>> _regionObjects; // do the sounds come with the objects?
        private static Dictionary<RegionNames, List<string>> _regionScenery; // idk exactly what we do here, maybe we just load all files in directory, need to index them somehow
        
        static RegionContentLoader()
        {
            _regionNpcs = new Dictionary<RegionNames, List<NpcNames>>();
            _regionObjects = new Dictionary<RegionNames, List<ObjectNames>>();
            _regionScenery = new Dictionary<RegionNames, List<string>>();

            #region Test0
            _regionNpcs[RegionNames.Test0] = new List<NpcNames>();
            _regionObjects[RegionNames.Test0] = new List<ObjectNames>();
            _regionScenery[RegionNames.Test0] = new List<string>();
            #endregion

            #region Test1
            _regionNpcs[RegionNames.Test1] = new List<NpcNames>();
            _regionObjects[RegionNames.Test1] = new List<ObjectNames>();
            _regionScenery[RegionNames.Test1] = new List<string>();
            #endregion

            #region Test2
            _regionNpcs[RegionNames.Test2] = new List<NpcNames>();
            _regionObjects[RegionNames.Test2] = new List<ObjectNames>();
            _regionScenery[RegionNames.Test2] = new List<string>();
            #endregion
        }

        // need to know the npcs to load, the scenery to load, the objects to load
        public static List<RegionContent> LoadContentForRegions(IServiceProvider serviceProvider, string rootDirectory, List<RegionNames> regions )
        {
            var regionContentList = new List<RegionContent>();

            foreach (var region in regions)
            {
                var regionContent = new RegionContent();
                var contentManager = new ContentManager(serviceProvider, rootDirectory);

                regionContent.Region = region;
                regionContent.NpcTextures = NpcContentLoader.LoadNpcsForRegion(contentManager, _regionNpcs[region]);
                regionContent.ObjectTextures = LoadObjectTextures(contentManager, region);
                regionContent.SceneryTextures = LoadSceneryTextures(contentManager, region);
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

        private static Dictionary<int, List<Texture2D>> LoadSceneryTextures(ContentManager contentManager, RegionNames region)
        {
            var sceneryDictionary = new Dictionary<int, List<Texture2D>>();

            return sceneryDictionary;
        }
    }
}
