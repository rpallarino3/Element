﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.NPCs;
using Element.Common.Enumerations.Sound;

namespace Element.ResourceManagement.RegionGeneration
{
    public class RegionInfo
    {
        private RegionTheme _theme;
        private List<ObjectNames> _tileObjects;
        private List<NpcNames> _npcs;
        private List<RegionNames> _adjacentRegions;
        private List<RegionOffset> _regionOffsets;
        private List<SoundName> _regionSounds;

        public RegionInfo()
        {
            _tileObjects = new List<ObjectNames>();
            _npcs = new List<NpcNames>();
            _adjacentRegions = new List<RegionNames>();
            _regionOffsets = new List<RegionOffset>();
            _regionSounds = new List<SoundName>();
        }

        public RegionTheme Theme
        {
            get { return _theme; }
            set { _theme = value; }
        }

        public List<ObjectNames> TileObjects
        {
            get { return _tileObjects; }
            set { _tileObjects = value; }
        }

        public List<NpcNames> Npcs
        {
            get { return _npcs; }
            set { _npcs = value; }
        }

        public List<RegionNames> AdjacentRegions
        {
            get { return _adjacentRegions; }
            set { _adjacentRegions = value; }
        }

        public List<RegionOffset> RegionOffsets
        {
            get { return _regionOffsets; }
            set { _regionOffsets = value; }
        }

        public List<SoundName> RegionSounds
        {
            get { return _regionSounds; }
            set { _regionSounds = value; }
        }
    }
}