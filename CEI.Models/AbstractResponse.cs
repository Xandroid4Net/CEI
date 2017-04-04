using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEI.Models
{
    public abstract class AbstractResponse
    {
        public IEnumerable<Item> Results { get; set; }
    }
}
