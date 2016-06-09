using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.Environment;
using Element.Common.HelperClasses;
using Element.Common.Messages;
using Element.Common.Data;

namespace Element.ResourceManagement
{
    public class BackgroundThread
    {
        private SaveLoadHandler _saveLoadHandler;

        private List<SaveLoadMessage> _saveRequests; // save requests come from moving between zones, only use the latest save request, the list should be cleared
        private List<RegionNames> _loadRequests; // requests in game to load regions
        private List<RegionNames> _unloadRequests; // requests in game to unload regions
        private object _loadUnloadLock;

        private List<PreferenceData> _preferenceRequests;
        private int _fileLoadRequest; // effectively the opposite of the save request
        private object _fileLoadLock;

        private bool _continueLooping;

        // everything here needs to be redone

        public BackgroundThread(SaveLoadHandler saveLoadHandler)
        {
            _saveLoadHandler = saveLoadHandler;
            _saveRequests = new List<SaveLoadMessage>();
            _loadRequests = new List<RegionNames>();
            _unloadRequests = new List<RegionNames>();
            _preferenceRequests = new List<PreferenceData>();
            _fileLoadRequest = -1;

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

            var saveData = DataHelper.GetDataCopyForCurrentFile();

            // load all the textures and then build the regions
            // this is where we need to determine what we need to save in the save data
            // need some place to list all the stuff we have to load
            // the save data should let us know if the position of a cross region npc has changed

            // to load content we need to know:
            // the theme of the region
            // which scenery to load and their indexes
            // which objects are preset in the region
            // which npcs are preset in the region and their types
            // which region specific sounds we need to load
            // which cross region npcs we need to load
            // which cross region sounds we need to load

            // to unload we need to know:
            // which regions we are unloading
            // which cross region npcs can be unloaded
            // which cross region sounds can be unloaded
        }
        
        private void ExecuteSaveRequest(SaveLoadMessage msg)
        {
            if (msg == null)
                return;

            SaveInitiated();

            if (msg.Erase)
                _saveLoadHandler.EraseFile(msg.FileNumber);
            else
            {
                _saveLoadHandler.RequestSave(msg.FileNumber, msg.Data);
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

            _saveLoadHandler.SavePreferenceData(data);
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
