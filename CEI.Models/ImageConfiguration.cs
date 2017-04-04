using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEI.Models
{
    public class ImageConfiguration
    {
        public string Base_url { get; set; }
        public string Secure_base_url { get; set; }

        public IEnumerable<string> Backdrop_sizes { get; set; }
        public IEnumerable<string> Logo_sizes { get; set; }

        public IEnumerable<string> Poster_sizes { get; set; }

        public IEnumerable<string> Profile_sizes { get; set; }
        public IEnumerable<string> Still_sizes { get; set; }
    }
}
