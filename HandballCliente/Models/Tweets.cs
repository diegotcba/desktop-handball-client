using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandballCliente.Models
{
    public class Tweets
    {
        public int id { get; set; }
        public string hashtag { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string mensaje { get; set; }
        public string avatar { get; set; }
    }
}
