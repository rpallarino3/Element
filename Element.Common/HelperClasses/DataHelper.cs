using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Data;
using Element.Common.Enumerations.GameBasics;

namespace Element.Common.HelperClasses
{
    public static class DataHelper
    {
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

        public static PreferenceData PreferenceData { get; set; }
        public static SaveData File0SaveData { get; set; }
        public static SaveData File1SaveData { get; set; }
        public static SaveData File2SaveData { get; set; }
    }
}
