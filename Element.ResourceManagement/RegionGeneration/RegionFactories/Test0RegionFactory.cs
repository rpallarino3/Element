using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Data;
using Element.Common.Enumerations.Environment;
using Element.Common.Environment;
using Element.Common.GameObjects.Scenery;

namespace Element.ResourceManagement.RegionGeneration.RegionFactories
{
    public class Test0RegionFactory : IRegionFactory
    {
        public Region CreateRegion(RegionInfo info, SaveData data)
        {
            var region = new Region(RegionNames.Test0);

            // i don't think we actually want to do this
            for (int i = 0; i < info.ZoneTileSizes.Count; i++)
            {
                var zone = new Zone(info.ZoneTileSizes[i], info.ZoneLevels[i]);

                region.Zones.Add(zone);
            }

            // where should the scenery indexes come from?
            region.Zones[0].SceneryObjects.Add(new SceneryObject(0, new Vector2(0, 0), 0, new Vector2(2000, 2000)));

            return region;
        }
    }
}
