using RecetasCocinas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecetasCocinas.Controllers
{
    [Authorize]
    public class IngredientesController : Controller
    {
        private recetasEntity bd = new recetasEntity();
        // GET: Ingredientes
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Listados(int id)
        {
            try
            {
                var lista = from i in bd.ingrediente join x in bd.receta
                            on i.id_receta equals x.id where i.id_receta == id
                            select new
                            {
                                i.id,
                                i.descripcion
                            };
            return Json(lista,JsonRequestBehavior.AllowGet);

            }catch(Exception e)
            {
                return Json(e.Message);
            }
        }


        public ActionResult Crear(int id)
        {
            var nombre = bd.receta.FirstOrDefault(x => x.id == id);
            ViewBag.titulo = nombre.titulo;
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public ActionResult Crear(ingrediente ingrediente)
        {
            if (ingrediente.id == 0)
            {
                bd.ingrediente.Add(ingrediente);
                bd.SaveChanges();
                return Json("Ingrediente agregado");
            }
            else
            {
                ingrediente i = bd.ingrediente.FirstOrDefault(x => x.id == ingrediente.id);
                i.descripcion = ingrediente.descripcion;
                bd.SaveChanges();
                return Json("Ingrediente actualizado");
            }
        }

        public ActionResult Buscar(int id)
        {
            var ingrediente = from i in bd.ingrediente
                        join x in bd.receta on i.id_receta equals x.id where i.id == id
                        select new
                        {
                            i.id,
                            i.descripcion
                        };
            return Json(ingrediente, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            var ingrediente = bd.ingrediente.FirstOrDefault(x => x.id == id);
            bd.ingrediente.Remove(ingrediente);
            bd.SaveChanges();
            return Json("Se elimino correctamente");
        }

    }
}