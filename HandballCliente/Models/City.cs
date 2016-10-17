using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandballCliente.Models
{
    public class City
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int provinciaId { get; set; }

        public City(int id, string name, int stateId)
        {
            this.id = id;
            this.nombre = name;
            this.provinciaId = stateId;
        }
    }
}
