using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Element.Common.HelperClasses
{
    public class SoundEventArgs : EventArgs
    {
    }

    public delegate void SoundEvent(SoundEventArgs e);
}
