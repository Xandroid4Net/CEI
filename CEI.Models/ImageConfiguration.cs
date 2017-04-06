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

        public IEnumerable<string> Backdrop_sizes { get; set; } = new List<string>();
        public IEnumerable<string> Logo_sizes { get; set; } = new List<string>();

        public IEnumerable<string> Poster_sizes { get; set; } = new List<string>();

        public IEnumerable<string> Profile_sizes { get; set; } = new List<string>();
        public IEnumerable<string> Still_sizes { get; set; } = new List<string>();

        public const string W92 = "w92";
        public const string Original = "original";
        public const string W154 = "w154";
        public bool Has_w92_Option()
        {
            return Backdrop_sizes.Contains(W92) || Logo_sizes.Contains(W92) || Poster_sizes.Contains(W92) || Profile_sizes.Contains(W92) || Still_sizes.Contains(W92);
        }

        public bool Has_w154_Option()
        {
            return Backdrop_sizes.Contains(W154) || Logo_sizes.Contains(W154) || Poster_sizes.Contains(W154) || Profile_sizes.Contains(W154) || Still_sizes.Contains(W154);
        }
    }
}
