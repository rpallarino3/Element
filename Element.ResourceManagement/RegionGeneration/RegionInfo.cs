using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.NPCs;

namespace Element.ResourceManagement.RegionGeneration
{
    public class RegionInfo
    {
        private RegionTheme _theme;
        private List<ObjectNames> _tileObjects;
        private List<NpcNames> _npcs;
        private List<RegionNames> _adjacentRegions;
        private List<RegionOffset> _regionOffsets;
    }
}
