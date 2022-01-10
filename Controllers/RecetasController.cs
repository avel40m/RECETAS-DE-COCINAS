using RecetasCocinas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecetasCocinas.Controllers
{
    [Authorize]
    public class RecetasController : Controller
    {
        private recetasEntity bd = new recetasEntity();
        // GET: Recetas
   
        public ActionResult Index(String buscar)
        {
            var receta = from r in bd.receta select r;
            if (!String.IsNullOrEmpty(buscar))
            {
                receta = receta.Where(x => x.titulo.Contains(buscar));
            }
            return View(receta);
        }

        public ActionResult Crear()
        {
            ViewBag.id_tipo = new SelectList(bd.tipo, "id", "descripcion");
            return View();
        }
        [HttpPost]
        public ActionResult Crear(receta receta)
        {
            receta.fecha = DateTime.Now;
            receta.publicar = false;

            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any()) 
            {
                var imagen = System.Web.HttpContext.Current.Request.Files["imagen"];
                string logoPath = Server.MapPath("~/Content/Imagen/");
                string fileName = imagen.FileName;
                imagen.SaveAs(logoPath + fileName);

                receta.imagen = "~/Content/Imagen/" + imagen.FileName;
                bd.receta.Add(receta);
                bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Detalles(int id)
        {

            ViewBag.selectTipo = bd.tipo.ToList();

            ViewBag.id = bd.receta.SingleOrDefault(x => x.id == id).id;

            ViewBag.receta = bd.receta.FirstOrDefault(x => x.id == id);

            var idtipo = bd.receta.SingleOrDefault(r => r.id == id).id_tipo;
            ViewBag.tipo = bd.tipo.FirstOrDefault(x => x.id == idtipo);

            var publicar = bd.receta.SingleOrDefault(x => x.id == id).publicar;
            
            if(publicar == true)
            {
                ViewBag.publicar = "Está publicada";
            }
            else
            {
                ViewBag.publicar = "No Está publicada";
            }

            var valoracion = bd.receta.SingleOrDefault(x => x.id == id).valoracion;
            ViewBag.valoracion = valoracion;

            List < ingrediente > listaIngredietes = bd.ingrediente.Where(x => x.id_receta == id).ToList();
            ViewBag.ListIngrediente = listaIngredietes;

            List<preparacion> listaPreparaciones = bd.preparacion.Where(x => x.id_receta == id).ToList();
            ViewBag.ListPreparacion = listaPreparaciones;
            return View();
        }

        [HttpPost]
        public ActionResult Publicacion(int id)
        {
            
            var estado= bd.receta.SingleOrDefault(x => x.id == id).publicar;
            ViewBag.estado = from r in bd.receta where r.id == id select r.publicar;
            if (estado == true)
            {
                receta receta = bd.receta.FirstOrDefault(x => x.id == id);
                receta.publicar = false;
                bd.SaveChanges();
                return Json("No se publico");
            }
            else
            {

                receta receta = bd.receta.FirstOrDefault(x => x.id == id);
                receta.publicar = true;
                bd.SaveChanges();
                return Json("Se publico");
            }
        }


        public ActionResult Buscar(int id)
        {
            var receta = from r in bd.receta join t in bd.tipo
                         on r.id_tipo equals t.id where r.id == id
                         select new
                         { 
                           id =  r.id ,
                           titulo =  r.titulo,
                           Rdescripcion =  r.descripcion,
                           Tdescripcion =  t.id,
                           valoracion = r.valoracion
                         };
            return Json(receta,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Actualizar(receta receta)
        {
            receta r = bd.receta.FirstOrDefault(x => x.id == receta.id);
            r.titulo = receta.titulo;
            r.descripcion = receta.descripcion;
            r.id_tipo = receta.id_tipo;
            r.valoracion = receta.valoracion;
            bd.SaveChanges();
            return Json("Se Actualizo correctamente");
        }

        public ActionResult Eliminar(int id)
        {
            receta receta = bd.receta.FirstOrDefault(x => x.id == id);
            bd.receta.Remove(receta);
            bd.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}