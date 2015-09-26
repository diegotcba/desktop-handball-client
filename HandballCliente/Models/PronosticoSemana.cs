using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandballCliente.Models
{
    public class PronosticoSemana
    {
        public string diaSemana { get; set; }
        public List<PronosticoDiario> pronostico { get; set; }
    }
}
