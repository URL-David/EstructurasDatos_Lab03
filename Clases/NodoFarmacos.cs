using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstructurasDatos_Lab03.Clases
{
    public class NodoFarmacos: IComparable
    {
       public int ID { get; set; }
       public string Nombre { get; set; }
       public int Inventario { get; set; }
        public string Descripcion { get; set; }
        public string Productora { get; set; }
        public string Precio { get; set; }

        public Comparison<NodoFarmacos> BuscarID = delegate (NodoFarmacos Far1, NodoFarmacos Far2)
        {
            return Far1.ID.CompareTo(Far2.ID);
        };
        public Comparison<NodoFarmacos> BuscarNombre = delegate (NodoFarmacos Far1, NodoFarmacos Far2)
        {
            return Far1.Nombre.CompareTo(Far2.Nombre);
        };
        public Comparison<NodoFarmacos> BuscarIventario = delegate (NodoFarmacos Far1, NodoFarmacos Far2)
        {
            return Far1.Inventario.CompareTo(Far2.Inventario);
        };
        public Comparison<NodoFarmacos> BuscarDescripcion = delegate (NodoFarmacos Far1, NodoFarmacos Far2)
        {
            return Far1.Descripcion.CompareTo(Far2.Nombre);
        };
        public Comparison<NodoFarmacos> BuscarProductora = delegate (NodoFarmacos Far1, NodoFarmacos Far2)
        {
            return Far1.Productora.CompareTo(Far2.Nombre);
        };
        public Comparison<NodoFarmacos> BuscarPrecio = delegate (NodoFarmacos Far1, NodoFarmacos Far2)
        {
            return Far1.Precio.CompareTo(Far2.Inventario);
        };
        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
