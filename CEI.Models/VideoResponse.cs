using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEI.Models
{
    public class VideoResponse
    {
        public int Id { get; set; }
        public IEnumerable<Video> Results { get; set; }
    }
}
