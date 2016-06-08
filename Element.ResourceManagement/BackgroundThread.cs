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

        public void Loop()
        {
            while (_continueLooping)
            {
                // maybe we don't want to do the load/unload one at a time?
                // this could be tricky
                if (_loadRequests.Count > 0)
                {
                    RegionNames region;

                    lock (_loadUnloadLock)
                    {
                        region = _loadRequests[0];
                        _loadRequests.RemoveAt(0);
                    }

                    ExecuteLoadRequest(region);
                }
                else if (_unloadRequests.Count > 0)
                {
                    RegionNames region;

                    lock (_loadUnloadLock)
                    {
                        region = _unloadRequests[0];
                        _unloadRequests.RemoveAt(0);
                    }

                    ExecuteUnloadRequest(region);
                }
                else if (_saveRequests.Count > 0)
                {
                    SaveLoadMessage msg;

                    lock (_saveRequests)
                    {
                        msg = _saveRequests[0];
                        _saveRequests.Clear();
                    }

                    ExecuteSaveRequest(msg);
                }
                else if (_preferenceRequests.Count > 0)
                {
                    PreferenceData data;

                    lock (_preferenceRequests)
                    {
                        data = _preferenceRequests[0];
                        _preferenceRequests.Clear();
                    }

                    ExecutePreferenceRequest(data);
                }
                else if (_fileLoadRequest != -1)
                {
                    ExecuteFileLoadRequest(_fileLoadRequest);
                }
            }
        }

        public bool AreNoRequests()
        {
            if (_loadRequests.Count == 0 &&
                _unloadRequests.Count == 0 &&
                _saveRequests.Count == 0 &&
                _preferenceRequests.Count == 0 &&
                _fileLoadRequest == -1)
                return true;

            return false;
        }

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
            lock (_loadUnloadLock)
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

        private void ExecuteUnloadRequest(RegionNames name)
        {

        }

        private void ExecuteLoadRequest(RegionNames region)
        {

        }

        private void ExecuteSaveRequest(SaveLoadMessage msg)
        {
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
            _saveLoadHandler.SavePreferenceData(data);
        }

        private void ExecuteFileLoadRequest(int fileNumber)
        {
            // what do we even do here?
            // i guess we replace the current save data with the one that is saved

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
