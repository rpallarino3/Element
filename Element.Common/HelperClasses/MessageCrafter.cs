using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Enumerations.Environment;
using Element.Common.Environment;
using Element.Common.Messages;

namespace Element.Common.HelperClasses
{
    public static class MessageCrafter
    {

        public static RegionLoadMessage CreateRegionLoadMessage(RegionNames region)
        {
            return new RegionLoadMessage();
        }

        public static SaveLoadMessage CreateSaveLoadMessage(Dictionary<RegionNames, Region> regions)
        {
            return new SaveLoadMessage();
        }
    }
}
