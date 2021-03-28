using System;
using System.Collections.Generic;
using System.Text;


namespace LibreriaGenerica.Estructuras
{
    public class ArbolAVL<T> : Interfaces.EstruturaBaseArbol<T> where T : IComparable
    {
        Nodo<T> Raiz = new Nodo<T>();
        public void Add(T Valor, Delegate delegado)
        {
            Insertar(Valor, delegado, Raiz);
            Raiz = Balancear(Raiz);
        }
        public void Delete(T Valor, Delegate Delegado)
        {
            Borrar(Valor, Delegado, Raiz);
        }
        public void Edit(T Valor, Delegate Delegado)
        {
            Nodo<T> NodoPivote = Raiz;
            Editar(Valor, Delegado, NodoPivote);

        }
        private void Editar(T Valor, Delegate Delegado, Nodo<T> NodoRaiz)
        {
            if (Convert.ToInt32(Delegado.DynamicInvoke(Valor, NodoRaiz.Valor)) == 0)
            {
                NodoRaiz.Valor = Valor;
            }
            else if (Convert.ToInt32(Delegado.DynamicInvoke(Valor, NodoRaiz.Valor)) == -1)
            {
                Editar(Valor, Delegado, NodoRaiz.Izquierda);

            }
            else if (Convert.ToInt32(Delegado.DynamicInvoke(Valor, NodoRaiz.Valor)) == 1)
            {
                Editar(Valor, Delegado, NodoRaiz.Derecha);
            }
        }

        public T Get(T Valor, Delegate Delegado)
        {
            return Obtener(Valor, Delegado);
        }
        public List<T> Mostrar()
        {
            List<T> ListaArbol = new List<T>();
            MostrarRaiz(Raiz, ListaArbol);
            return ListaArbol;
        }

        private void MostrarRaiz(Nodo<T> NodoRaiz, List<T> Lista)
        {
            if (NodoRaiz.Valor != null)
            {
                MostrarRaiz(NodoRaiz.Izquierda, Lista);
                Lista.Add(NodoRaiz.Valor);
                MostrarRaiz(NodoRaiz.Derecha, Lista);
            }
        }
        private Nodo<T> ActualizarEliminar(Nodo<T> NodoRaiz)
        {
            if (NodoRaiz.Derecha.Valor != null)
                NodoRaiz.Derecha = ActualizarEliminar(NodoRaiz.Derecha);
            if (NodoRaiz.Izquierda.Valor != null)
                NodoRaiz.Izquierda = ActualizarEliminar(NodoRaiz.Izquierda);
            NodoRaiz = Balancear(NodoRaiz);
            return NodoRaiz;
        }
        protected override Nodo<T> Borrar(T Valor, Delegate Delegado, Nodo<T> NodoRaiz)
        {

            if (Convert.ToInt32(Delegado.DynamicInvoke(Valor, NodoRaiz.Valor)) == 0)
            {
                if (NodoRaiz.Izquierda.Valor == null && NodoRaiz.Derecha.Valor == null)
                {
                    NodoRaiz = new Nodo<T>();
                    NodoRaiz.Izquierda = new Nodo<T>();
                    NodoRaiz.Derecha = new Nodo<T>();
                }
                //El nodo tiene 2 Hijos
                else if (NodoRaiz.Derecha.Valor != null && NodoRaiz.Izquierda.Valor != null)
                {
                    Nodo<T> NodoSustituto = NodoRaiz;
                    Nodo<T> NodoAux1 = new Nodo<T>();
                    NodoSustituto = NodoSustituto.Derecha;
                    while (NodoSustituto.Izquierda.Valor != null)
                    {
                        NodoAux1 = NodoSustituto;
                        NodoSustituto = NodoSustituto.Izquierda;
                    }
                    if (NodoAux1.Valor != null)
                    {
                        NodoAux1.Izquierda = NodoSustituto.Derecha;
                        NodoSustituto.Derecha = NodoRaiz.Derecha;
                    }
                    NodoSustituto.Izquierda = NodoRaiz.Izquierda;
                    NodoRaiz = NodoSustituto;
                }
                //El nodo tiene un solo hijo
                else if (NodoRaiz.Derecha.Valor != null)
                {
                    NodoRaiz = NodoRaiz.Derecha;
                }
                else if (NodoRaiz.Izquierda.Valor != null)
                {
                    NodoRaiz = NodoRaiz.Izquierda;
                }
                ActualizarAltura(NodoRaiz);
                NodoRaiz = ActualizarEliminar(NodoRaiz);
                ActualizarAltura(NodoRaiz);
            }
            else if (Convert.ToInt32(Delegado.DynamicInvoke(Valor, NodoRaiz.Valor)) == 1)
            {
                NodoRaiz.AlturaDerecha = 0;
                NodoRaiz.Derecha = Borrar(Valor, Delegado, NodoRaiz.Derecha);
                if (NodoRaiz.Derecha.Valor != null)
                {
                    NodoRaiz.AlturaDerecha = Math.Max(NodoRaiz.Derecha.AlturaDerecha, NodoRaiz.Derecha.AlturaIzquierda) + 1;
                }
            }
            else if (Convert.ToInt32(Delegado.DynamicInvoke(Valor, NodoRaiz.Valor)) == -1)
            {
                NodoRaiz.AlturaIzquierda = 0;
                NodoRaiz.Izquierda = Borrar(Valor, Delegado, NodoRaiz.Izquierda);
                if (NodoRaiz.Izquierda.Valor != null)
                {
                    NodoRaiz.AlturaIzquierda = Math.Max(NodoRaiz.Izquierda.AlturaDerecha, NodoRaiz.Izquierda.AlturaIzquierda) + 1;
                }
            }
            NodoRaiz.Equilibrio = NodoRaiz.AlturaDerecha - NodoRaiz.AlturaIzquierda;
            NodoRaiz = Balancear(NodoRaiz);
            return NodoRaiz;
        }

        protected override T Obtener(T Valor, Delegate Delegado)
        {
            Nodo<T> NodoPivote = Raiz;
            Nodo<T> NoEncontrado = new Nodo<T>();
            while (NodoPivote.Valor != null)
            {
                if (Convert.ToInt32(Delegado.DynamicInvoke(Valor, NodoPivote.Valor)) == 1)
                {
                    if (NodoPivote.Derecha.Valor != null)
                    {
                        NodoPivote = NodoPivote.Derecha;
                    }
                    else
                    {
                        return NoEncontrado.Valor;
                    }
                }
                else if (Convert.ToInt32(Delegado.DynamicInvoke(Valor, NodoPivote.Valor)) == -1)
                {
                    if (NodoPivote.Izquierda.Valor != null)
                    {
                        NodoPivote = NodoPivote.Izquierda;
                    }
                    else
                    {
                        return NoEncontrado.Valor;
                    }
                }
                else if (Convert.ToInt32(Delegado.DynamicInvoke(Valor, NodoPivote.Valor)) == 0)
                {
                    return NodoPivote.Valor;
                }
                else
                {
                    return NoEncontrado.Valor;
                }
            }
            return NodoPivote.Valor;

        }

        private void ActualizarAltura(Nodo<T> NodoRaiz)
        {
            NodoRaiz.AlturaIzquierda = 0;
            NodoRaiz.AlturaDerecha = 0;
            NodoRaiz.Equilibrio = 0;
            if (NodoRaiz.Izquierda.Valor != null)
            {
                ActualizarAltura(NodoRaiz.Izquierda);
                if (NodoRaiz.Izquierda.Valor != null)
                {
                    NodoRaiz.AlturaIzquierda = Math.Max(NodoRaiz.Izquierda.AlturaDerecha, NodoRaiz.Izquierda.AlturaIzquierda) + 1;
                    NodoRaiz.Equilibrio = NodoRaiz.AlturaDerecha - NodoRaiz.AlturaIzquierda;
                }
            }
            if (NodoRaiz.Derecha.Valor != null)
            {
                ActualizarAltura(NodoRaiz.Derecha);
                if (NodoRaiz.Derecha.Valor != null)
                {
                    NodoRaiz.AlturaDerecha = Math.Max(NodoRaiz.Derecha.AlturaDerecha, NodoRaiz.Derecha.AlturaIzquierda) + 1;
                    NodoRaiz.Equilibrio = NodoRaiz.AlturaDerecha - NodoRaiz.AlturaIzquierda;
                }
            }
        }

        //ROTACIONES
        private Nodo<T> Balancear(Nodo<T> NodoRaiz)
        {
            //Rotacion Doble a la Izquierda(+2,-1)
            if (NodoRaiz.Equilibrio == 2 && NodoRaiz.Derecha.Equilibrio == -1)
            {
                //Rotacion Derecha, luego Rotacion Izquierda(2,-1)
                Nodo<T> NodoSust1 = NodoRaiz.Derecha;
                Nodo<T> NodoSust2 = NodoRaiz.Derecha.Izquierda;
                NodoSust1.Izquierda = NodoSust2.Derecha;
                NodoRaiz.Derecha = NodoSust2;
                NodoSust2.Derecha = NodoSust1;
            }
            //Rotacion Doble a al Derecha(-2,+1)
            else if (NodoRaiz.Equilibrio == -2 && NodoRaiz.Izquierda.Equilibrio == 1)
            {
                //Rotacion Izquierda, luego Rotacion Derecha
                Nodo<T> NodoSust1 = NodoRaiz.Izquierda;
                Nodo<T> NodoSust2 = NodoRaiz.Izquierda.Derecha;
                NodoSust1.Derecha = NodoSust2.Izquierda;
                NodoRaiz.Izquierda = NodoSust2;
                NodoSust2.Izquierda = NodoSust1;
            }
            //Rotacion Simple a la Izquierda
            if (NodoRaiz.Equilibrio == 2)
            {
                Nodo<T> NodoAux1 = NodoRaiz.Derecha;
                Nodo<T> NodoAux2 = NodoRaiz.Derecha.Izquierda;
                NodoRaiz.Derecha = NodoAux2;
                NodoAux1.Izquierda = NodoRaiz;
                ActualizarAltura(NodoAux1);
                NodoRaiz = NodoAux1;
            }
            //Rotacion Simple a la Derecha

            else if (NodoRaiz.Equilibrio == -2)
            {
                Nodo<T> NodoAux1 = NodoRaiz.Izquierda;
                Nodo<T> NodoAux2 = NodoRaiz.Izquierda.Derecha;
                NodoRaiz.Izquierda = NodoAux2;
                NodoAux1.Derecha = NodoRaiz;
                ActualizarAltura(NodoAux1);
                NodoRaiz = NodoAux1;
            }
            return NodoRaiz;
        }

        protected override void Insertar(T Valor, Delegate Delegado, Nodo<T> NodoRaiz)
        {
            NodoRaiz.Equilibrio = 0;
            if (NodoRaiz.Valor == null)
            {
                NodoRaiz.Valor = Valor;
                NodoRaiz.Izquierda = new Nodo<T>();
                NodoRaiz.Derecha = new Nodo<T>();
            }
            else if (Convert.ToInt32(Delegado.DynamicInvoke(Valor, NodoRaiz.Valor)) == 1)
            {
                NodoRaiz.AlturaDerecha = 0;
                Insertar(Valor, Delegado, NodoRaiz.Derecha);
                NodoRaiz.Derecha = Balancear(NodoRaiz.Derecha);
                if (NodoRaiz.Derecha.Valor != null)
                {
                    NodoRaiz.AlturaDerecha = Math.Max(NodoRaiz.Derecha.AlturaDerecha, NodoRaiz.Derecha.AlturaIzquierda) + 1;
                }
            }
            else if (Convert.ToInt32(Delegado.DynamicInvoke(Valor, NodoRaiz.Valor)) == -1)
            {
                NodoRaiz.AlturaIzquierda = 0;
                Insertar(Valor, Delegado, NodoRaiz.Izquierda);
                NodoRaiz.Izquierda = Balancear(NodoRaiz.Izquierda);
                if (NodoRaiz.Izquierda.Valor != null)
                {
                    NodoRaiz.AlturaIzquierda = Math.Max(NodoRaiz.Izquierda.AlturaDerecha, NodoRaiz.Izquierda.AlturaIzquierda) + 1;
                }
            }
            NodoRaiz.Equilibrio = NodoRaiz.AlturaDerecha - NodoRaiz.AlturaIzquierda;
        }
        public List<T> Where(Func<T, bool> Predicate)
        {
            var Lista = Mostrar();
            List<T> ListaResultado = new List<T>();
            foreach (T item in Lista)
            {
                if (Predicate(item))
                {
                    ListaResultado.Add(item);
                }
            }
            return ListaResultado;
        }
    }
}
