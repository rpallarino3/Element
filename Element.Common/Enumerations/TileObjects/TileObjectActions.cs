using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Element.Common.Enumerations.TileObjects
{
    public enum TileObjectActions
    {
        WalkOn,
        WalkOff,
        // walk on top is a direction agnostic walk, for floor object it is effectively place on top
        WalkOnTop,
        FallOnTop,
        JumpOver,
        Push,
        PushOnTop,
        Pull,
        LandOnTop,
        ClimbOnTop,
        ClimbOnBottom,
    }
}
