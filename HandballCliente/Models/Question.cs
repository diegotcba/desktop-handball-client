using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandballCliente
{
    [Serializable()]
    public class Question
    {
        public int id { get; set; }
        public String question { get; set; }
        public List<Answer> answers { get; set; }
        public int correctAnswer { get; set; }

        public Question() { answers = new List<Answer>(); }
    }
}
