using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Element.Common.Enumerations.Environment;
using Element.Common.Environment;

namespace Element.ResourceManagement
{
    public class AssetsLoadedEventArgs : EventArgs
    {
        public Dictionary<RegionNames, Region> NewRegions { get; set; }
        public Dictionary<RegionNames, Dictionary<int, Texture2D>> NewSceneryTextures { get; set; }
        public Dictionary<RegionNames, Dictionary<int, Texture2D>> NewObjectTextures { get; set; }
        public Dictionary<RegionNames, Dictionary<int, Texture2D>> NewNpcTextures { get; set; }
    }

    public class AssetsUnloadedEventArgs : EventArgs
    {
        public List<RegionNames> RegionsUnloaded { get; set; }
    }

    public delegate void AssetsLoadedEvent(AssetsLoadedEventArgs e);
    public delegate void AssetsUnloadedEvent(AssetsUnloadedEventArgs e);
    public delegate void AssetLoadRequestedEvent();
    public delegate void AllAssetsLoadedEvent();
}
