using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Data;
using Element.Common.Enumerations.Environment;
using Element.Common.Environment;

namespace Element.ResourceManagement.RegionGeneration.RegionFactories
{
    public class Test0RegionFactory : IRegionFactory
    {
        public Region CreateRegion(RegionInfo info, SaveData data)
        {
            var region = new Region(RegionNames.Test0);

            return region;
        }
    }
}
