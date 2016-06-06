using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;

namespace Element.Common.Data
{
    public class SaveFileInfo
    {
        public SaveFileInfo()
        {
            Name = string.Empty;
        }

        public string Name { get; set; }
        public RegionNames LastRegion { get; set; }
        public DateTime LastDate { get; set; }

        public SaveFileInfo Copy()
        {
            var saveFileInfo = new SaveFileInfo();
            saveFileInfo.Name = string.Copy(Name);
            saveFileInfo.LastRegion = LastRegion;
            saveFileInfo.LastDate = LastDate;
            return saveFileInfo;
        }
    }
}
