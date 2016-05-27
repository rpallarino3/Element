using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;
using Element.Common.HelperClasses;
using Element.Common.Messages;
using Element.Common.Misc;
using Element.Common.Data;

namespace Element.ResourceManagement
{
    public class ResourceManager
    {
        private IServiceProvider _serviceProvider;
        private string _rootDirectory;

        private MenuResourceManager _menuResourceManager;

        private Dictionary<RegionNames, ContentManager> _regionContentManagers;
        private Dictionary<RegionNames, Dictionary<int, Texture2D>> _regionSceneryTextures;
        private Dictionary<RegionNames, Dictionary<int, Texture2D>> _regionNpcTextures;
        private Dictionary<RegionNames, Dictionary<int, Texture2D>> _regionObjectTextures;

        private ContentManager _transRegionNpcContentManager;
        private Dictionary<int, Texture2D> _transRegionNpcTextures;

        private ContentManager _playerContentManager;

        private SaveLoadHandler _saveLoadHandler;
        private BackgroundThread _backgroundThread;

        public ResourceManager(IServiceProvider serviceProvider, string rootDirectory)
        {
            _serviceProvider = serviceProvider;
            _rootDirectory = rootDirectory;

            _saveLoadHandler = new SaveLoadHandler();

            _transRegionNpcContentManager = new ContentManager(_serviceProvider, rootDirectory);
            _playerContentManager = new ContentManager(_serviceProvider, rootDirectory);

            _regionContentManagers = new Dictionary<RegionNames, ContentManager>();
            _regionSceneryTextures = new Dictionary<RegionNames, Dictionary<int, Texture2D>>();
            _regionNpcTextures = new Dictionary<RegionNames, Dictionary<int, Texture2D>>();
            _regionObjectTextures = new Dictionary<RegionNames, Dictionary<int, Texture2D>>();
            _transRegionNpcTextures = new Dictionary<int, Texture2D>();


            _backgroundThread = new BackgroundThread(_saveLoadHandler);
            _backgroundThread.AssetsLoaded += AddLoadedTextures;
            _backgroundThread.AssetsUnloaded += RemoveLoadedTextures;

            var t = new Thread(() => _backgroundThread.Loop());
            t.Start();
        }

        private void AddLoadedTextures(AssetsLoadedEventArgs e)
        {
            foreach (var region in e.NewSceneryTextures.Keys)
            {
                _regionContentManagers[region] = new ContentManager(_serviceProvider, _rootDirectory);
                _regionSceneryTextures[region] = e.NewSceneryTextures[region];
                _regionNpcTextures[region] = e.NewNpcTextures[region];
                _regionObjectTextures[region] = e.NewObjectTextures[region];
            }
        }

        private void RemoveLoadedTextures(AssetsUnloadedEventArgs e)
        {
            foreach (var region in e.RegionsUnloaded)
            {
                _regionSceneryTextures.Remove(region);
                _regionNpcTextures.Remove(region);
                _regionObjectTextures.Remove(region);

                if (_regionContentManagers.ContainsKey(region))
                {
                    _regionContentManagers[region].Unload();
                    _regionContentManagers.Remove(region);
                }
            }
        }

        public void LoadStaticContent()
        {

        }

        public void RequestSave(SaveLoadMessage msg)
        {
        }

        public void EraseFile(int fileNumber)
        {
            var msg = new SaveLoadMessage();

            msg.Data = DataHelper.GetDataCopyFromFileNumber(fileNumber);
            msg.FileNumber = fileNumber;
            msg.Erase = true;

            _backgroundThread.AddSaveRequest(msg);
        }

        public void Shutdown()
        {
            _backgroundThread.Shutdown();
        }

        #region Preference Methods

        // should really look at all these methods and see if we want to move them to the helper

        public void LoadFilesAndUpdatePreferenceData()
        {
            DataHelper.PreferenceData = _saveLoadHandler.LoadPreferenceData(); // maybe move this all into one method

            if (DataHelper.PreferenceData == null)
            {
                DataHelper.PreferenceData = new PreferenceData();
                PreferenceValidator.AddDefaultKeybindsToDictionaries(DataHelper.PreferenceData.Keybindings, DataHelper.PreferenceData.ButtonBindings);
                DataHelper.PreferenceData.Volume = PreferenceValidator.DefaultVolume;
                DataHelper.PreferenceData.Resolution = PreferenceValidator.DefaultResolution;
            }
            else
            {
                PreferenceValidator.ValidatePreferenceData(DataHelper.PreferenceData);
            }

            _saveLoadHandler.LoadFiles();

            DataHelper.File0SaveData = _saveLoadHandler.File0Data != null ? _saveLoadHandler.File0Data.Copy() : new SaveData();
            DataHelper.File1SaveData = _saveLoadHandler.File1Data != null ? _saveLoadHandler.File1Data.Copy() : new SaveData();
            DataHelper.File2SaveData = _saveLoadHandler.File2Data != null ? _saveLoadHandler.File2Data.Copy() : new SaveData();

            DataHelper.PreferenceData.File0Info = DataHelper.File0SaveData.FileInfo;
            DataHelper.PreferenceData.File1Info = DataHelper.File1SaveData.FileInfo;
            DataHelper.PreferenceData.File2Info = DataHelper.File2SaveData.FileInfo;

            // shouldn't really have to save anything after this since we are going to do this every time anyways
        }

        public void ResetPreferenceData()
        {
            var prefData = new PreferenceData();
            PreferenceValidator.AddDefaultKeybindsToDictionaries(prefData.Keybindings, prefData.ButtonBindings);
            prefData.Volume = PreferenceValidator.DefaultVolume;
            prefData.Resolution = PreferenceValidator.DefaultResolution;

            prefData.File0Info = DataHelper.PreferenceData.File0Info;
            prefData.File1Info = DataHelper.PreferenceData.File1Info;
            prefData.File2Info = DataHelper.PreferenceData.File2Info;

            DataHelper.PreferenceData = prefData;
            _backgroundThread.AddPreferenceRequest(DataHelper.PreferenceData.Copy());
        }

        public void ResetPreferenceKeybinds()
        {
            PreferenceValidator.AddDefaultKeybindsToDictionaries(DataHelper.PreferenceData.Keybindings, DataHelper.PreferenceData.ButtonBindings);
            _backgroundThread.AddPreferenceRequest(DataHelper.PreferenceData.Copy());
        }

        public void UpdatePreferenceKeybindData(Dictionary<ControlFunctions, Control> controls)
        {
            // we might need to validate the keybinds here?

            if (controls == null)
                return;

            var keys = new Dictionary<ControlFunctions, List<Keys>>();
            var buttons = new Dictionary<ControlFunctions, List<Buttons>>();

            foreach (var function in controls.Keys)
            {
                var functionKeys = controls[function].Keys;
                var functionButtons = controls[function].Buttons;

                keys.Add(function, functionKeys);
                buttons.Add(function, functionButtons);
            }

            DataHelper.PreferenceData.Keybindings = keys;
            DataHelper.PreferenceData.ButtonBindings = buttons;
            
            _backgroundThread.AddPreferenceRequest(DataHelper.PreferenceData.Copy());
        }

        public void UpdatePreferenceResolutionData(Resolutions resolution)
        {
            DataHelper.PreferenceData.Resolution = resolution;
            _backgroundThread.AddPreferenceRequest(DataHelper.PreferenceData.Copy());
        }

        public void UpdatePreferenceVolumeData(bool up)
        {
            if (up)
                DataHelper.PreferenceData.Volume++;
            else
                DataHelper.PreferenceData.Volume--;

            UpdatePreferenceVolumeData(DataHelper.PreferenceData.Volume);
        }

        public void UpdatePreferenceVolumeData(int volume)
        {
            if (volume > 10)
                volume = 10;
            else if (volume < 0)
                volume = 0;

            DataHelper.PreferenceData.Volume = volume;

            _backgroundThread.AddPreferenceRequest(DataHelper.PreferenceData.Copy());
        }

        public void UpdatePreferenceSaveFileInfo(int file, SaveFileInfo info)
        {
            if (file == 0)
                DataHelper.PreferenceData.File0Info = info;
            else if (file == 1)
                DataHelper.PreferenceData.File1Info = info;
            else if (file == 2)
                DataHelper.PreferenceData.File2Info = info;

            _backgroundThread.AddPreferenceRequest(DataHelper.PreferenceData.Copy());
        }

        #endregion

        #region Events

        // put all the events that would be in the bgthread in here

        #endregion
    }
}
