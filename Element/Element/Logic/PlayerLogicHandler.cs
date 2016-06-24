using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.GameObjects.Npcs;
using Element.Input;

namespace Element.Logic
{
    public static class PlayerLogicHandler
    {
        private static Npc _player;
        private static RegionNames _region;
        private static int _zone;
        
        static PlayerLogicHandler()
        {
            _region = RegionNames.None;
        }

        public static void UpdatePlayerLogic()
        {
        }

        public static Npc Player
        {
            get { return _player; }
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
    }
}
