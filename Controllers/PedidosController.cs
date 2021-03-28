using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EstructurasDatos_Lab03.Models;
using EstructurasDatos_Lab03.Clases;
using LibreriaGenerica.Estructuras;
using Microsoft.VisualBasic.FileIO;
using Microsoft.AspNetCore.Hosting;
using PagedList;

namespace EstructurasDatos_Lab03.Controllers
{
    public class PedidosController : Controller
    {
        public static ArbolAVL<NodoFarmacos> ArbolBusqueda = new ArbolAVL<NodoFarmacos>();
        public static List<NodoFarmacos> FarmacosVacios = new List<NodoFarmacos>();
        public static PedidosModel NuevoPedido = new PedidosModel();
        public static string RutaBase;
        public static string RutaArchivoAux;
        private IWebHostEnvironment Environment;
        public void Editar(NodoFarmacos NodoAuxFarmaco, int Borrar)
        {
            int NumeroLinea = 1;
            string Linea;
            using (StreamReader ArchivoLectura = new StreamReader(RutaBase))
            {
                using (StreamWriter ArchivoLimpiar = new StreamWriter(RutaArchivoAux))
                {
                    ArchivoLimpiar.WriteLine(ArchivoLectura.ReadLine());
                    ArchivoLimpiar.Flush();
                }
                using (StreamWriter ArchivoEscritura = new StreamWriter(RutaArchivoAux, true))
                {
                    do
                    {
                        Linea = ArchivoLectura.ReadLine();
                        if (NumeroLinea == NodoAuxFarmaco.ID)
                        {
                            int Posicion = Linea.Length - 1;
                            if (Borrar > 10)
                                Posicion = Linea.Length - 2;
                            Linea = Linea.Substring(0, Posicion);
                            Linea += NodoAuxFarmaco.Inventario;
                        }
                        ArchivoEscritura.WriteLineAsync(Linea);
                        ArchivoEscritura.Flush();
                        NumeroLinea++;
                    } while (Linea != null);

                }
            }
            System.IO.File.Copy(RutaArchivoAux, RutaBase, true);
        }
        public ActionResult Reabastecer(int? Pagina)
        {
            List<NodoFarmacos> ListaFamacos = FarmacosVacios;
            if (Pagina == 0)
                Pagina = 1;
            int Cantidad = 12;
            int NumeroPagina = (Pagina ?? 1);
            return View("ReabastecimientoFar", ListaFamacos.ToPagedList(NumeroPagina, Cantidad));
        }

        public ActionResult RealizarReabastecimiento()
        {
            Random GenerarRandom = new Random();
            foreach (NodoFarmacos item in FarmacosVacios)
            {
                int NumeroRandom = GenerarRandom.Next(1, 15);
                item.Inventario = NumeroRandom;
                ArbolBusqueda.Add(item, item.BuscarNombre);
                Editar(item, NumeroRandom);
            }
            FarmacosVacios.Clear();
            ViewBag.Farmacos = ArbolBusqueda.Mostrar();
            return View("Index");
        }

        public Farmacos ObtenerFarmaco(NodoFarmacos NodoAuxFarmaco)
        {
            Farmacos FarmacoMostrar = new Farmacos();
            using (TextFieldParser Archivo = new TextFieldParser(RutaBase))
            {
                Archivo.TextFieldType = FieldType.Delimited;
                Archivo.SetDelimiters(",");
                while (!Archivo.EndOfData)
                {
                    try
                    {
                        if (NodoAuxFarmaco.ID == Archivo.LineNumber - 1)
                        {
                            string[] Texto = Archivo.ReadFields();
                            FarmacoMostrar.ID = Convert.ToInt32(Texto[0]);
                            FarmacoMostrar.Nombre = Texto[1];
                            FarmacoMostrar.Descripcion = Texto[2];
                            FarmacoMostrar.CasaProductora = Texto[3];
                            FarmacoMostrar.Precio = Convert.ToDouble(Texto[4].Substring(1));
                            FarmacoMostrar.Inventario = Convert.ToInt32(Texto[5]);
                            Archivo.ReadToEnd();
                        }
                    }
                    catch (Exception)
                    {
                    }
                    Archivo.ReadLine();
                }
            }
            return FarmacoMostrar;
        }

        public PedidosController(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }
        // GET: Pedidos
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RealizarPedidos()
        {
            ViewBag.Farmacos = NuevoPedido.PedidoFarmacos;
            return View(NuevoPedido);
        }
        public ActionResult ConfirmarPedido()
        {
            NuevoPedido = new PedidosModel();
            return View("Index");
        }

        //Vista Importar  AgregarFarmaco
        public ActionResult CargarFarmacos(int? pagina)
        {

            return View("ImportarFarmacos");
        }

        public ActionResult Paginacion(int? Pagina)
        {
            List<NodoFarmacos> ListaFamacos = ArbolBusqueda.Mostrar();
            if (Pagina == 0)
                Pagina = 1;
            int Cantidad = 12;
            int NumeroPagina = (Pagina ?? 1);
            return View("InventarioFarmacos", ListaFamacos.ToPagedList(NumeroPagina, Cantidad));
        }

        public ActionResult BuscarFarmacos(string Texto, IFormCollection collection)
        {
            NuevoPedido.NombreCliente = collection["NombreCliente"];
            NuevoPedido.Nit = collection["Nit"];
            NuevoPedido.Direccion = collection["Direccion"];
            NodoFarmacos NodoAuxFarmaco = new NodoFarmacos();
            NodoAuxFarmaco.Nombre = Texto;
            NodoAuxFarmaco = ArbolBusqueda.Get(NodoAuxFarmaco, NodoAuxFarmaco.BuscarNombre);
            if (NodoAuxFarmaco != null)
            {
                return View("AgregarFarmaco", ObtenerFarmaco(NodoAuxFarmaco));
            }
            else
            {
                ViewBag.Farmacos = NuevoPedido.PedidoFarmacos;
                return View("RealizarPedidos", NuevoPedido);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarFarmaco(IFormCollection collection)
        {
            NodoFarmacos EditarFarmaco = new NodoFarmacos();
            EditarFarmaco.Nombre = collection["Nombre"];
            EditarFarmaco = ArbolBusqueda.Get(EditarFarmaco, EditarFarmaco.BuscarNombre);
            int Descontar = int.Parse(collection["Inventario"]);
            Farmacos FarmacoAux = ObtenerFarmaco(EditarFarmaco);
            if (EditarFarmaco.Inventario >= Descontar && Descontar >= 0)
            {
                //Borra el nodo de un farmaco exitente
                if (NuevoPedido.PedidoFarmacos.Exists(x => x.Nombre == EditarFarmaco.Nombre))
                {
                    FarmacoAux = NuevoPedido.PedidoFarmacos.Find(x => x.Nombre == EditarFarmaco.Nombre);
                    NuevoPedido.PedidoFarmacos.Remove(FarmacoAux);
                }
                //Resta la cantidad a Inventario
                EditarFarmaco.Inventario -= Descontar;
                //Edita el Nodo en el Arbol
                ArbolBusqueda.Edit(EditarFarmaco, EditarFarmaco.BuscarNombre);
                //Agrego la Cantidad Comprada al producto
                FarmacoAux.CantidadComprada += Descontar;
                FarmacoAux.PrecioTotal = Math.Round(FarmacoAux.Precio * Descontar, 2);
                NuevoPedido.PedidoFarmacos.Add(FarmacoAux);
                NuevoPedido.Total = 0;
                foreach (Farmacos item in NuevoPedido.PedidoFarmacos)
                {
                    NuevoPedido.Total += item.PrecioTotal;
                }
                if (EditarFarmaco.Inventario == 0)
                {
                    FarmacosVacios.Add(EditarFarmaco);
                    ArbolBusqueda.Delete(EditarFarmaco, EditarFarmaco.BuscarNombre);
                }
                Editar(EditarFarmaco, Descontar);
                ViewBag.Farmacos = NuevoPedido.PedidoFarmacos;
                return View("RealizarPedidos", NuevoPedido);
            }
            else
            {
                return View("AgregarFarmaco", FarmacoAux);
            }
        }

        [HttpPost]
        public IActionResult ImportarFarmacos(IFormFile ArchivoCargado)
        {
            if (ArchivoCargado.FileName.Contains(".csv"))
            {
                string Ruta = Path.Combine(Environment.WebRootPath, "Documentos/");
                RutaArchivoAux = Ruta + "AchivoAux.csv";
                RutaBase = Path.Combine(Ruta, "ArchivoOriginal.csv");
                if (!Directory.Exists(Ruta))
                    Directory.CreateDirectory(Ruta);
                using (FileStream stream = new FileStream(Path.Combine(Ruta, "ArchivoOriginal.csv"), FileMode.Create))
                {
                    ArchivoCargado.CopyTo(stream);
                }
                using (FileStream stream2 = new FileStream(Path.Combine(Ruta, "AchivoAux.csv"), FileMode.Create))
                {
                    ArchivoCargado.CopyTo(stream2);
                }
                using (TextFieldParser Archivo = new TextFieldParser(RutaBase))
                {
                    Archivo.TextFieldType = FieldType.Delimited;
                    Archivo.SetDelimiters(",");
                    while (!Archivo.EndOfData)
                    {
                        string[] Texto = Archivo.ReadFields();
                        NodoFarmacos NodoFarmaco = new NodoFarmacos();
                        try
                        {
                            NodoFarmaco.ID = Convert.ToInt32(Texto[0]);
                            NodoFarmaco.Nombre = Texto[1];
                            NodoFarmaco.Descripcion = Texto[2];
                            NodoFarmaco.Productora = Texto[3];
                            NodoFarmaco.Precio = Texto[4];
                            NodoFarmaco.Inventario = Convert.ToInt32(Texto[5]);
                            
                            ArbolBusqueda.Add(NodoFarmaco, NodoFarmaco.BuscarNombre);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    ViewBag.Farmacos = ArbolBusqueda.Mostrar();
                    return View("Index");
                }
            }
            else { return View("ImportarFarmacos"); }
        }

        public ActionResult Details(string Nombre)
        {
            NodoFarmacos NodoAuxFarmaco = new NodoFarmacos();
            NodoAuxFarmaco.Nombre = Nombre;
            NodoAuxFarmaco = ArbolBusqueda.Get(NodoAuxFarmaco, NodoAuxFarmaco.BuscarNombre);
        
                return View(NodoAuxFarmaco);
         
         
        }



    }
}