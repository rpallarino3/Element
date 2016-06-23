using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Data;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;
using Element.Common.GameObjects.Npcs;

namespace Element.Common.HelperClasses
{
    public static class DataHelper
    {
        // need to think of a way to make this thread save without having to lock on this at all times
        private static readonly string NEW_GAME = "New Game";

        private static SaveData _file0SaveData;
        private static SaveData _file1SaveData;
        private static SaveData _file2SaveData;
        private static int _currentFileNumber;
        private static PreferenceData _preferenceData;

        #region Save Data

        public static SaveData GetDataFromFileNumber(int fileNumber)
        {
            if (fileNumber == 0)
                return _file0SaveData;
            else if (fileNumber == 1)
                return _file1SaveData;
            else if (fileNumber == 2)
                return _file2SaveData;

            return new SaveData();
        }

        public static SaveData CreateStartingSaveData()
        {
            var saveData = new SaveData();

            saveData.PlayerRegion = RegionNames.Test0;
            saveData.PlayerZone = 0;
            saveData.PlayerLocation = new Vector2(22, 13);
            saveData.PlayerLevel = 0;
            saveData.PlayerFacingDirection = Directions.Down;

            return saveData;
        }
                
        public static void EraseDataForFile(int fileNumber)
        {
            if (fileNumber == 0)
            {
                if (_file0SaveData == null)
                    return;

                lock (_file0SaveData)
                    _file0SaveData = new SaveData();

                _preferenceData.File0Info = new SaveFileInfo();
            }
            else if (fileNumber == 1)
            {
                lock (_file1SaveData)
                    _file1SaveData = new SaveData();

                _preferenceData.File1Info = new SaveFileInfo();
            }
            else if (fileNumber == 2)
            {
                lock (_file2SaveData)
                    _file2SaveData = new SaveData();

                _preferenceData.File2Info = new SaveFileInfo();
            }
        }
        
        public static SaveData GetSaveDataCopyForFileNumber(int fileNumber)
        {
            if (fileNumber == 0)
            {
                if (_file0SaveData == null)
                    return new SaveData();

                lock (_file0SaveData)
                    return _file0SaveData.Copy();
            }
            else if (fileNumber == 1)
            {
                if (_file1SaveData == null)
                    return new SaveData();

                lock (_file1SaveData)
                    return _file1SaveData.Copy();
            }
            else if (fileNumber == 2)
            {
                if (_file2SaveData == null)
                    return new SaveData();

                lock (_file2SaveData)
                    return _file2SaveData.Copy();
            }

            return new SaveData();
        }

        public static SaveData GetCopySaveDataForCurrentFile()
        {
            if (_currentFileNumber == 0)
            {
                if (_file0SaveData == null)
                    return new SaveData();

                lock (_file0SaveData)
                    return _file0SaveData.Copy();
            }
            else if (_currentFileNumber == 1)
            {
                if (_file1SaveData == null)
                    return new SaveData();

                lock (_file1SaveData)
                    return _file1SaveData.Copy();
            }
            else if (_currentFileNumber == 2)
            {
                if (_file2SaveData == null)
                    return new SaveData();

                lock (_file2SaveData)
                    return _file2SaveData.Copy();
            }

            return new SaveData();
        }

        #endregion

        #region Pref Data

        public static Vector2 GetVector2FromResolution()
        {
            if (_preferenceData.Resolution == Resolutions.r960x540)
                return new Vector2(960, 540);
            else if (_preferenceData.Resolution == Resolutions.r1280x720)
                return new Vector2(1280, 720);
            else if (_preferenceData.Resolution == Resolutions.r1600x900)
                return new Vector2(1600, 900);
            else
                return new Vector2(1920, 1080);
        }

        // not really sure why this is in here but ok
        public static Vector2 GetScreenRatioFromResolution()
        {
            float x = GetVector2FromResolution().X / 1280f;
            float y = GetVector2FromResolution().Y / 720f;

            return new Vector2(x, y);
        }

        public static string GetStringFromFileInfo(int index)
        {
            if (index == 0)
            {
                if (_preferenceData.File0Info.LastRegion == RegionNames.None)
                    return NEW_GAME;

                return _preferenceData.File0Info.Name + System.Environment.NewLine + _preferenceData.File0Info.LastDate.ToString();
            }
            else if (index == 1)
            {
                if (_preferenceData.File1Info.LastRegion == RegionNames.None)
                    return NEW_GAME;

                return _preferenceData.File1Info.Name + System.Environment.NewLine + _preferenceData.File1Info.LastDate.ToString();
            }
            else if (index == 2)
            {
                if (_preferenceData.File2Info.LastRegion == RegionNames.None)
                    return NEW_GAME;

                return _preferenceData.File2Info.Name + System.Environment.NewLine + _preferenceData.File2Info.LastDate.ToString();
            }
            else
                return NEW_GAME;
        }

        #endregion

        public static void SaveCrossRegionNpcState(Npc npc)
        {
            // do shit here
            // need to make sure to lock on save data whenever we want to add to it
        }

        #region Properties

        public static int CurrentFileNumber
        {
            get { return _currentFileNumber; }
            set { _currentFileNumber = value; }
        }

        public static PreferenceData PreferenceData
        {
            get { return _preferenceData; }
            set { _preferenceData = value; }
        }

        // might want to restrict access to these

        public static SaveData File0SaveData
        {
            get { return _file0SaveData; }
            set { _file0SaveData = value; }
        }

        public static SaveData File1SaveData
        {
            get { return _file1SaveData; }
            set { _file1SaveData = value; }
        }

        public static SaveData File2SaveData
        {
            get { return _file2SaveData; }
            set { _file2SaveData = value; }
        }

        #endregion
    }
}
