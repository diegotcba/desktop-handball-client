using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandballCliente
{
    [Serializable()]
    public class Answer
    {
        public String answer { get; set; }

        public Answer() { }
        public Answer(String a)
        {
            this.answer = a;
        }
    }
}
