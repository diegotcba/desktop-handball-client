using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Serialization;
using System.Data;
namespace HandballCliente
{
    [Serializable()]
    public class Player
    {
        public int number { get; set; }
        public String lastName { get; set; }
        public String firstName { get; set; }
        public String comment { get; set; }

        public Player()
        {

        }

        public Player(int number, String lastNmae, String firstName, String comment="")
        {
            this.number = number;
            this.lastName = lastName;
            this.firstName = firstName;
            this.comment = comment;
        }

        public String getFullName()
        {
            return lastName + ", " + firstName;
        }
    }
}
