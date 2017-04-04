using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEI.Models
{
    public class NowPlayingResponse : MovieResponse
    {
        public MinMaxDate Dates { get; set; }

    }

    public class MinMaxDate
    {
        public DateTime Minimum { get; set; }
        public DateTime Maximum { get; set; }
    }
}
