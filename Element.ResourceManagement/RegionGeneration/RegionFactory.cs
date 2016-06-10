using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.Environment;
using Element.Common.Data;

namespace Element.ResourceManagement.RegionGeneration
{
    public static class RegionFactory
    {
        private static Dictionary<RegionNames, IRegionFactory> _regionFactories;

        static RegionFactory()
        {
            _regionFactories = new Dictionary<RegionNames, IRegionFactory>();
        }

        // make sure to apply a copy of the save data when creating the regions
        public static List<Region> CreateRegions(List<RegionNames> regionsToCreate, SaveData data)
        {
            var regions = new List<Region>();

            foreach (var region in regionsToCreate)
            {
                regions.Add(CreateRegion(region));
            }

            return regions;
        }

        public static Region CreateRegion(RegionNames region)
        {
            return _regionFactories[region].CreateRegion();
        }
    }
}
