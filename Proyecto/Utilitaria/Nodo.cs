using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto.Utilitaria
{
    public class Nodo
    {
        public int valor;
        public string nombreCiudad;
        public string lat;
        public string lng;
        public Nodo ptrSig;
        public Nodo ptrAnt;

        public Nodo(int val, string nombreCiudad,string lat, string lng)
        {
            this.valor = val;
            this.nombreCiudad = nombreCiudad;
            this.lat = lat;
            this.lng = lng;
            ptrSig = ptrAnt = null;
        }

    }

    public class Lista
    {
        public int Tamano { get; set; }
        public Nodo ptrPrimero { get; set; }
        public Nodo ptrUltimo { get; set; }

        public Lista()
        {
            ptrPrimero = ptrUltimo = null;
            Tamano = 0;
        }

        public void ingresar(ListaModels lista)
        {
            List<ListaModels> lst = new List<ListaModels>();
            if (HttpContext.Current.Session["Lista"] != null)
            {
                lst = (List<ListaModels>)HttpContext.Current.Session["Lista"];
            }

            int numero = lst.Count;

            lista.id = numero + 1;

            lst.Add(lista);

            HttpContext.Current.Session["Lista"] = null;

            HttpContext.Current.Session["Lista"] = lst;
        }

        public List<ListaModels> devolverLista()
        {
            List<ListaModels> lst = new List<ListaModels>();

            if (HttpContext.Current.Session["Lista"] != null)
            {
                lst = (List<ListaModels>)HttpContext.Current.Session["Lista"];
            }


            agregarMatriz(lst);

            return lst;

        }


        public List<ListaModels> devolverListaCoordenadas(ListaModels lista)
        {
            List<ListaModels> lst = new List<ListaModels>();

            List<ListaModels> lstMar = new List<ListaModels>();

            if (HttpContext.Current.Session["Lista"] != null)
            {
                lst = (List<ListaModels>)HttpContext.Current.Session["Lista"];
            }

            ListaModels l = null;

            foreach (var a in lst)
            {
                if (lista.referencia == a.id)
                {
                    l = new ListaModels
                    {
                        id = a.id,
                        lat = a.lat,
                        lng = a.lng,
                        nombreCiudad = a.nombreCiudad,
                        referencia = a.referencia,
                        valor = a.valor
                    };

                    lstMar.Add(l);
                    lstMar.Add(lista);
                    break;

                }

            }

            return lstMar;

        }

        public List<ListaCiudades> getListaMarcadores()
        {
            List<ListaModels> lst = new List<ListaModels>();
            if (HttpContext.Current.Session["Lista"] != null)
            {
                lst = (List<ListaModels>)HttpContext.Current.Session["Lista"];
            }

            var query = lst.Where(x => x.referencia != 0).ToList();

            var result = from t1 in query
                         join t2 in lst on
                         t1.referencia equals t2.id
                         select new ListaCiudades
                         {
                             CiudadDestino = t1.nombreCiudad,
                             CiudadOrigen = t2.nombreCiudad,
                             Distancia = t1.valor
                         };

            return result.ToList();
        }



        public void agregarMatriz(List<ListaModels> lstLista)
        {
            HttpContext.Current.Session["Verteci"] = null;
            int cantidad = lstLista.Count;
            int[,] matriz = new int[cantidad,cantidad];

            int i = 0;
            foreach(var a in lstLista)
            {
                if (a.referencia == -1)
                {
                    matriz[(a.id - 1), (0)] = a.valor;
                }
                else
                {
                    matriz[(a.referencia - 1), (a.id - 1)] = a.valor;
                }
                i++;
            }

            HttpContext.Current.Session["Verteci"] = matriz;

        }


        public void CalcularDistancia(Persona perosna)
        {
            //Falta la parte mas yuca
        }

    }

}