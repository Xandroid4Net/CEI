using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEI.Models
{
    public class Item
    {
        public string Poster_path { get; set; }
        public bool Adult { get; set; }

        public string Overview { get; set; }

        public DateTime? Release_date { get; set; }

        public IEnumerable<int> Genre_ids { get; set; }

        public int Id { get; set; }

        public string Original_title { get; set; }

        public string Original_language { get; set; }

        public string Backdrop_path { get; set; }

        public decimal Popularity { get; set; }

        public int Vote_count { get; set; }

        public bool Video { get; set; }

        public decimal Vote_average { get; set; }
    }
}
