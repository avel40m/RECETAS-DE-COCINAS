using RecetasCocinas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecetasCocinas.Controllers
{
    [Authorize]
    public class tipoController : Controller
    {
        private recetasEntity bd = new recetasEntity();
        // GET: tipo
        public ActionResult Index()
        {
            var tipo = bd.tipo.ToList();
            return View(tipo);
        }

        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Crear(tipo t)
        {
            if (ModelState.IsValid)
            { 
            bd.tipo.Add(t);
            bd.SaveChanges();
            return RedirectToAction("Index");
            }

            return View();
        }
        public ActionResult Actualizar(int id)
        {
            tipo tipo = bd.tipo.Find(id);
            return View(tipo);
        }

        [HttpPost]
        public ActionResult Actualizar(tipo t)
        {
            if (ModelState.IsValid)
            {
            var tipo = bd.tipo.FirstOrDefault(x => x.id == t.id);

            tipo.descripcion = t.descripcion;
            bd.SaveChanges();
            return RedirectToAction("Index");
            }

            return View(t);
        }

        public ActionResult Eliminar(int id)
        {
            tipo t = bd.tipo.FirstOrDefault(tipo => tipo.id == id);
            bd.tipo.Remove(t);
            bd.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}