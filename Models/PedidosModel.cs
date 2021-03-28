using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstructurasDatos_Lab03.Clases;

namespace EstructurasDatos_Lab03.Models
{
    public class PedidosModel
    {
        public string NombreCliente { get; set; }
        public string Direccion { get; set; }
        public string Nit { get; set; }
        public double Total { get; set; }

        public List<Farmacos> PedidoFarmacos = new List<Farmacos>();
    }
}
