using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.NPCs;
using Element.ResourceManagement.NpcGeneration;
using Element.ResourceManagement.RegionGeneration;

namespace Element.ResourceManagement.ContentLoaders
{
    public static class NpcContentLoader
    {
        private static Dictionary<NpcTypes, List<string>> _fileNames;

        static NpcContentLoader()
        {
            _fileNames = new Dictionary<NpcTypes, List<string>>();
            
        }

        public static List<CrossRegionContent> LoadCrossRegionNpcContent(IServiceProvider serviceProvider, string rootDirectory, List<NpcNames> npcsToLoad)
        {
            var loadedContent = new List<CrossRegionContent>();

            foreach (var npc in npcsToLoad)
            {
                var content = new CrossRegionNpcContent();
                var contentManager = new ContentManager(serviceProvider, rootDirectory);
                var textureFiles = GetFileNamesFromType(npc);
                var textures = new List<Texture2D>();

                foreach (var file in textureFiles)
                {
                    textures.Add(contentManager.Load<Texture2D>(file));
                }

                content.Id = npc;
                content.ContentManager = contentManager;
                content.Textures = textures;
            }

            return loadedContent;
        }

        // i think this is fine since the npc name is what defines the texture i suppose
        public static Dictionary<NpcNames, List<Texture2D>> LoadNpcsForRegion(ContentManager contentManager, RegionNames region)
        {
            var npcTextures = new Dictionary<NpcNames, List<Texture2D>>();
            var npcs = RegionFactory.GetInfoForRegion(region).Npcs;

            foreach (var npc in npcs)
            {
                var fileNames = GetFileNamesFromType(npc);
                var textures = new List<Texture2D>();

                foreach (var file in fileNames)
                {
                    textures.Add(contentManager.Load<Texture2D>(file));
                }

                npcTextures.Add(npc, textures);
            }

            return npcTextures;
        }


        public static IEnumerable<string> GetFileNamesFromType(NpcNames npc)
        {
            return _fileNames[NpcMapper.GetTypeForNpc(npc)];
        }
    }
}
