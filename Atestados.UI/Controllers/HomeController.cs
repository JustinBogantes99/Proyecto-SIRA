using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atestados.UIProcess.Clases;
using Atestados.Objetos.Clases;
using Atestados.Utilitarios.Clases;
using Atestados.Utilitarios.Constantes;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System.Web.WebPages;
using Atestados.Objetos.Dtos;
using Atestados.Negocios.Negocios;
using System.Web.Routing;

namespace Atestados.UI.Controllers
{
    public class HomeController : Controller
    {
        #region Instancias
        //Instancia a la clase de Servicios en UIProcess
        private AccesoSeguridad ServiciosSeguridad = new AccesoSeguridad();
        //Objeto de retorno
        oRespuesta<string> respuestaValidacion = new oRespuesta<string>();

        //Debido que el usuario aun no está logueado, se usa un dummy inicial al logueo.
        //Luego de loguearse, cambia los datos al usuario que se encuentra logueado.
        public int idUsuarioLogueado = 1;
        public string usuarioLogueado = "sistemaLogueo@itcr.ac.cr";
        public string sessionNombreUsuario = "NombreUsuario";
        public string sessionId = ConfigurationManager.AppSettings["SessionID"];

        private InformacionGeneral info = new InformacionGeneral();
        #endregion

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Si la sesión es null, se redirige a la página de login
            if (Session["Usuario"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"controller", "Login"},
                        {"action", "Index"}
                    }
                );
                return;
            }
        }

        [Route("")]
        [Route("Home")]
        [Route("Home/Index")]
        public ActionResult Index()
        {
            if (Session["Usuario"] != null)
            {
                UsuarioDTO usuario = (UsuarioDTO)Session["Usuario"];
                ViewBag.NombreUsuario = usuario.Email;
                ViewBag.NombreCompleto = usuario.NombreCompleto();
                Session["NombreCorto"] = usuario.NombreCorto();
                Session["NombreCompleto"] = usuario.NombreCompleto();
                Session["NombreUsuario"] = usuario.Email;
                Session["UsuarioID"] = usuario.UsuarioID;
                Session["TipoUsuario"] = usuario.TipoUsuario;
                return RedirectToAction("Index", "Funcionario");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(UsuarioDTO usuario)
        {
            if (ModelState.IsValid)
            {
                var existe = info.UsuarioPorEmail(usuario.Email);
                if(existe == null)
                {
                    usuario.CategoriaActual = 0;
                    usuario.TipoUsuario = 0;
                    info.CrearUsuario(usuario);
                    return RedirectToAction("Index", "Home");
                } else
                {
                    return View();
                }
            }
            return View();
        }

    }
}