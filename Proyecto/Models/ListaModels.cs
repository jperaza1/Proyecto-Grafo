using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class ListaModels
    {
        
        public int id { get; set; }
        public int valor { get; set; }
        public string nombreCiudad { get; set; }
        public int referencia { get; set; }
        public string lat { get; set; }
        public string lng { get; set; } 
    }

    public class ListaCiudades
    {
        public string CiudadOrigen { get; set; }
        public string CiudadDestino { get; set; }
        public int Distancia { get; set; }
    }

    public class Persona
    {
        public int idPosicionPartida { get; set; }
        public string nombrePersona { get; set; }
        public string marcaVehiculo { get; set; }
        public double precioCombustible { get; set; }
        public double kilometroGalon { get; set; }
        public double Presupuesto { get; set; }
    }

}