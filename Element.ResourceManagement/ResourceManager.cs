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
    public static class ResourceManager
    {
        private static IServiceProvider _serviceProvider;
        private static string _rootDirectory;
        
        private static Dictionary<RegionNames, RegionContent> _regionContent;
        private static Dictionary<NpcNames, CrossRegionNpcContent> _crossRegionNpcContent;
        private static Dictionary<SoundName, CrossRegionSoundContent> _crossRegionSoundContent;

        private static List<RegionNames> _contentToRemove;
        private static List<RegionContent> _contentToAdd;

        private static List<NpcNames> _crossRegionNpcContentToRemove;
        private static List<SoundName> _crossRegionSoundContentToRemove;
        private static List<CrossRegionContent> _crossRegionContentToAdd;

        private static ContentManager _playerContentManager;
        private static BackgroundThread _backgroundThread;

        // we technically don't have to dispose of content managers, just unload the assets we don't want and the content manager is resusable. should unload then dispose if we want to get rid of content manager for good though
        static ResourceManager()
        {
            _regionContent = new Dictionary<RegionNames, RegionContent>();
            _crossRegionNpcContent = new Dictionary<NpcNames, CrossRegionNpcContent>();
            _crossRegionSoundContent = new Dictionary<SoundName, CrossRegionSoundContent>();

            _contentToRemove = new List<RegionNames>();
            _contentToAdd = new List<RegionContent>();
            _crossRegionNpcContentToRemove = new List<NpcNames>();
            _crossRegionSoundContentToRemove = new List<SoundName>();
            _crossRegionContentToAdd = new List<CrossRegionContent>();
        }

        public static void PassProviderAndRootDirectory(IServiceProvider serviceProvider, string rootDirectory)
        {
            _serviceProvider = serviceProvider;
            _rootDirectory = rootDirectory;
            _playerContentManager = new ContentManager(_serviceProvider, _rootDirectory);
            MenuResourceManager.PassProviderAndRootDirectory(serviceProvider, rootDirectory);

            _backgroundThread = new BackgroundThread(_serviceProvider, _rootDirectory);
            _backgroundThread.SaveInitiated += SaveStarted;
            _backgroundThread.SaveCompleted += SaveDone;
            _backgroundThread.AssetsLoaded += AssetsLoaded;
            _backgroundThread.AssetsUnloaded += AssetsUnloaded;
            var t = new Thread(() => _backgroundThread.Loop());
            t.Start();
        }

        public static void LoadStaticContent()
        {
            MenuResourceManager.LoadContent();
        }

        public static void Shutdown()
        {
            MenuResourceManager.UnloadContent();
            _backgroundThread.Shutdown();
        }

        public static bool IsBackgroundThreadClear()
        {
            return _backgroundThread.AreNoRequests();
        }

        public static void CheckLoad()
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
                }
                else if (content is CrossRegionSoundContent)
                {
                    var soundContent = content as CrossRegionSoundContent;
                    _crossRegionSoundContent.Add(soundContent.Id, soundContent);
                }
            }
            crossRegionContentToAdd.Clear();
        }

        public static void CheckUnload()
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

        public static void RequestSave(SaveLoadMessage msg)
        {
            _backgroundThread.AddSaveRequest(msg);
        }
        
        public static void EraseFile(int fileNumber)
        {
            var msg = new SaveLoadMessage();

            msg.Data = DataHelper.GetSaveDataCopyForFileNumber(fileNumber);
            msg.FileNumber = fileNumber;
            msg.Erase = true;

            DataHelper.EraseDataForFile(fileNumber);

            _backgroundThread.AddSaveRequest(msg);
            _backgroundThread.AddPreferenceRequest(DataHelper.PreferenceData.Copy());
        }

        private static void SaveStarted()
        {
            // this should resolve any timing issues from different threads raising the events
            SaveInitiated();
        }

        private static void SaveDone()
        {
            SaveCompleted();
        }

        #endregion

        #region Region Load/Unload

        public static void RequestRegionLoad(List<RegionNames> regions)
        {
            _backgroundThread.AddLoadRequests(regions);
        }

        public static void RequestRegionUnload(List<RegionNames> regions)
        {
            _backgroundThread.AddUnloadRequests(regions);
        }

        public static void RequestRegionLoadUnload(List<RegionNames> regionsToLoad, List<RegionNames> regionsToUnload)
        {
            _backgroundThread.AddLoadAndUnloadRequests(regionsToLoad, regionsToUnload);
        }
        
        private static void AssetsLoaded(AssetsLoadedEventArgs e)
        {
            AddLoadedContent(e);
            RegionsLoaded(e);
        }

        private static void AssetsUnloaded(AssetsUnloadedEventArgs e)
        {
            // need to make sure that the regions are removed BEFORE the textures are removed so that we don't try to draw on non existant texture
            // maybe check to see if there is a content manager for that region before we try to draw anything for that region?
            RegionsUnloaded(e);
            RemoveUnloadedContent(e);
        }

        private static void AddLoadedContent(AssetsLoadedEventArgs e)
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

        private static void RemoveUnloadedContent(AssetsUnloadedEventArgs e)
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
        public static void LoadFilesAndUpdatePreferenceData()
        {
            DataHelper.PreferenceData = SaveLoadHandler.LoadPreferenceData(); // something is not right here

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

            SaveLoadHandler.LoadFiles();

            DataHelper.File0SaveData = SaveLoadHandler.File0Data != null ? SaveLoadHandler.File0Data.Copy() : DataHelper.CreateStartingSaveData();
            DataHelper.File1SaveData = SaveLoadHandler.File1Data != null ? SaveLoadHandler.File1Data.Copy() : DataHelper.CreateStartingSaveData();
            DataHelper.File2SaveData = SaveLoadHandler.File2Data != null ? SaveLoadHandler.File2Data.Copy() : DataHelper.CreateStartingSaveData();

            DataHelper.PreferenceData.File0Info = DataHelper.File0SaveData.FileInfo;
            DataHelper.PreferenceData.File1Info = DataHelper.File1SaveData.FileInfo;
            DataHelper.PreferenceData.File2Info = DataHelper.File2SaveData.FileInfo;

            SaveLoadHandler.SavePreferenceData(DataHelper.PreferenceData.Copy());
        }

        public static void ResetPreferenceData()
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

        public static void ResetPreferenceKeybinds()
        {
            PreferenceValidator.AddDefaultKeybindsToDictionaries();
            _backgroundThread.AddPreferenceRequest(DataHelper.PreferenceData.Copy());
        }

        public static void UpdatePreferenceKeybindData(Dictionary<ControlFunctions, Control> controls)
        {
            // we might need to validate the keybinds here?

            if (controls == null)
                return;

            var functions = new List<ControlFunctions>();
            var keys = new List<List<Keys>>();
            var buttons = new List<List<Buttons>>();

            foreach (var function in controls.Keys)
            {
                var functionKeys = new List<Keys>(controls[function].Keys);
                var functionButtons = new List<Buttons>(controls[function].Buttons);

                functions.Add(function);
                keys.Add(functionKeys);
                buttons.Add(functionButtons);
            }

            DataHelper.PreferenceData.Functions = functions;
            DataHelper.PreferenceData.Keybindings = keys;
            DataHelper.PreferenceData.ButtonBindings = buttons;
            
            _backgroundThread.AddPreferenceRequest(DataHelper.PreferenceData.Copy());
        }

        public static void UpdatePreferenceResolutionData(Resolutions resolution)
        {
            DataHelper.PreferenceData.Resolution = resolution;
            _backgroundThread.AddPreferenceRequest(DataHelper.PreferenceData.Copy());
        }

        public static void UpdatePreferenceVolumeData(bool up)
        {
            if (up)
                DataHelper.PreferenceData.Volume++;
            else
                DataHelper.PreferenceData.Volume--;

            UpdatePreferenceVolumeData(DataHelper.PreferenceData.Volume);
        }

        public static void UpdatePreferenceVolumeData(int volume)
        {
            if (volume > 10)
                volume = 10;
            else if (volume < 0)
                volume = 0;

            DataHelper.PreferenceData.Volume = volume;

            _backgroundThread.AddPreferenceRequest(DataHelper.PreferenceData.Copy());
        }

        public static void UpdatePreferenceSaveFileInfo(int file, SaveFileInfo info)
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
        
        public static Dictionary<RegionNames, RegionContent> RegionContent { get { return _regionContent; } }
        public static Dictionary<NpcNames, CrossRegionNpcContent> CrossRegionNpcContent { get { return _crossRegionNpcContent; } }
        public static Dictionary<SoundName, CrossRegionSoundContent> CrossRegionSoundContenet { get { return _crossRegionSoundContent; } }

        #region Events

        public static event AssetsLoadedEvent RegionsLoaded;
        public static event AssetsUnloadedEvent RegionsUnloaded;
        
        public static event SaveInitiatedEvent SaveInitiated;
        public static event SaveCompletedEvent SaveCompleted;

        #endregion
    }
}
