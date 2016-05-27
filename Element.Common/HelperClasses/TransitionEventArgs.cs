using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Environment;

namespace Element.Common.HelperClasses
{
    public class TransitionEventArgs : EventArgs
    {
        public Transition Transition { get; set; }
    }

    public delegate void TransitionEvent(TransitionEventArgs e);
}
