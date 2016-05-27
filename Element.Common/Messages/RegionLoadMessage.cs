using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;

namespace Element.Common.Messages
{
    public class RegionLoadMessage
    {
        public List<RegionNames> RegionsToLoad { get; set; }
        public List<RegionNames> RegionsToUnload { get; set; }
    }
}
