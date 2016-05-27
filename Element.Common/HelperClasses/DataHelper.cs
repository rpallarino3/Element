using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Data;

namespace Element.Common.HelperClasses
{
    public static class DataHelper
    {

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

        public static PreferenceData PreferenceData { get; set; }
        public static SaveData File0SaveData { get; set; }
        public static SaveData File1SaveData { get; set; }
        public static SaveData File2SaveData { get; set; }
    }
}
