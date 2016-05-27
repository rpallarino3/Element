using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Element.Common.Enumerations.GameBasics;
using Element.Common.Enumerations.Menu;

namespace Element.Common.Menus
{
    public abstract class MenuPageEventArgs { }

    public class CloseDialogEventArgs : MenuPageEventArgs { }

    public class EraseFileEventArgs : MenuPageEventArgs
    {
        public EraseFileEventArgs(int fileNumber)
        {
            FileNumber = fileNumber;
        }

        public int FileNumber { get; set; }
    }

    public class ExitGameEventArgs : MenuPageEventArgs { }

    public class KeybindChangeEventArgs : MenuPageEventArgs
    {
        public KeybindChangeEventArgs(ControlFunctions function, MenuButton button)
        {
            Function = function;
            Button = button;
        }
        
        public ControlFunctions Function { get; set; }
        public MenuButton Button { get; set; }
    }

    public class OpenDialogEventArgs : MenuPageEventArgs { }

    public class PageEventArgs : MenuPageEventArgs { }

    public class ResetPreferencesEventArgs : MenuPageEventArgs
    {
        public ResetPreferencesEventArgs(PreferenceTypes type)
        {
            Type = type;
        }

        public PreferenceTypes Type { get; set; }
    }

    public class ResolutionChangeEventArgs : MenuPageEventArgs
    {
        public ResolutionChangeEventArgs(Resolutions resolution)
        {
            Resolution = resolution;
        }

        public Resolutions Resolution { get; set; }
    }

    public class ResumeGameEventArgs : MenuPageEventArgs { }

    public class SaveGameEventArgs : MenuPageEventArgs
    {
        public SaveGameEventArgs(int fileNumber)
        {
            FileNumber = fileNumber;
        }

        public int FileNumber { get; set; }
    }

    public class StartOrLoadEventArgs : MenuPageEventArgs
    {
        public StartOrLoadEventArgs(int fileNumber)
        {
            FileNumber = fileNumber;
        }

        public int FileNumber { get; set; }
    }

    public class SwitchPageEventArgs : MenuPageEventArgs
    {
        public SwitchPageEventArgs(MenuPageNames page, MenuPageNames previousPage)
        {
            Page = page;
            PreviousPage = previousPage;
            EnterFromExit = null;
        }

        public SwitchPageEventArgs(MenuPageNames page, MenuPageNames previousPage, bool enterFromExit)
        {
            Page = page;
            PreviousPage = previousPage;
            EnterFromExit = enterFromExit;
        }

        // null means no, true means save, false means load
        public bool? EnterFromExit { get; set; }
        public MenuPageNames Page { get; set; }
        public MenuPageNames PreviousPage { get; set; }
    }

    public class VolumeChangeEventArgs : MenuPageEventArgs
    {
        public VolumeChangeEventArgs(bool up)
        {
            Up = up;
        }

        public bool Up { get; set; }
    }
    
    public delegate void MenuPageEvent(MenuPageEventArgs e);
}
