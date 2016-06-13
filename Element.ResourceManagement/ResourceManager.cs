using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.NPCs;
using Element.Common.Enumerations.Sound;
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

        private Dictionary<RegionNames, RegionContent> _regionContent;
        private Dictionary<NpcNames, CrossRegionNpcContent> _crossRegionNpcContent;
        private Dictionary<SoundName, CrossRegionSoundContent> _crossRegionSoundContent;

        private List<RegionNames> _contentToRemove;
        private List<RegionContent> _contentToAdd;

        private List<NpcNames> _crossRegionNpcContentToRemove;
        private List<SoundName> _crossRegionSoundContentToRemove;
        private List<CrossRegionContent> _crossRegionContentToAdd;

        private ContentManager _playerContentManager;

        private SaveLoadHandler _saveLoadHandler;
        private BackgroundThread _backgroundThread;

        public ResourceManager(IServiceProvider serviceProvider, string rootDirectory)
        {
            _serviceProvider = serviceProvider;
            _rootDirectory = rootDirectory;

            _menuResourceManager = new MenuResourceManager(_serviceProvider, rootDirectory);
            _saveLoadHandler = new SaveLoadHandler();
            
            _playerContentManager = new ContentManager(_serviceProvider, rootDirectory);

            _regionContent = new Dictionary<RegionNames, RegionContent>();
            _crossRegionNpcContent = new Dictionary<NpcNames, CrossRegionNpcContent>();
            _crossRegionSoundContent = new Dictionary<SoundName, CrossRegionSoundContent>();

            _contentToRemove = new List<RegionNames>();
            _contentToAdd = new List<RegionContent>();
            _crossRegionNpcContentToRemove = new List<NpcNames>();
            _crossRegionSoundContentToRemove = new List<SoundName>();
            _crossRegionContentToAdd = new List<CrossRegionContent>();

            _backgroundThread = new BackgroundThread(_saveLoadHandler, _serviceProvider, _rootDirectory);
            _backgroundThread.SaveInitiated += SaveStarted;
            _backgroundThread.SaveCompleted += SaveDone;
            _backgroundThread.AssetsLoaded += AssetsLoaded;
            _backgroundThread.AssetsUnloaded += AssetsUnloaded;

            var t = new Thread(() => _backgroundThread.Loop());
            t.Start();
        }

        public void LoadStaticContent()
        {
            _menuResourceManager.LoadContent();
        }

        public void Shutdown()
        {
            _menuResourceManager.UnloadContent();
            _backgroundThread.Shutdown();
        }

        public bool IsBackgroundThreadClear()
        {
            return _backgroundThread.AreNoRequests();
        }

        public void CheckLoad()
        {
            var contentToAdd = new List<RegionContent>();

            lock (_contentToAdd)
            {
                contentToAdd.AddRange(_contentToAdd);
                _contentToAdd.Clear();
            }

            foreach (var content in contentToAdd)
            {
                _regionContent.Add(content.Region, content);
                contentToAdd.Remove(content);
            }

            var crossRegionContentToAdd = new List<CrossRegionContent>();

            lock (_crossRegionContentToAdd)
            {
                crossRegionContentToAdd.AddRange(_crossRegionContentToAdd);
                _crossRegionContentToAdd.Clear();
            }

            foreach (var content in crossRegionContentToAdd)
            {
                if (content is CrossRegionNpcContent)
                {
                    var npcContent = content as CrossRegionNpcContent;
                    _crossRegionNpcContent.Add(npcContent.Id, npcContent);
                    crossRegionContentToAdd.Remove(npcContent);
                }
                else if (content is CrossRegionSoundContent)
                {
                    var soundContent = content as CrossRegionSoundContent;
                    _crossRegionSoundContent.Add(soundContent.Id, soundContent);
                    crossRegionContentToAdd.Remove(soundContent);
                }
            }
        }

        public void CheckUnload()
        {
            var contentToRemove = new List<RegionNames>();

            lock (_contentToRemove)
            {
                contentToRemove.AddRange(_contentToRemove);
                _contentToRemove.Clear();
            }

            foreach (var region in contentToRemove)
            {
                if (_regionContent.ContainsKey(region))
                {
                    _regionContent[region].ContentManager.Unload();
                    _regionContent[region].ContentManager.Dispose();
                    _regionContent.Remove(region);
                }

                contentToRemove.Remove(region);
            }

            var crossRegionNpcContentToRemove = new List<NpcNames>();

            lock (_crossRegionNpcContentToRemove)
            {
                crossRegionNpcContentToRemove.AddRange(_crossRegionNpcContentToRemove);
                _crossRegionNpcContentToRemove.Clear();
            }

            foreach (var item in crossRegionNpcContentToRemove)
            {
                if (_crossRegionNpcContent.ContainsKey(item))
                {
                    _crossRegionNpcContent[item].Dispose();
                    _crossRegionNpcContent.Remove(item);
                }
            }

            var crossRegionSoundContentToRemove = new List<SoundName>();

            lock (_crossRegionSoundContentToRemove)
            {
                crossRegionSoundContentToRemove.AddRange(_crossRegionSoundContentToRemove);
                _crossRegionSoundContentToRemove.Clear();
            }

            foreach (var item in crossRegionSoundContentToRemove)
            {
                if (_crossRegionSoundContent.ContainsKey(item))
                {
                    _crossRegionSoundContent[item].Dispose();
                    _crossRegionSoundContent.Remove(item);
                }
            }
        }

        #region Save Data

        public void RequestSave(SaveLoadMessage msg)
        {
            _backgroundThread.AddSaveRequest(msg);
        }
        
        public void EraseFile(int fileNumber)
        {
            var msg = new SaveLoadMessage();

            msg.Data = DataHelper.GetDataCopyFromFileNumber(fileNumber);
            msg.FileNumber = fileNumber;
            msg.Erase = true;

            DataHelper.EraseDataForFile(fileNumber);

            _backgroundThread.AddSaveRequest(msg);
            _backgroundThread.AddPreferenceRequest(DataHelper.PreferenceData.Copy());
        }

        private void SaveStarted()
        {
            // this should resolve any timing issues from different threads raising the events
            SaveInitiated();
        }

        private void SaveDone()
        {
            SaveCompleted();
        }

        #endregion

        #region Region Load/Unload

        public void RequestRegionLoad(List<RegionNames> regions)
        {
            _backgroundThread.AddLoadRequests(regions);
        }

        public void RequestRegionUnload(List<RegionNames> regions)
        {
            _backgroundThread.AddUnloadRequests(regions);
        }

        public void RequestRegionLoadUnload(List<RegionNames> regionsToLoad, List<RegionNames> regionsToUnload)
        {
            _backgroundThread.AddLoadAndUnloadRequests(regionsToLoad, regionsToUnload);
        }
        
        private void AssetsLoaded(AssetsLoadedEventArgs e)
        {
            AddLoadedContent(e);
            RegionsLoaded(e);
        }

        private void AssetsUnloaded(AssetsUnloadedEventArgs e)
        {
            // need to make sure that the regions are removed BEFORE the textures are removed so that we don't try to draw on non existant texture
            // maybe check to see if there is a content manager for that region before we try to draw anything for that region?
            RegionsUnloaded(e);
            RemoveUnloadedContent(e);
        }

        private void AddLoadedContent(AssetsLoadedEventArgs e)
        {
            lock (_contentToAdd)
            {
                _contentToAdd.AddRange(e.RegionContent);
            }

            lock (_crossRegionContentToAdd)
            {
                _crossRegionContentToAdd.AddRange(e.CrossRegionContent);
            }
        }

        private void RemoveUnloadedContent(AssetsUnloadedEventArgs e)
        {
            lock (_contentToRemove)
            {
                _contentToRemove.AddRange(e.RegionsUnloaded);
            }

            lock (_crossRegionNpcContentToRemove)
            {
                _crossRegionNpcContentToRemove.AddRange(e.CrossRegionNpcsUnloaded);
            }
            
            lock (_crossRegionSoundContentToRemove)
            {
                _crossRegionSoundContentToRemove.AddRange(e.CrossRegionSoundUnloaded);
            }
        }

        #endregion

        #region Preference Methods

        // should really look at all these methods and see if we want to move them to the helper

        // i think this method is ok because it is called before any other thread should access the data
        public void LoadFilesAndUpdatePreferenceData()
        {
            DataHelper.PreferenceData = _saveLoadHandler.LoadPreferenceData(); // maybe move this all into one method

            if (DataHelper.PreferenceData == null)
            {
                DataHelper.PreferenceData = new PreferenceData();
                PreferenceValidator.AddDefaultKeybindsToDictionaries();
                DataHelper.PreferenceData.Volume = PreferenceValidator.DefaultVolume;
                DataHelper.PreferenceData.Resolution = PreferenceValidator.DefaultResolution;
            }
            else
            {
                PreferenceValidator.ValidatePreferenceData();
            }

            _saveLoadHandler.LoadFiles();

            DataHelper.File0SaveData = _saveLoadHandler.File0Data != null ? _saveLoadHandler.File0Data.Copy() : DataHelper.CreateStartingSaveData();
            DataHelper.File1SaveData = _saveLoadHandler.File1Data != null ? _saveLoadHandler.File1Data.Copy() : DataHelper.CreateStartingSaveData();
            DataHelper.File2SaveData = _saveLoadHandler.File2Data != null ? _saveLoadHandler.File2Data.Copy() : DataHelper.CreateStartingSaveData();

            DataHelper.PreferenceData.File0Info = DataHelper.File0SaveData.FileInfo;
            DataHelper.PreferenceData.File1Info = DataHelper.File1SaveData.FileInfo;
            DataHelper.PreferenceData.File2Info = DataHelper.File2SaveData.FileInfo;

            DataHelper.InitialCopy();
        }

        public void ResetPreferenceData()
        {
            var prefData = new PreferenceData();
            PreferenceValidator.AddDefaultKeybindsToDictionaries();
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
            PreferenceValidator.AddDefaultKeybindsToDictionaries();
            _backgroundThread.AddPreferenceRequest(DataHelper.PreferenceData.Copy());
        }

        public void UpdatePreferenceKeybindData(Dictionary<ControlFunctions, Control> controls)
        {
            // we might need to validate the keybinds here?

            if (controls == null)
                return;

            var keys = new List<KeyValuePair<ControlFunctions, List<Keys>>>();
            var buttons = new List<KeyValuePair<ControlFunctions, List<Buttons>>>();

            foreach (var function in controls.Keys)
            {
                var functionKeys = new List<Keys>(controls[function].Keys);
                var functionButtons = new List<Buttons>(controls[function].Buttons);

                keys.Add(new KeyValuePair<ControlFunctions, List<Keys>>(function, functionKeys));
                buttons.Add(new KeyValuePair<ControlFunctions, List<Buttons>>(function, functionButtons));
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

        public MenuResourceManager MenuResourceManager { get { return _menuResourceManager; } }

        #region Events

        public event AssetsLoadedEvent RegionsLoaded;
        public event AssetsUnloadedEvent RegionsUnloaded;
        
        public event SaveInitiatedEvent SaveInitiated;
        public event SaveCompletedEvent SaveCompleted;

        #endregion
    }
}
