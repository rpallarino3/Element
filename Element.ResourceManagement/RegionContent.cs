using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.NPCs;

namespace Element.ResourceManagement
{
    public class RegionContent
    {
        public ContentManager ContentManager { get; set; }
        public RegionNames Region { get; set; }
        public Dictionary<ObjectNames, List<Texture2D>> ObjectTextures { get; set; }
        public Dictionary<NpcNames, List<Texture2D>> NpcTextures { get; set; }
        public Dictionary<int, List<Texture2D>> SceneryTextures { get; set; }
        // add sounds here
    }

    public abstract class CrossRegionContent
    {
        public ContentManager ContentManager { get; set; }
    }

    public class CrossRegionNpcContent : CrossRegionContent
    {
        public NpcNames Id { get; set; }
        public List<Texture2D> Textures { get; set; }
    }

    public class CrossRegionSoundContent : CrossRegionContent
    {
    }
}
