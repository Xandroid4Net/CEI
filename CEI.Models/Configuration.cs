using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEI.Models
{
    public class Configuration
    {
        public ImageConfiguration Images { get; set; }
        public IEnumerable<string> Change_keys { get; set; }
    }
}
