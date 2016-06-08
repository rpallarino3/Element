using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.Environment;

namespace Element.ResourceManagement.RegionGeneration
{
    public class RegionFactory
    {
        private Dictionary<RegionNames, IRegionFactory> _regionFactories;

        public RegionFactory()
        {
            _regionFactories = new Dictionary<RegionNames, IRegionFactory>();
        }

        // make sure to apply a copy of the save data when creating the regions
        public Dictionary<RegionNames, Region> CreateRegions(List<RegionNames> regionsToCreate)
        {
            var regions = new Dictionary<RegionNames, Region>();

            foreach (var region in regionsToCreate)
            {
                regions.Add(region, CreateRegion(region));
            }

            return regions;
        }

        public Region CreateRegion(RegionNames region)
        {
            return _regionFactories[region].CreateRegion();
        }
    }
}
