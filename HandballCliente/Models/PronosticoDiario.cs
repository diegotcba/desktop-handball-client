using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandballCliente.Models
{
    public class PronosticoDiario
    {
        public string parteDia { get; set; }
        public string icono { get; set; }
        public string tipoTemperatura { get; set; }
        public int temperatura { get; set; }
        public string simboloTiempo { get; set; }
        public string horaPronostico { get; set; }
        public string textoPronostico { get; set; }
    }
}
