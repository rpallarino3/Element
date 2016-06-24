using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.NPCs;
using Element.Common.Environment;
using Element.Common.HelperClasses;
using Element.Common.Messages;
using Element.Common.Data;
using Element.ResourceManagement.ContentLoaders;
using Element.ResourceManagement.NpcGeneration;
using Element.ResourceManagement.Sound;
using Element.ResourceManagement.RegionGeneration;

namespace Element.ResourceManagement
{
    public class BackgroundThread
    {
        private IServiceProvider _serviceProvider;
        private string _rootDirectory;

        private List<SaveLoadMessage> _saveRequests; // save requests come from moving between zones, only use the latest save request, the list should be cleared
        private List<RegionNames> _loadRequests; // requests in game to load regions
        private List<RegionNames> _unloadRequests; // requests in game to unload regions
        private object _loadUnloadLock;

        private List<PreferenceData> _preferenceRequests;
        private int _fileLoadRequest; // effectively the opposite of the save request
        private object _fileLoadLock;

        private bool _continueLooping;

        private List<RegionNames> _currentLoadedRegions;

        // everything here needs to be redone

        public BackgroundThread(IServiceProvider serviceProvider, string rootDirectory)
        {
            _serviceProvider = serviceProvider;
            _rootDirectory = rootDirectory;
            
            _saveRequests = new List<SaveLoadMessage>();
            _loadRequests = new List<RegionNames>();
            _unloadRequests = new List<RegionNames>();
            _preferenceRequests = new List<PreferenceData>();
            _fileLoadRequest = -1;

            _currentLoadedRegions = new List<RegionNames>();

            _loadUnloadLock = new object();
            _fileLoadLock = new object();

            _continueLooping = true;
        }

        public void Shutdown()
        {
            // i guess we would want to wait for all open requests to finish
            _continueLooping = false;
        }

        #region Loop

        public void Loop()
        {
            while (_continueLooping)
            {
                // region load/unload requests
                var regionsToLoad = new List<RegionNames>();
                var regionsToUnload = new List<RegionNames>();

                lock (_loadUnloadLock)
                {
                    if (_loadRequests.Count > 0 || _unloadRequests.Count > 0)
                    {
                        regionsToLoad.AddRange(_loadRequests);
                        _loadRequests.Clear();

                        regionsToUnload.AddRange(_unloadRequests);
                        _unloadRequests.Clear();                        
                    }
                }

                ExecuteLoadUnloadRequest(regionsToLoad, regionsToUnload);

                // save requests
                SaveLoadMessage msg = null;

                lock (_saveRequests)
                {
                    if (_saveRequests.Count > 0)
                    {
                        msg = _saveRequests[0];
                        _saveRequests.Clear();
                    }
                }

                ExecuteSaveRequest(msg);

                // pref data requests
                PreferenceData data = null;

                lock (_preferenceRequests)
                {
                    if (_preferenceRequests.Count > 0)
                    {
                        data = _preferenceRequests[0];
                        _preferenceRequests.Clear();                        
                    }
                }

                ExecutePreferenceRequest(data);

                // file load requests
                int fileLoadRequest;

                lock (_fileLoadLock)
                {
                    fileLoadRequest = _fileLoadRequest;
                }

                if (fileLoadRequest != -1)
                {
                    ExecuteFileLoadRequest(_fileLoadRequest);
                }
            }
        }
        
        public bool AreNoRequests()
        {
            lock (_loadUnloadLock)
            {
                if (_loadRequests.Count != 0 || _unloadRequests.Count != 0)
                    return false;
            }

            lock (_saveRequests)
            {
                if (_saveRequests.Count != 0)
                    return false;
            }

            lock (_preferenceRequests)
            {
                if (_preferenceRequests.Count != 0)
                    return false;
            }

            lock (_fileLoadLock)
            {
                if (_fileLoadRequest != -1)
                    return false;
            }

            return true;
        }

        #endregion

        #region Requests

        public void AddSaveRequest(SaveLoadMessage msg)
        {
            lock (_saveRequests)
            {
                _saveRequests.Clear();
                _saveRequests.Add(msg);
            }
        }

        public void AddLoadRequests(List<RegionNames> regionsToLoad)
        {
            lock (_loadUnloadLock)
            {
                foreach (var region in regionsToLoad)
                {
                    if (_unloadRequests.Contains(region))
                        _unloadRequests.Remove(region);

                    if (!_loadRequests.Contains(region))
                        _loadRequests.Add(region);
                }
            }
        }

        public void AddUnloadRequests(List<RegionNames> regionsToUnload)
        {
            lock (_loadUnloadLock)
            {
                foreach (var region in regionsToUnload)
                {
                    if (_loadRequests.Contains(region))
                        _loadRequests.Remove(region);

                    if (_unloadRequests.Contains(region))
                        _unloadRequests.Add(region);
                }
            }
        }

        public void AddLoadAndUnloadRequests(List<RegionNames> regionsToLoad, List<RegionNames> regionsToUnload)
        {
            lock (_loadUnloadLock) // look at this again
            {
                foreach (var region in regionsToLoad)
                {
                    // this will check for duplicates (there shouldn't be any)
                    if (regionsToUnload.Contains(region))
                    {
                        Console.WriteLine("DUPLICATE REGION IN REQUEST: " + region);
                        regionsToLoad.Remove(region);
                        regionsToUnload.Remove(region);
                        continue;
                    }

                    if (_unloadRequests.Contains(region))
                        _unloadRequests.Remove(region);

                    if (!_loadRequests.Contains(region))
                        _loadRequests.Add(region);
                }

                foreach (var region in regionsToUnload)
                {
                    if (_loadRequests.Contains(region))
                        _loadRequests.Remove(region);

                    if (_unloadRequests.Contains(region))
                        _unloadRequests.Add(region);
                }
            }
        }

        public void AddPreferenceRequest(PreferenceData data)
        {
            lock (_preferenceRequests)
            {
                _preferenceRequests.Clear();
                _preferenceRequests.Add(data); // this might not be correct? maybe only take last one?
            }
        }

        public void AddFileLoadRequest(int fileNumber)
        {
            lock (_fileLoadLock)
            {
                _fileLoadRequest = fileNumber;
            }
        }

        #endregion

        #region Executions

        private void ExecuteLoadUnloadRequest(List<RegionNames> regionsToLoad, List<RegionNames> regionsToUnload)
        {
            if (regionsToLoad.Count == 0 && regionsToUnload.Count == 0)
                return;
            
            var crossRegionNpcsToLoad = NpcMapper.GetCrossRegionNpcsForRegions(regionsToLoad);
            var currentCrossRegionNpcsUnload = NpcMapper.GetCrossRegionNpcsForRegions(_currentLoadedRegions);

            var crossRegionSoundsToLoad = SoundMapper.GetCrossRegionSoundEffects(regionsToLoad);
            var currentCrossRegionSoundsUnload = SoundMapper.GetCrossRegionSoundEffects(_currentLoadedRegions);
            
            foreach (var npc in crossRegionNpcsToLoad)
            {
                foreach (var region in NpcMapper.GetRegionsForCrossRegionNpc(npc))
                {
                    if (_currentLoadedRegions.Contains(region))
                    {
                        crossRegionNpcsToLoad.Remove(npc);
                        break;
                    }
                }
            }

            foreach (var sound in crossRegionSoundsToLoad)
            {
                foreach (var region in SoundMapper.GetRegionsForSound(sound))
                {
                    if (_currentLoadedRegions.Contains(region))
                    {
                        crossRegionSoundsToLoad.Remove(sound);
                        break;
                    }
                }
            }

            foreach (var region in regionsToLoad)
                _currentLoadedRegions.Add(region);
            
            foreach (var region in regionsToUnload)
                _currentLoadedRegions.Remove(region);

            foreach (var npc in currentCrossRegionNpcsUnload)
            {
                foreach (var region in NpcMapper.GetRegionsForCrossRegionNpc(npc))
                {
                    if (_currentLoadedRegions.Contains(region))
                    {
                        currentCrossRegionNpcsUnload.Remove(npc);
                        break;
                    }
                }
            }

            foreach (var sound in currentCrossRegionSoundsUnload)
            {
                foreach (var region in SoundMapper.GetRegionsForSound(sound))
                {
                    if (_currentLoadedRegions.Contains(region))
                    {
                        currentCrossRegionSoundsUnload.Remove(sound);
                        break;
                    }
                }
            }

            var saveData = DataHelper.GetCopySaveDataForCurrentFile(); // we need to use this to create the regions
            var regionContent = RegionContentLoader.LoadContentForRegions(_serviceProvider, _rootDirectory, regionsToLoad);            
            var regions = RegionFactory.CreateRegions(regionsToLoad, saveData);                        
            var crossRegionNpcs = NpcMapper.CreateCrossRegionNpcs(crossRegionNpcsToLoad, saveData);
            var crossRegionContent = NpcContentLoader.LoadCrossRegionNpcContent(_serviceProvider, _rootDirectory, crossRegionNpcsToLoad);
            crossRegionContent.AddRange(SoundMapper.CreateCrossRegionSoundContent(_serviceProvider, _rootDirectory, crossRegionSoundsToLoad));
            
            var loadedArgs = new AssetsLoadedEventArgs();
            loadedArgs.RegionContent = regionContent;
            loadedArgs.Regions = regions;
            loadedArgs.CrossRegionContent = crossRegionContent;
            loadedArgs.CrossRegionNpcs = crossRegionNpcs;

            var unloadedArgs = new AssetsUnloadedEventArgs();
            unloadedArgs.RegionsUnloaded = regionsToUnload;
            unloadedArgs.CrossRegionSoundUnloaded = currentCrossRegionSoundsUnload;
            unloadedArgs.CrossRegionNpcsUnloaded = currentCrossRegionNpcsUnload;

            AssetsLoaded(loadedArgs);
            AssetsUnloaded(unloadedArgs);
        }
        
        private void ExecuteSaveRequest(SaveLoadMessage msg)
        {
            if (msg == null)
                return;

            SaveInitiated();

            if (msg.Erase)
                SaveLoadHandler.EraseFile(msg.FileNumber);
            else
            {
                SaveLoadHandler.RequestSave(msg.FileNumber, msg.Data);
            }

            if (_saveRequests.Count == 0)
            {
                SaveCompleted();
            }
        }

        private void ExecutePreferenceRequest(PreferenceData data)
        {
            if (data == null)
                return;

            SaveLoadHandler.SavePreferenceData(data);
        }

        private void ExecuteFileLoadRequest(int fileNumber)
        {
            // what do we even do here?
            // i guess we replace the current save data with the one that is saved
            // so load the save data from file
            // copy it and pass it up to the resource manager
            // then switch files using it

            int file = _fileLoadRequest;

            lock (_fileLoadLock)
            {
                _fileLoadRequest = -1;
            }
        }

        #endregion

        #region Events

        public event AssetsLoadedEvent AssetsLoaded;
        public event AssetsUnloadedEvent AssetsUnloaded;

        public event SaveCompletedEvent SaveCompleted;
        public event SaveInitiatedEvent SaveInitiated;

        #endregion
    }
}
