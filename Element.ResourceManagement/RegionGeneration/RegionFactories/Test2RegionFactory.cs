using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Data;
using Element.Common.Enumerations.Environment;
using Element.Common.Environment;
using Element.Common.GameObjects.Scenery;
using Element.ResourceManagement.Scenery;

namespace Element.ResourceManagement.RegionGeneration.RegionFactories
{
    public class Test2RegionFactory : IRegionFactory
    {
        public Region CreateRegion(RegionInfo info, SaveData data)
        {
            var region = new Region(RegionNames.Test2);

            for (int i = 0; i < info.ZoneTileSizes.Count; i++)
            {
                var zone = new Zone(info.ZoneTileSizes[i], info.ZoneLevels[i]);

                region.Zones.Add(zone);
            }

            region.Zones[0].SceneryObjects.Add(SceneryMapper.CreateSceneryObject(SceneryNames.Test2Level0Left, new Vector2(0, 0), 0));
            region.Zones[0].SceneryObjects.Add(SceneryMapper.CreateSceneryObject(SceneryNames.Test2Level0Right, new Vector2(1800, 0), 0));

            return region;
        }
    }
}
