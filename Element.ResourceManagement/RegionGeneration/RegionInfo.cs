﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
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
        private List<NpcNames> _crossRegionNpcs;

        private List<SceneryNames> _sceneryNames;

        private List<SoundName> _regionSounds;
        private List<SoundName> _crossRegionSounds;

        private List<RegionNames> _adjacentRegions;
        private List<RegionOffset> _regionOffsets;

        private List<Vector2> _zoneTileSizes;
        private List<int> _zoneLevels;

        private List<Rectangle> _cameraCollisionBoxes;

        public RegionInfo(RegionTheme theme)
        {
            _theme = theme;

            _tileObjects = new List<ObjectNames>();

            _npcs = new List<NpcNames>();
            _crossRegionNpcs = new List<NpcNames>();

            _sceneryNames = new List<SceneryNames>();

            _regionSounds = new List<SoundName>();
            _crossRegionSounds = new List<SoundName>();

            _adjacentRegions = new List<RegionNames>();
            _regionOffsets = new List<RegionOffset>();

            _zoneTileSizes = new List<Vector2>();
            _zoneLevels = new List<int>();

            _cameraCollisionBoxes = new List<Rectangle>();
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

        public List<NpcNames> CrossRegionNpcs
        {
            get { return _crossRegionNpcs; }
            set { _crossRegionNpcs = value; }
        }

        public List<SceneryNames> Scenery
        {
            get { return _sceneryNames; }
            set { _sceneryNames = value; }
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

        public List<SoundName> CrossRegionSounds
        {
            get { return _crossRegionSounds; }
            set { _crossRegionSounds = value; }
        }

        public List<Vector2> ZoneTileSizes
        {
            get { return _zoneTileSizes; }
            set { _zoneTileSizes = value; }
        }

        public List<int> ZoneLevels
        {
            get { return _zoneLevels; }
            set { _zoneLevels = value; }
        }

        public List<Rectangle> CameraCollisionBoxes
        {
            get { return _cameraCollisionBoxes; }
            set { _cameraCollisionBoxes = value; }
        }
    }
}
