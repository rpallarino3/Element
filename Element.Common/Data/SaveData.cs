using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.Instructions;

namespace Element.Common.Data
{
    public class SaveData
    {

        public SaveData Copy()
        {
            return new SaveData();
        }

        public SaveFileInfo FileInfo { get; set; }
        public Dictionary<RegionNames, List<ObjectInstruction>> RegionInstructions { get; set; }
        public List<ObjectInstruction> ZoneInstructions { get; set; }
    }
}
