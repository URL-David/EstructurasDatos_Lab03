using System;
using System.Collections.Generic;
using System.Text;

namespace LibreriaGenerica.Interfaces
{
    interface IEstructuras<T>
    {
        void Insertar();
        T Borrar();
        T Obetener();
    }
}
