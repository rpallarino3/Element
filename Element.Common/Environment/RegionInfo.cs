using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.Environment;

namespace Element.Common.Environment
{
    public class RegionInfo
    {
        public RegionNames Name { get; set; }
        public RegionTheme Theme { get; set; }
        public List<RegionNames> RegionsToLoad { get; set; }
        public List<RegionNames> RegionsToUnload { get; set; }
        public Dictionary<RegionNames, Vector2> RegionOffsets { get; set; }
    }
}
