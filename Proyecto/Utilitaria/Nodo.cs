using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Proyecto.Utilitaria.Diijkstra;

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


            var result = lst.Where(x => x.id == lista.referencia).Select(x => new ListaModels
            {
                id = x.id,
                lat = x.lat,
                lng = x.lng,
                nombreCiudad = x.nombreCiudad,
                referencia = x.referencia,
                valor = x.valor
            }).FirstOrDefault();

            lstMar.Add(result);
            lstMar.Add(lista);

            return lstMar;

        }

        public List<ListaCiudades> getListaMarcadores()
        {
            List<ListaModels> lst = new List<ListaModels>();
            List<ListaCiudades> lstc = new List<ListaCiudades>();
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


            lstc = result.ToList();

            List<Adyacente> lstAdy = new List<Adyacente>();
            if (HttpContext.Current.Session["Ady"] != null)
            {
                lstAdy = (List<Adyacente>)HttpContext.Current.Session["Ady"];


                foreach (var a in lstAdy)
                {
                    ListaCiudades c = new ListaCiudades();
                    foreach (var b in lst)
                    {

                        if (a.Desde == b.id)
                        {
                            c.CiudadOrigen = b.nombreCiudad;
                        }

                        if (a.Hasta == b.id)
                        {
                            c.CiudadDestino = b.nombreCiudad;
                        }

                    }

                    c.Distancia = a.distancia;
                    lstc.Add(c);

                }

            }

            return lstc;
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
                    matriz[(a.id - 1), (0)] = 0;
                }
                else if(a.referencia > 0)
                {
                    matriz[(a.referencia - 1), (a.id - 1)] = a.valor;
                }
                i++;
            }

            List<Adyacente> lstAdy = new List<Adyacente>();
            if (HttpContext.Current.Session["Ady"] != null)
            {
                lstAdy = (List<Adyacente>)HttpContext.Current.Session["Ady"];


                foreach(var a in lstAdy)
                {
                    matriz[(a.Desde - 1), (a.Hasta - 1)] = a.distancia;
                }

            }


                HttpContext.Current.Session["Verteci"] = matriz;
        }


        public List<string> CalcularDistancia(Persona perosna)
        {
            int[,] matriz = (int[,])HttpContext.Current.Session["Verteci"];

            List<ListaModels> lst = new List<ListaModels>();

            if (HttpContext.Current.Session["Lista"] != null)
            {
                lst = (List<ListaModels>)HttpContext.Current.Session["Lista"];
            }

            int cantidad = lst.Count;



            Dijkstra prueba = new Dijkstra();
            prueba.DijkstraAlgo(matriz, (perosna.idPosicionPartida - 1), cantidad);

            

            List<string> lstRe = new List<string>();

            double km = CalculoPresupuesto(perosna);

            int g = 1;
            foreach(var a in prueba.D)
            {
                if(a > 0 && a != int.MaxValue)
                {
                    if(a <= km)
                    {
                        lstRe.Add("*Desde " + obtenerNombreCiudad(lst, perosna.idPosicionPartida) + " a " + obtenerNombreCiudad(lst, g) + " Recorrera " + a + " Km");
                    }
                }
                
                g++;
            }

            if (lstRe.Count == 0)
            {
                lstRe.Add("No hay rutas para este punto o Su presupuesto es muy bajo para tomar algunas de las rutas");
            }
            
            return lstRe;
        }


        public List<string> retornoCliente(Persona per)
        {
            List<string> lst = new List<string>();

            lst.Add("Bienvenido Sr(a) " + per.nombrePersona);
            lst.Add("Marca de Vehiculo: " + per.marcaVehiculo);
            lst.Add("Precio del Galon de Combustible: " + per.precioCombustible);
            lst.Add("Recorrido de su Vehiculo por Galon: " + per.kilometroGalon);
            lst.Add("Presupuesto para sus Vacaciones: " + per.Presupuesto);
            lst.Add("Maximo de Kilometros a Recorrer segun su Presupuesto es de: " + CalculoPresupuesto(per));

            return lst;
        }

        public List<ListaModels> AgregarAdyacente(Adyacente ady)
        {
            List<ListaModels> lst = new List<ListaModels>();
            List<ListaModels> lstreturn = new List<ListaModels>();
            List<Adyacente> lstady = new List<Adyacente>();

            int[,] matriz = (int[,])HttpContext.Current.Session["Verteci"];

            matriz[(ady.Desde - 1), (ady.Hasta - 1)] = ady.distancia;

            HttpContext.Current.Session["Verteci"] = matriz;


            if (HttpContext.Current.Session["Lista"] != null)
            {
                lst = (List<ListaModels>)HttpContext.Current.Session["Lista"];
            }


            if (HttpContext.Current.Session["Ady"] != null)
            {
                lstady = (List<Adyacente>)HttpContext.Current.Session["Ady"];
            }

            var desde = lst.Where(x => x.id == ady.Desde).Select(x => new ListaModels
            {
                id = x.id,
                lat = x.lat,
                lng = x.lng,
                nombreCiudad = x.nombreCiudad,
                referencia = x.referencia,
                valor = x.valor
            }).FirstOrDefault();

            var hasta = lst.Where(x => x.id == ady.Hasta).Select(x => new ListaModels
            {
                id = x.id,
                lat = x.lat,
                lng = x.lng,
                nombreCiudad = x.nombreCiudad,
                referencia = x.referencia,
                valor = x.valor
            }).FirstOrDefault();


            lstady.Add(ady);
            HttpContext.Current.Session["Ady"] = null;
            HttpContext.Current.Session["Ady"] = lstady;


            lstreturn.Add(desde);
            lstreturn.Add(hasta);

            return lstreturn;

        }


        public double CalculoPresupuesto(Persona per)
        {
            double Galones = per.Presupuesto / per.precioCombustible;

            double km = Galones * per.kilometroGalon;

            return km;
        }
        


        public string obtenerNombreCiudad(List<ListaModels> lst, int posicion)
        {

            return lst.Where(x => x.id == posicion).FirstOrDefault().nombreCiudad;
        }


    }

}