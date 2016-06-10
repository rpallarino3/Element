﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.NPCs;

namespace Element.Common.GameObjects.Npcs
{
    public class Npc : GameObject
    {
        private List<RegionNames> _possibleRegions;
        private NpcNames _name;
        private NpcTypes _type;

        public List<RegionNames> PossibleRegions
        {
            get { return _possibleRegions; }
        }

        public NpcNames Name
        {
            get { return _name; }
        }

        public NpcTypes Type
        {
            get { return _type; }
        }
    }
}
