using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Element.Common.Enumerations.GameObjects
{
    // does this contain only actions that something can take or actions that something can have used on it?
    public enum GameObjectActionType
    {
        None,
        Turn,
        Grab, // is this actually an action?
        ReleaseGrab,
        Interact,
        Cast,
        // don't know if we need to add cycle here

        // single tile level translation
        Walk,
        WalkOn,
        WalkOff,
        WalkOnTop,
        TryWalk,
        Push,
        PushOn,
        PushOff,
        PushOnTop,
        TryPush,
        Pull,
        PullOn,
        PullOff,
        PullOnTop,
        TryPull,

        DropOnto, // direction agnostic
        LandOn,
        Jump,
        TryJump,

        // walk variants
        Climb,
        ClimbOnBottom,
        ClimbOnTop,
        ClimbOn,
        ClimbOff,
        TryClimb,
        StartSlide,
        Slide,

        Fall,
        Spawn,
        Float,
        FloatIn,

        FireIn,
        FireOut,
        WaterIn,
        WaterOut,
        NatureIn,
        NatureOut,
        ElectricityIn,
        ElectricityOut,
        LifeIn,
        LifeOut,

        Die,
        Sit,
        Lie,
    }
}
