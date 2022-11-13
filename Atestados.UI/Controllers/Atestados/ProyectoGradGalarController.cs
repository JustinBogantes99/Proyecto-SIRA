using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Atestados.Datos.Modelo;
using Atestados.Negocios.Negocios;
using Atestados.Objetos.Dtos;
using Newtonsoft.Json;


namespace Atestados.UI.Controllers.Atestados
{
    public class ProyectoGradGalarController : Controller
    {
        private AtestadosEntities db = new AtestadosEntities();
        private InformacionAtestado infoAtestado = new InformacionAtestado();
        private InformacionGeneral infoGeneral = new InformacionGeneral();
        public static List<ArchivoDTO> archivosOld = null;
        private string Rubro = "Proyectos de graduación galardonados";

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

        // GET: ProyectosGradGalar
        public ActionResult Index()
        {
            return View(infoAtestado.CargarAtestadosDeTipo(infoAtestado.ObtenerIDdeRubro(Rubro)));
        }

        // GET: ProyectosGradGalar/Ver
        public ActionResult Ver(int? id)
        {
            UsuarioDTO usuario = (UsuarioDTO)Session["Usuario"];

            // Validar los datos ingresados.
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            AtestadoDTO atestado = infoAtestado.CargarAtestado(id);

            if (atestado == null)
                return HttpNotFound();

            // Asignar los datos para visualizar.
            ViewBag.Autores = infoAtestado.CargarAutoresAtestado(id);
            ViewBag.NotasPonderadas = infoAtestado.CargarNotasPonderadasAutores(id);
            ViewBag.Puntos = infoAtestado.CargarPuntosAutores(id);
            Session["TipoUsuario"] = usuario.TipoUsuario;
            Session["idAtestado"] = id;
            Session["idUsuario"] = usuario.UsuarioID;
            return View(atestado);
        }
        //GET: ProyectoGradGalar/Crear
        public ActionResult Crear()
        {
            AtestadoDTO atestado = new AtestadoDTO();
            atestado.FechaInicio = DateTime.Now;
            atestado.FechaFinal = DateTime.Now;
            atestado.NumeroAutores = 1;

            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", infoAtestado.ObtenerIDdePais("costa rica"));
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersonaPorTipo(infoAtestado.ObtenerIDdeRubro(Rubro), (int)Session["UsuarioID"]);

            // Limpiar las listas de archivos y autores por si tienen basura.
            Session["Archivos"] = new List<ArchivoDTO>();

            return View(atestado);
        }

        // POST: ProyectoGradGalar/crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "AtestadoID,Nombre,NumeroAutores,Observaciones,HoraCreacion,Enviado,FechaFinal,Descargado,CantidadHoras,Lugar,CatalogoTipo,Enlace,PaisID,PersonaID,RubroID,AutoresEq,AutoresCheck")] AtestadoDTO atestado)
        {
            if (ModelState.IsValid)
            {
                List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];

                // Obtener el id del usuario que está agregando el atestado.
                atestado.PersonaID = (int)Session["UsuarioID"];
                atestado.RubroID = infoAtestado.ObtenerIDdeRubro(Rubro);
                atestado.PaisID = infoAtestado.ObtenerIDdePais("costa rica"); // Costa Rica

                // Mappear el atestado una vez que está completo.
                // Esta operación es muy frágil, y podría llevar a errores de llaves en la BD.
                Atestado atestado_mapped = AutoMapper.Mapper.Map<AtestadoDTO, Atestado>(atestado);
                infoAtestado.GuardarAtestado(atestado_mapped);
                // Obtener y guardar información adicional del atestado.
                atestado.AtestadoID = atestado_mapped.AtestadoID;
                Fecha fecha = AutoMapper.Mapper.Map<AtestadoDTO, Fecha>(atestado);
                infoAtestado.GuardarFecha(fecha);
                _ = (List<ArchivoDTO>)Session["Archivos"];
                foreach (ArchivoDTO archivo in archivos)
                {
                    Archivo ar = AutoMapper.Mapper.Map<ArchivoDTO, Archivo>(archivo);
                    ar.AtestadoID = atestado_mapped.AtestadoID;
                    infoAtestado.GuardarArchivo(ar);
                }

                Session["Archivos"] = new List<ArchivoDTO>();

                return RedirectToAction("Crear");
            }
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersonaPorTipo(infoAtestado.ObtenerIDdeRubro(Rubro), (int)Session["UsuarioID"]);
            return View(atestado);
        }
        // GET: ProyectoGradGalar/Editar
        public ActionResult Editar(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // Asegurarse que los autores y archivos no son nulos.
            if (Session["Autores"] == null)
                Session["Autores"] = new List<AutorDTO>();
            if (Session["Archivos"] == null)
                Session["Archivos"] = new List<ArchivoDTO>();

            // Cargar el atestado y verificar que no es nulo.
            Atestado atestado = infoAtestado.CargarAtestadoParaEditar(id);
            if (atestado == null)
                return HttpNotFound();

            AtestadoDTO atestado_mapped = AutoMapper.Mapper.Map<Atestado, AtestadoDTO>(atestado);

            // Cargar y poner los datos adicionales del formulario en la vista.
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersonaPorTipo(infoAtestado.ObtenerIDdeRubro(Rubro), (int)Session["UsuarioID"]);
            // Guardar el estado de los archivos previos a su edición.
            archivosOld = new List<ArchivoDTO>();
            List<ArchivoDTO> tmpList = infoAtestado.CargarArchivosDeAtestado(id);
            tmpList.ForEach((item) => { archivosOld.Add(new ArchivoDTO(item)); });
            Session["Archivos"] = infoAtestado.CargarArchivosDeAtestado(id);
            indexarListas();
            return View(atestado_mapped);
        }
        // POST: ProyectoGradGalar/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Lugar,CantidadHoras,Archivos,CatalogoTipo,Archivos,AtestadoID,AtestadoXPersona,Editorial,Enlace,HoraCreacion,Nombre,NumeroAutores,Observaciones,PaisID,Persona,PersonaID,RubroID,Website,Fecha,DominioIdioma,Persona,Rubro,Pais,InfoEditorial,Archivo,AutoresEq,AutoresCheck")] AtestadoDTO atestado)
        {
            if (ModelState.IsValid)
            {
                List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];

                atestado.PersonaID = (int)Session["UsuarioID"];
                atestado.RubroID = infoAtestado.ObtenerIDdeRubro(Rubro);
                atestado.PaisID = infoAtestado.ObtenerIDdePais("costa rica"); // Costa Rica
                atestado.HoraCreacion = DateTime.Now;
                atestado.Archivos = infoAtestado.CargarArchivosDeAtestado(atestado.AtestadoID);
                atestado.AtestadoXPersona = AutoMapper.Mapper.Map<List<AtestadoXPersona>, List<AtestadoXPersonaDTO>>(infoAtestado.CargarAtestadoXPersonasdeAtestado(atestado.AtestadoID));
                Atestado atestado_mapped = AutoMapper.Mapper.Map<AtestadoDTO, Atestado>(atestado);
                infoAtestado.EditarAtestado(atestado_mapped);

                foreach (ArchivoDTO archivo in archivos)
                {
                    Archivo ar = AutoMapper.Mapper.Map<ArchivoDTO, Archivo>(archivo);
                    ar.AtestadoID = atestado.AtestadoID;
                    infoAtestado.GuardarArchivo(ar);
                }

                Session["Archivos"] = new List<ArchivoDTO>();

                return RedirectToAction("Crear");
            }
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersonaPorTipo(infoAtestado.ObtenerIDdeRubro(Rubro), (int)Session["UsuarioID"]);
            return View(atestado);
        }
        // Indexar la lista de archivos con números.
        private void indexarListas()
        {
            int cont = 1;
            List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];

            foreach (ArchivoDTO archivo in archivos)
                archivo.numArchivo = cont++;

            Session["Archivos"] = archivos;
        }

         
        // GET: ProyectosGradGalar/Borrar
        public ActionResult Borrar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atestado atestado = infoAtestado.CargarAtestadoParaBorrar(id);
            if (atestado == null)
            {
                return HttpNotFound();
            }
            ViewBag.Autores = infoAtestado.CargarAutoresAtestado(id);
            return View(atestado);
        }

        // POST: ProyectosGradGalar/Borrar
        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult Borrar(int id)
        {
            infoAtestado.BorrarAtestado(id);
            return RedirectToAction("Crear");
        }
    }
}
