using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.GameObjects.Npcs;
using Element.Common.GameObjects.Scenery;
using Element.Common.GameObjects.Tile;
using Element.Common.GameObjects.TileObjects;

namespace Element.Common.Environment
{
    public class Zone
    {
        private Tile[, ,] _tileMap;
        private List<Npc> _npcs;
        private List<TileObject> _tileObjects;
        private List<SceneryObject> _sceneryObjects;
        private List<Rectangle> _cameraCollisionBoxes;
    }
}
