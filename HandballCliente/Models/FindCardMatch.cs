using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandballCliente.Models
{
    public class FindCardMatch
    {
        public long uid { get; set; }
        public DateTime dateTime { get; set; }
        public List<Card> Cards { get; set; }
    }
}
