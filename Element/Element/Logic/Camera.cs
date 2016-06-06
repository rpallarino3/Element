using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.Environment;

namespace Element.Logic
{
    public static class Camera
    {
        private static RegionNames _region;
        private static int _zone;
        private static Vector2 _location; // this is not tile location, this is global coords location

        public static RegionNames Region
        {
            get { return _region; }
            set { _region = value; }
        }

        public static int Zone
        {
            get { return _zone; }
            set { _zone = value; }
        }

        public static Vector2 Location
        {
            get { return _location; }
            set { _location = value; }
        }
    }
}
