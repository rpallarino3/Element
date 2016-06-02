using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Element.ResourceManagement
{
    public class SaveEventArgs
    {
    }

    public delegate void SaveRequestedEvent(SaveEventArgs e);
    public delegate void SaveCompletedEvent(SaveEventArgs e);
}
