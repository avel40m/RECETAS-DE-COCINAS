using RecetasCocinas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecetasCocinas.Controllers
{
    [Authorize]
    public class PreparacionesController : Controller
    {
        private recetasEntity bd = new recetasEntity(); 
        // GET: Preparaciones
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Crear(int id)
        {
            var nombre = bd.receta.FirstOrDefault(x => x.id == id);
            ViewBag.nombre = nombre.titulo;
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public ActionResult Crear(preparacion preparacion)
        {
            if (preparacion.id == 0)
            {
                bd.preparacion.Add(preparacion);
                bd.SaveChanges();
                return Json("Agregado");
            }
            else
            {
                preparacion p = bd.preparacion.FirstOrDefault(x => x.id == preparacion.id);
                p.descripcion = preparacion.descripcion;
                bd.SaveChanges();
                return Json("Actualizado");
            }
        }

        public ActionResult Listado(int id)
        {
            var lista = from x in bd.preparacion
                        join i in bd.receta
                        on x.id_receta equals i.id where x.id_receta == id
                        select new
                        {
                            x.id,
                            x.descripcion
                        };
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Buscar(int id)
        {
            var preparacion = from x in bd.preparacion
                              join i in bd.receta
      on x.id_receta equals i.id where x.id == id
                              select new
                              {
                                  x.id,
                                  x.descripcion
                              };
            return Json(preparacion, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            preparacion preparacion = bd.preparacion.FirstOrDefault(x => x.id == id);
            bd.preparacion.Remove(preparacion);
            bd.SaveChanges();
            return Json("Se elimino");
        }
    }
}