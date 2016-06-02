using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Element.Common.Enumerations.GameBasics
{
    public enum GameStates
    {
        Start, // go from start through whatever we need to do then to start menu
        StartMenu,
        Roam,
        ExitMenu,
        Chat,
    }
}
