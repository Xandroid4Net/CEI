using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEI.Models
{
    public class MovieResponse : AbstractResponse
    {
        public int Page { get; set; }
        public int Total_pages { get; set; }
        public int Total_results { get; set; }
    }
}
