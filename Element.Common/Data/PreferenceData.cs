using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Element.Common.Enumerations.GameBasics;

namespace Element.Common.Data
{
    public class PreferenceData
    {
        public PreferenceData()
        {
            Keybindings = new List<KeyValuePair<ControlFunctions, List<Keys>>>();
            ButtonBindings = new List<KeyValuePair<ControlFunctions, List<Buttons>>>();
        }

        public PreferenceData Copy()
        {
            var data = new PreferenceData();
            data.Keybindings = new List<KeyValuePair<ControlFunctions, List<Keys>>>(Keybindings); // this doesn't work exactly
            data.ButtonBindings = new List<KeyValuePair<ControlFunctions, List<Buttons>>>(ButtonBindings);

            data.File0Info = File0Info.Copy();
            data.File1Info = File1Info.Copy();
            data.File2Info = File2Info.Copy();

            data.Volume = Volume;
            data.Resolution = Resolution;

            return data;
        }

        public List<KeyValuePair<ControlFunctions, List<Keys>>> Keybindings { get; set; }
        public List<KeyValuePair<ControlFunctions, List<Buttons>>> ButtonBindings { get; set; }

        public SaveFileInfo File0Info { get; set; }
        public SaveFileInfo File1Info { get; set; }
        public SaveFileInfo File2Info { get; set; }

        public int Volume { get; set; }
        // don't let them set the resolution greater than the size of their monitor or smaller than 900 x 600? maybe 1280 x 720
        public Resolutions Resolution { get; set; } // validate this when you try to set resolution
    }
}
