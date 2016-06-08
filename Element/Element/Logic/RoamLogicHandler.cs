using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.ResourceManagement;
using Element.Common.Enumerations.Environment;
using Element.Common.Environment;
using Element.Common.HelperClasses;
using Element.Common.Messages;

namespace Element.Logic
{
    public class RoamLogicHandler
    {
        private ResourceManager _resourceManager;

        private Dictionary<RegionNames, Region> _regions;
        private RegionNames _currentPlayerRegion;

        private Dictionary<RegionNames, Region> _regionsToAdd;
        private List<RegionNames> _regionsToUnload;

        public RoamLogicHandler(ResourceManager resourceManager)
        {
            _regions = new Dictionary<RegionNames, Region>();
            _regionsToUnload = new List<RegionNames>();

            _resourceManager = resourceManager;
            _resourceManager.RegionsLoaded += AddLoadedRegions;
            _resourceManager.RegionsUnloaded += RemoveUnloadRegions;
        }

        public void UpdateLogic()
        {
            CheckLoad();
            CheckUnload();
        }

        #region Load/Unload Regions

        private void CheckLoad()
        {
            var regionsToAdd = new Dictionary<RegionNames, Region>();

            lock (_regionsToAdd)
            {
                foreach (var region in _regionsToAdd.Keys)
                {
                    regionsToAdd[region] = _regionsToAdd[region];
                    _regionsToAdd.Remove(region);
                }
            }

            foreach (var region in regionsToAdd.Keys)
            {
                _regions[region] = regionsToAdd[region];
            }
        }

        private void CheckUnload()
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
        }

        private void AddLoadedRegions(AssetsLoadedEventArgs e)
        {
            lock (_regionsToAdd)
            {
                foreach (var region in e.NewRegions)
                {
                    _regionsToAdd[region.Key] = region.Value;
                }
            }
        }

        private void RemoveUnloadRegions(AssetsUnloadedEventArgs e)
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

        public void UpdatePlayerPositionWithTransition(RoamTransition transition)
        {

        }

        public bool IsRegionLoaded(RegionNames region)
        {
            return _regions.Keys.Contains(region);
        }

        #region Properties

        public Dictionary<RegionNames, Region> Regions
        {
            get { return _regions; }
        }

        public RegionNames CurrentPlayerRegion
        {
            get { return _currentPlayerRegion; }
        }

        #endregion

        #region Events

        public event TransitionEvent InitiateTransition;

        #endregion

    }
}
