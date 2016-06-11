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
        private RegionNames _anchorRegion;
        private RegionNames _otherRegion;
        private int _anchorZone;
        private int _otherZone;
        private Vector2 _offset;

        public RegionNames AnchorRegion
        {
            get { return _anchorRegion; }
            set { _anchorRegion = value; }
        }

        public RegionNames OtherRegion
        {
            get { return _otherRegion; }
            set { _otherRegion = value; }
        }

        public int AnchorZone
        {
            get { return _anchorZone; }
            set { _anchorZone = value; }
        }

        public int OtherZone
        {
            get { return _otherZone; }
            set { _otherZone = value; }
        }

        public Vector2 Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }
    }
}
