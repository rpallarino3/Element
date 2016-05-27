using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Environment;

namespace Element.ResourceManagement.RegionGeneration
{
    public interface IRegionFactory
    {
        Region CreateRegion();
    }
}
