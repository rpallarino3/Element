using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.NPCs;
using Element.Common.Enumerations.Sound;

namespace Element.ResourceManagement
{
    public class RegionContent
    {
        public ContentManager ContentManager { get; set; }
        public RegionNames Region { get; set; }
        public Dictionary<ObjectNames, List<Texture2D>> ObjectTextures { get; set; }
        public Dictionary<NpcNames, List<Texture2D>> NpcTextures { get; set; }
        public Dictionary<SceneryNames, List<Texture2D>> SceneryTextures { get; set; }
        public Dictionary<SoundName, SoundEffect> SoundEffects { get; set; }
    }

    public abstract class CrossRegionContent
    {
        public ContentManager ContentManager { get; set; }

        public void Dispose()
        {
            ContentManager.Unload();
            ContentManager.Dispose();
        }
    }

    public class CrossRegionNpcContent : CrossRegionContent
    {
        public NpcNames Id { get; set; }
        public List<Texture2D> Textures { get; set; }
    }

    public class CrossRegionSoundContent : CrossRegionContent
    {
        public SoundName Id { get; set; }
        public SoundEffect Sound { get; set; }
    }
}
