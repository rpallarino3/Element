﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Element.Common.HelperClasses
{
    public static class GameConstants
    {
        public static readonly Vector2 SCREEN_SIZE_IN_GAME_UNITS = new Vector2(1280, 720);
        public static readonly Vector2 MAX_TEXTURE_SIZE = new Vector2(2000, 2000);
        public static readonly Vector2 DEFAULT_IMAGE_ORIGIN = new Vector2(0, 0);
        public static readonly float DEFAULT_ROTATION = 0f;
        public static readonly float DEFAULT_LAYER = 0;

        public static readonly float TILE_SIZE = 30;
        public static readonly int TURN_THRESHOLD = 3;
        public static readonly int WIDTH_INDEX = 0;
        public static readonly int HEIGHT_INDEX = 1;
        public static readonly int LEVEL_INDEX = 2;
    }
}
