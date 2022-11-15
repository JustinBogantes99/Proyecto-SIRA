using Atestados.Negocios.Negocios;
using Atestados.Objetos.Clases;
using Atestados.Objetos.Dtos;
using Atestados.UIProcess.Clases;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using static Atestados.Utilitarios.Constantes.Constantes;

namespace Atestados.UI.Controllers
{
    public class LoginController : Controller
    {
        #region Instancias
        //Instancia a la clase de Servicios en UIProcess
        private AccesoSeguridad ServiciosSeguridad = new AccesoSeguridad();
        //Objeto de retorno
        oRespuesta<string> respuestaValidacion = new oRespuesta<string>();

        //Debido que el usuario aun no está logueado, se usa un dummy inicial al logueo.
        //Luego de loguearse, cambia los datos al usuario que se encuentra logueado.
        //int idUsuarioLogueado = 1;
        //string usuarioLogueado = "sistemaLogueo@itcr.ac.cr";
        public string sessionId = ConfigurationManager.AppSettings["SessionID"];
        private InformacionGeneral info = new InformacionGeneral();

        public void InitializeController(RequestContext context)
        {
            base.Initialize(context);
        }
        #endregion

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string email, string contrasena)
        {
            if (ModelState.IsValid)
            {
                RESULTADO_LOGIN validacion = info.ValidarUsuario(email, contrasena);
                if (validacion == RESULTADO_LOGIN.Exito)
                {
                    // Obtener el usuario y crear su sesión.
                    UsuarioDTO usuario = info.UsuarioPorEmail(email);
                    CrearSesion(usuario);
                    // Llevar al usuario a su panel principal según su tipo.
                    if (usuario.TipoUsuario == 1)
                        return RedirectToAction("Index", "Administrador");
                    if (usuario.TipoUsuario == 2)
                        return RedirectToAction("Index", "Comision");
                    else
                        return RedirectToAction("Index", "Funcionario");
                }
                else if (validacion == RESULTADO_LOGIN.UsuarioNoExiste)
                {
                    ViewBag.Error = "El usuario no existe en el sistema";
                }
                else if (validacion == RESULTADO_LOGIN.ContrasenaIncorrecta)
                {
                    ViewBag.Error = "La contraseña ingresada es incorrecta";
                }
                return View();
            }
            else
            {
                return View();
            }
        }

        public void CrearSesion(UsuarioDTO usuario)
        {
            Session["Usuario"] = usuario;
            Session["NombreCorto"] = usuario.NombreCorto();
            Session["NombreCompleto"] = usuario.NombreCompleto();
            Session["NombreUsuario"] = usuario.Email;
            Session["UsuarioID"] = usuario.UsuarioID;
            Session["TipoUsuario"] = usuario.TipoUsuario;
            Session["TipoUsuarioNombre"] = usuario.TipoUsuarioToStr();
        }

        public ActionResult PaginaInvalida()
        {
            return View();
        }

        //Cerrar sesión de aplicación
        public ActionResult CerrarSesion()
        {
            //inserta en bitácora del sist ema de seguridad la salida
            if (Session["Usuario"] != null)
            {
                // ServiciosSeguridad.InsertarBitacoraSistema("Cierre de sesión Usuario : " + Session[ConfigurationManager.AppSettings["UsuarioLogueado"]].ToString(), Constantes.CodigosBitacora.INACTIVAR, "LoginController - Interno", "CerrarSesion", false, Convert.ToInt32(Session[ConfigurationManager.AppSettings["CodigoUsuarioLogueado"]]), Session[ConfigurationManager.AppSettings["UsuarioLogueado"]].ToString(), Utilitarios.Clases.Utilitarios.GetIpAddress(), Session[sessionId].ToString(), null);
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

            return RedirectToAction("Index", "Login");
        }
    }
}