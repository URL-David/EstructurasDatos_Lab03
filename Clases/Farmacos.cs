using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstructurasDatos_Lab03.Clases
{
    public class Farmacos
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Inventario { get; set; }
        public int CantidadComprada { get; set; }
        public string CasaProductora { get; set; }
        public double Precio { get; set; }
        public double PrecioTotal { get; set; }

    }
}
