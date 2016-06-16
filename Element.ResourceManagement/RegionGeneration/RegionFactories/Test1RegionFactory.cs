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
    public class Test1RegionFactory : IRegionFactory
    {
        public Region CreateRegion(RegionInfo info, SaveData data)
        {
            var region = new Region(RegionNames.Test1);

            for (int i = 0; i < info.ZoneTileSizes.Count; i++)
            {
                var zone = new Zone(info.ZoneTileSizes[i], info.ZoneLevels[i]);

                region.Zones.Add(zone);
            }

            region.Zones[0].SceneryObjects.Add(SceneryMapper.CreateSceneryObject(SceneryNames.Test1Level0Top, new Vector2(0, 0), 0));
            region.Zones[0].SceneryObjects.Add(SceneryMapper.CreateSceneryObject(SceneryNames.Test1Level0Bottom, new Vector2(0, 1800), 0));

            return region;
        }
    }
}
