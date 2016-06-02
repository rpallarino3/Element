using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Element.Common.Enumerations.Environment;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Instructions;

namespace Element.Common.Data
{
    public class SaveData
    {

        public SaveData Copy()
        {
            return new SaveData();
        }

        public Vector2 PlayerLocation { get; set; }
        public int PlayerLevel { get; set; }
        public Directions PlayerFacingDirection { get; set; }
        public RegionNames PlayerRegion { get; set; }
        public int PlayerZone { get; set; }

        public SaveFileInfo FileInfo { get; set; }
        public Dictionary<RegionNames, List<ObjectInstruction>> RegionInstructions { get; set; }
        public List<ObjectInstruction> ZoneInstructions { get; set; }
    }
}
