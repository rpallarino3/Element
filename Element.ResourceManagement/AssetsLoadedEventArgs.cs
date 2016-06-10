using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.NPCs;
using Element.Common.Environment;
using Element.Common.GameObjects.Npcs;

namespace Element.ResourceManagement
{
    public class AssetsLoadedEventArgs : EventArgs
    {
        public List<Region> Regions { get; set; }
        public List<Npc> CrossRegionNpcs { get; set; }
        public List<RegionContent> RegionContent { get; set; }
        public List<CrossRegionContent> CrossRegionContent { get; set; }
    }

    public class AssetsUnloadedEventArgs : EventArgs
    {
        public List<RegionNames> RegionsUnloaded { get; set; }
        public List<NpcNames> CrossRegionNpcsUnloaded { get; set; } // do we even need this?
        // add sounds here
    }

    public delegate void AssetsLoadedEvent(AssetsLoadedEventArgs e);
    public delegate void AssetsUnloadedEvent(AssetsUnloadedEventArgs e);
    public delegate void AssetLoadRequestedEvent();
    public delegate void AllAssetsLoadedEvent();
}
