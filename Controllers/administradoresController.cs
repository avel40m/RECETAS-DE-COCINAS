using RecetasCocinas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RecetasCocinas.Controllers
{
    public class administradoresController : Controller
    {
        private recetasEntity bd = new recetasEntity();
        public ActionResult Index(string mensaje = "")
        {
            ViewBag.mensaje = mensaje;
            return View();
        }
        [HttpPost]
        public ActionResult Index(string usuario,string clave)
        {
            if(usuario == null || clave == null)
            {
                return RedirectToAction("Index",new {mensaje = "Complete los campos" });
            }
            var admin = bd.administrador.FirstOrDefault(x => x.usuario == usuario && x.clave == clave);

            if(admin != null)
            {
                FormsAuthentication.SetAuthCookie(admin.usuario, true);
                return RedirectToAction("Index", "Recetas");
            }
            else
            {
                return RedirectToAction("Index",new { mensaje = "No se encontro el usuario" });
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}