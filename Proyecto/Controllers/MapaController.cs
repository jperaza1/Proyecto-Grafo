using Proyecto.Models;
using Proyecto.Utilitaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class MapaController : Controller //Nombre del controlador
    {
        Lista l = new Lista();
        
        // GET: Mapa
        public ActionResult Rutas()
        {
            if (Session["Lista"] == null)
            {
                Session["Lista"] = null;
                Session["Verteci"] = null;
            }


            return View();
        }

        public ActionResult insertNodo(ListaModels lista) //Metodo que devuelve un Json con todos los marcados
        {
            if (ModelState.IsValid)
            {
                l.ingresar(lista);
                return Json(l.devolverLista(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View(lista);
            }
            
        }

        public JsonResult lstCoordenadas(ListaModels lista)
        {
            return Json(new { coordenadas = l.devolverListaCoordenadas(lista) , ciudades = l.getListaMarcadores() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CalcularDistancias(Persona persona)
        {
            return Json(new { datos = l.CalcularDistancia(persona) }, JsonRequestBehavior.AllowGet);
        }

    }
}