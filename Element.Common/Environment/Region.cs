using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;

namespace Element.Common.Environment
{
    public class Region
    {
        private RegionNames _name;
        private List<Zone> _zones;

        public RegionNames Name
        {
            get { return _name; }
        }
    }
}
