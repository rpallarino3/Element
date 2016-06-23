using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.GameObjects.Npcs;

namespace Element.Logic
{
    public class PlayerLogicHandler
    {
        private Npc _player;
        private RegionNames _region;
        private int _zone;

        public void UpdatePlayerLogic()
        {

        }

        public PlayerLogicHandler()
        {
            _region = RegionNames.None;
        }

        public Npc Player
        {
            get { return _player; }
        }

        public RegionNames Region
        {
            get { return _region; }
            set { _region = value; }
        }

        public int Zone
        {
            get { return _zone; }
            set { _zone = value; }
        }
    }
}
