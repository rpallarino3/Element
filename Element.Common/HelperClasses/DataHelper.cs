using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Data;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;

namespace Element.Common.HelperClasses
{
    public static class DataHelper
    {
        private static readonly string NEW_GAME = "New Game";

        public static SaveData GetDataFromFileNumber(int fileNumber)
        {
            if (fileNumber == 0)
                return File0SaveData;
            else if (fileNumber == 1)
                return File1SaveData;
            else if (fileNumber == 2)
                return File2SaveData;

            return new SaveData();
        }

        public static SaveData GetDataCopyFromFileNumber(int fileNumber)
        {
            if (fileNumber == 0 && File0SaveData != null)
                return File0SaveData.Copy();
            else if (fileNumber == 1 && File1SaveData != null)
                return File1SaveData.Copy();
            else if (fileNumber == 2 && File2SaveData != null)
                return File2SaveData.Copy();

            return new SaveData();
        }

        public static void EraseDataForFile(int fileNumber)
        {
            if (fileNumber == 0)
            {
                File0SaveData = new SaveData();
                PreferenceData.File0Info = new SaveFileInfo();
            }
            else if (fileNumber == 1)
            {
                File1SaveData = new SaveData();
                PreferenceData.File1Info = new SaveFileInfo();
            }
            else if (fileNumber == 2)
            {
                File2SaveData = new SaveData();
                PreferenceData.File2Info = new SaveFileInfo();
            }
        }

        public static Vector2 GetVector2FromResolution()
        {
            if (PreferenceData.Resolution == Resolutions.r960x540)
                return new Vector2(960, 540);
            else if (PreferenceData.Resolution == Resolutions.r1280x720)
                return new Vector2(1280, 720);
            else if (PreferenceData.Resolution == Resolutions.r1600x900)
                return new Vector2(1600, 900);
            else
                return new Vector2(1920, 1080);
        }

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
                if (PreferenceData.File0Info.LastRegion == RegionNames.None)
                    return NEW_GAME;

                return PreferenceData.File0Info.Name + System.Environment.NewLine + PreferenceData.File0Info.LastDate.ToString();
            }
            else if (index == 1)
            {
                if (PreferenceData.File1Info.LastRegion == RegionNames.None)
                    return NEW_GAME;

                return PreferenceData.File1Info.Name + System.Environment.NewLine + PreferenceData.File1Info.LastDate.ToString();
            }
            else if (index == 2)
            {
                if (PreferenceData.File2Info.LastRegion == RegionNames.None)
                    return NEW_GAME;

                return PreferenceData.File2Info.Name + System.Environment.NewLine + PreferenceData.File2Info.LastDate.ToString();
            }
            else
                return NEW_GAME;
        }

        public static int CurrentFileNumber { get; set; }
        public static PreferenceData PreferenceData { get; set; }
        public static SaveData File0SaveData { get; set; }
        public static SaveData File1SaveData { get; set; }
        public static SaveData File2SaveData { get; set; }
    }
}
