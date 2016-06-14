using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.Environment;
using Element.Common.Data;
using Element.ResourceManagement.RegionGeneration.RegionFactories;

namespace Element.ResourceManagement.RegionGeneration
{
    public static class RegionFactory
    {
        private static Dictionary<RegionNames, IRegionFactory> _regionFactories;

        static RegionFactory()
        {
            _regionFactories = new Dictionary<RegionNames, IRegionFactory>();

            _regionFactories[RegionNames.Test0] = new Test0RegionFactory();
            _regionFactories[RegionNames.Test1] = new Test1RegionFactory();
            _regionFactories[RegionNames.Test2] = new Test2RegionFactory();
        }

        // make sure to apply a copy of the save data when creating the regions
        public static List<Region> CreateRegions(List<RegionNames> regionsToCreate, SaveData data)
        {
            var regions = new List<Region>();

            foreach (var region in regionsToCreate)
            {
                regions.Add(CreateRegion(region, data));
            }

            return regions;
        }

        public static Region CreateRegion(RegionNames region, SaveData data)
        {
            return _regionFactories[region].CreateRegion(GetInfoForRegion(region), data);
        }

        public static RegionInfo GetInfoForRegion(RegionNames region)
        {
            return RegionLayout.RegionInfo[region];
        }
    }
}
