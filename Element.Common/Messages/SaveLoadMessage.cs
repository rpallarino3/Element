using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.Common.Data;

namespace Element.Common.Messages
{
    public class SaveLoadMessage
    {
        public bool Erase { get; set; }
        public int FileNumber { get; set; }
        public SaveData Data { get; set; }
    }
}
