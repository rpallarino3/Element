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
        private Dictionary<RegionNames, Region> _regions;

        private ResourceManager _resourceManager;

        public RoamLogicHandler(ResourceManager resourceManager)
        {
            _regions = new Dictionary<RegionNames, Region>();

            _resourceManager = resourceManager;
        }

        private void AddLoadedRegions(AssetsLoadedEventArgs e)
        {
            _resourceManager.RequestSave(new SaveLoadMessage());
            // add new regions to data structure, there should be no duplicates
            foreach (var item in e.NewRegions.Keys)
                _regions.Add(item, e.NewRegions[item]);
            // remove the old ones and save them
        }

        public bool IsRegionLoaded(RegionNames region)
        {
            return _regions.Keys.Contains(region);
        }

        private void RemoveRegions(AssetsUnloadedEventArgs e)
        {
            foreach (var item in e.RegionsUnloaded)
                _regions.Remove(item);
        }

        public Dictionary<RegionNames, Region> Regions
        {
            get { return _regions; }
        }

        public event TransitionEvent InitiateTransition;
    }
}
