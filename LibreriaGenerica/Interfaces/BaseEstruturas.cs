using System;
using System.Collections.Generic;
using System.Text;
using LibreriaGenerica.Estructuras;

namespace LibreriaGenerica.Interfaces
{
    public abstract class EstruturaBaseArbol<T>
    {
        protected abstract void Insertar(T Valor, Delegate Delegado, Nodo<T> NodoRaiz);
        protected abstract Nodo<T> Borrar(T Valor, Delegate Delegado, Nodo<T> NodoRaiz);
        protected abstract T Obtener(T Valor, Delegate Delegado);

    }
}
