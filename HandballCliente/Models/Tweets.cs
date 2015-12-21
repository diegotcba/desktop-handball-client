using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandballCliente.Models
{
    public class Tweets
    {
        public long id { get; set; }
        public string hashtag { get; set; }
        public string userName { get; set; }
        public string fullName { get; set; }
        public string message { get; set; }
        public string urlAvatar { get; set; }
        public bool reTweet { get; set; }
    }
}
