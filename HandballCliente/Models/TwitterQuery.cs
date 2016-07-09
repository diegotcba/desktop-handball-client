using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandballCliente
{
    class TwitterQuery
    {
        public string hashtag { get; set; }
        public string user { get; set; }
        public string startDateTime { get; set; }
        public string endDateTime { get; set; }
        public string location { get; set; }
    }
}
