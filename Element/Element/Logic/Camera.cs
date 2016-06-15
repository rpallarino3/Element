using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.Environment;
using Element.Common.GameObjects;

namespace Element.Logic
{
    public static class Camera
    {
        private const int PAN_MAX_DISTANCE = 10;

        private static RegionNames _region;
        private static int _zone;
        private static Vector2 _location; // this is not tile location, this is global coords location

        private static bool _fixedOnObject;
        private static Vector2 _fixedPoint;
        private static GameObject _fixedObject;

        public static void UpdateLogic(List<Rectangle> cameraCollisionBoxes)
        {
            Vector2 destinationLocation;

            if (_fixedOnObject)
                destinationLocation = _fixedObject.Location;
            else
                destinationLocation = _fixedPoint;

            var distanceToMove = destinationLocation - _location;
        }

        // we might need to tell it what region and zone it is going to
        public static void LockOnObject(GameObject fixedObject)
        {
            _fixedObject = fixedObject;
            _fixedOnObject = true;
        }

        public static void LockOnPoint(Vector2 fixedPoint)
        {
            _fixedPoint = fixedPoint;
            _fixedOnObject = false;
        }

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
