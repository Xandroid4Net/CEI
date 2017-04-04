using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEI.Models
{
    public class Video
    {
        public string Id { get; set; }
        public string ISO_639_1 { get; set; }
        public string ISO_3166_1{ get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Site { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }
    }
}
