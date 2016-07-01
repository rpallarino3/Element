using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.Environment;
using Element.Common.GameObjects.Npcs;
using Element.Common.HelperClasses;
using Element.Common.Messages;
using Element.Input;
using Element.ResourceManagement;

namespace Element.Logic
{
    public static class RoamLogicHandler
    {
        private static Dictionary<RegionNames, Region> _regions;

        private static Dictionary<RegionNames, Region> _regionsToAdd;
        private static List<RegionNames> _regionsToUnload;
        private static List<Npc> _crossRegionNpcsToAdd;

        private static List<Npc> _crossRegionNpcs;

        static RoamLogicHandler()
        {
            _regions = new Dictionary<RegionNames, Region>();
            _regionsToUnload = new List<RegionNames>();

            _regionsToAdd = new Dictionary<RegionNames, Region>();
            _regionsToUnload = new List<RegionNames>();
            _crossRegionNpcsToAdd = new List<Npc>();

            _crossRegionNpcs = new List<Npc>();            
        }

        public static void SubscribeToEvents()
        {
            ResourceManager.RegionsLoaded += AddLoadedRegions;
            ResourceManager.RegionsUnloaded += RemoveUnloadRegions;
        }

        public static void UpdateLogic()
        {
            CheckLoad();
            CheckUnload();
            TrafficHandler.Regions = _regions;
        }

        #region Load/Unload Regions

        private static void CheckLoad()
        {
            var regionsToAdd = new Dictionary<RegionNames, Region>();

            lock (_regionsToAdd)
            {
                foreach (var region in _regionsToAdd.Keys)
                {
                    regionsToAdd[region] = _regionsToAdd[region];
                }
                _regionsToAdd.Clear();
            }

            foreach (var region in regionsToAdd.Keys)
            {
                _regions[region] = regionsToAdd[region];
            }
            
            lock (_crossRegionNpcsToAdd)
            {
                _crossRegionNpcs.AddRange(_crossRegionNpcsToAdd);
                _crossRegionNpcsToAdd.Clear();
            }
        }

        private static void CheckUnload()
        {
            var regionsToUnload = new List<RegionNames>();

            lock (_regionsToUnload)
            {
                if (_regionsToUnload.Count != 0)
                {
                    regionsToUnload.AddRange(_regionsToUnload);
                    _regionsToUnload.Clear();
                }
            }

            foreach (var region in regionsToUnload)
            {
                _regions.Remove(region);
            }

            foreach (var npc in _crossRegionNpcs) // not sure if this will work
            {
                bool cont = false;

                foreach (var region in npc.PossibleRegions)
                {
                    if (_regions.ContainsKey(region))
                    {
                        cont = true;
                        break;
                    }
                }

                if (cont)
                    continue;

                DataHelper.SaveCrossRegionNpcState(npc);
                _crossRegionNpcs.Remove(npc);
            }
        }
        
        private static void AddLoadedRegions(AssetsLoadedEventArgs e)
        {
            lock (_regionsToAdd)
            {
                foreach (var region in e.Regions)
                {
                    _regionsToAdd[region.Name] = region;
                }
            }

            lock (_crossRegionNpcsToAdd)
            {
                foreach (var npc in e.CrossRegionNpcs)
                {
                    _crossRegionNpcsToAdd.Add(npc);
                }
            }
        }

        private static void RemoveUnloadRegions(AssetsUnloadedEventArgs e)
        {
            lock (_regionsToUnload)
            {
                foreach (var region in e.RegionsUnloaded)
                {
                    if (!_regionsToUnload.Contains(region))
                        _regionsToUnload.Add(region);
                }
            }
        }

        #endregion
                
        public static void UpdatePlayerPositionWithTransition(RoamTransition transition)
        {
            // update teh camera here too, this isn't going to be entirely correct
            Camera.Zone = transition.DestinationZone;
            Camera.Region = transition.DestinationRegion;
            Camera.Location = transition.DestinationCoords * GameConstants.TILE_SIZE; // going to need to calc camera position based on player coords and zone size
        }

        public static bool IsRegionLoaded(RegionNames region)
        {
            return _regions.Keys.Contains(region);
        }

        #region Properties

        public static Dictionary<RegionNames, Region> Regions
        {
            get { return _regions; }
        }        

        #endregion

        #region Events

        public static event TransitionEvent InitiateTransition;

        #endregion

    }
}
