﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandballCliente.Models
{
    public class Provincia
    {
        public int id { get; set; }
        public string nombre { get; set; }

        public Provincia(int id, string nombre)
        {
            this.id = id;
            this.nombre = nombre;
        }
    }
}
