using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Microsoft.Xna.Framework;

namespace Element.ResourceManagement.RegionGeneration
{
    public class RegionOffset
    {
        private RegionNames _achorRegion;
        private RegionNames _otherRegion;
        private int _anchorZone;
        private int _otherZone;
        private Vector2 _offset;
    }
}
