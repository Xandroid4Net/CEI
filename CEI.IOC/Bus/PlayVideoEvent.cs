using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEI.IOC.Bus
{
    public class PlayVideoEvent
    {
        public readonly string Key;
        public PlayVideoEvent(string key)
        {
            Key = key;
        }
    }
}
