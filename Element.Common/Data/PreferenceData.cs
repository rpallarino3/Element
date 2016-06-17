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
            Functions = new List<ControlFunctions>();
            Keybindings = new List<List<Keys>>();
            ButtonBindings = new List<List<Buttons>>();
        }

        public PreferenceData Copy()
        {
            var data = new PreferenceData();

            var functions = new List<ControlFunctions>(Functions);
            var keybindings = new List<List<Keys>>();
            var buttonBindings = new List<List<Buttons>>();

            foreach (var item in Keybindings)
            {
                var list = new List<Keys>(item);
                keybindings.Add(list);
            }

            foreach (var item in ButtonBindings)
            {
                var list = new List<Buttons>(item);
                buttonBindings.Add(list);
            }

            data.Functions = functions;
            data.Keybindings = keybindings;
            data.ButtonBindings = buttonBindings;

            data.File0Info = File0Info.Copy();
            data.File1Info = File1Info.Copy();
            data.File2Info = File2Info.Copy();

            data.Volume = Volume;
            data.Resolution = Resolution;

            return data;
        }

        public List<ControlFunctions> Functions { get; set; }
        public List<List<Keys>> Keybindings { get; set; }
        public List<List<Buttons>> ButtonBindings { get; set; }

        public SaveFileInfo File0Info { get; set; }
        public SaveFileInfo File1Info { get; set; }
        public SaveFileInfo File2Info { get; set; }

        public int Volume { get; set; }
        // don't let them set the resolution greater than the size of their monitor or smaller than 900 x 600? maybe 1280 x 720
        public Resolutions Resolution { get; set; } // validate this when you try to set resolution
    }
}
