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
        private static Dictionary<RegionNames, RegionInfo> _regionInfo;

        static RegionFactory()
        {
            _regionFactories = new Dictionary<RegionNames, IRegionFactory>();
            _regionInfo = new Dictionary<RegionNames, RegionInfo>();

            #region Test0
            _regionInfo[RegionNames.Test0] = new RegionInfo();
            #endregion
            #region Test1
            _regionInfo[RegionNames.Test1] = new RegionInfo();
            #endregion
            #region Test2
            _regionInfo[RegionNames.Test2] = new RegionInfo();
            #endregion
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
            return _regionFactories[region].CreateRegion(_regionInfo[region], data);
        }

        public static RegionInfo GetInfoForRegion(RegionNames region)
        {
            return _regionInfo[region];
        }
    }
}
