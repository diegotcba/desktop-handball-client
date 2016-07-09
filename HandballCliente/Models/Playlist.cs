using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandballCliente.Models
{
    public class Playlist
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string dateTime { get; set; }
        public List<Tweets> tweets { get; set; }
    }
}
