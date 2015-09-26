using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandballCliente.Models
{
    public class PronosticoCiudad
    {
        public int id { get; set;  }
        public string ciudad { get; set; }
        public string fechaHoraPronosticoOficial { get; set; }
        public List<PronosticoSemana> pronosticoSemana { get; set; }
    }
}
