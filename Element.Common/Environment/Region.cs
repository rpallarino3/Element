using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.Environment;

namespace Element.Common.Environment
{
    public class Region
    {
        private RegionNames _name;
        private List<Zone> _zones;
        
        public Region(RegionNames name)
        {
            _name = name;
            _zones = new List<Zone>();
        }

        public RegionNames Name
        {
            get { return _name; }
        }

        public List<Zone> Zones
        {
            get { return _zones; }
        }
    }
}
