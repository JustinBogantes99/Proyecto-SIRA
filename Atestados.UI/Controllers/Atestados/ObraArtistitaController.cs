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
    public class ObraArtisticaController : Controller
    {
        private AtestadosEntities db = new AtestadosEntities();
        private InformacionAtestado infoAtestado = new InformacionAtestado();
        private InformacionGeneral infoGeneral = new InformacionGeneral();
        private string Rubro = "Obras artísticas"; // esta como otras obras profesionales
        public static List<ArchivoDTO> archivosOld = null;

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

        // GET: ObraArtistica
        public ActionResult Index()
        {
            return View(infoAtestado.CargarAtestadosDeTipo(infoAtestado.ObtenerIDdeRubro(Rubro)));
        }

        // GET: ObraArtistica/Ver
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

        // GET: ObraArtistica/Crear
        public ActionResult Crear()
        {
            AtestadoDTO atestado = new AtestadoDTO();
            atestado.FechaInicio = DateTime.Now;
            atestado.FechaFinal = DateTime.Now;
            atestado.NumeroAutores = 1;

            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", infoAtestado.ObtenerIDdePais("costa rica"));
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersonaPorTipo(infoAtestado.ObtenerIDdeRubro(Rubro), (int)Session["UsuarioID"]);

            //Agregar Usuario como autor
            UsuarioDTO usuario = (UsuarioDTO)Session["Usuario"];
            AutorDTO autorUsuario = new AutorDTO();
            autorUsuario.Nombre = usuario.Nombre;
            autorUsuario.PrimerApellido = usuario.PrimerApellido;
            autorUsuario.SegundoApellido = usuario.SegundoApellido;
            autorUsuario.Email = usuario.Email;
            autorUsuario.PersonaID = usuario.UsuarioID;
            autorUsuario.Porcentaje = 100;
            autorUsuario.esFuncionario = true;
            autorUsuario.porcEquitativo = false;
            autorUsuario.numAutor = 1;

            // Limpiar las listas de archivos y autores por si tienen basura.
            Session["Autores"] = new List<AutorDTO> { autorUsuario };
            Session["Archivos"] = new List<ArchivoDTO>();

            return View(atestado);
        }

        // POST: ObraArtistica/Crear
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "AtestadoID,Nombre,NumeroAutores,Observaciones,HoraCreacion,Enviado,FechaFinal,Descargado,CantidadHoras,Lugar,CatalogoTipo,Enlace,PaisID,PersonaID,RubroID,AutoresEq,AutoresCheck")] AtestadoDTO atestado)
        {
            // Check manual para determinar si hay al menos un autor ingresado.
            if (!atestado.AutoresCheck)
                ModelState.AddModelError("AutoresCheck", "El libro debe tener al menos un autor.");
            else
            if (ModelState.IsValid)
            {
                List<AutorDTO> autores = (List<AutorDTO>)Session["Autores"];
                List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];

                // Obtener el id del usuario que está agregando el atestado.
                atestado.PersonaID = (int)Session["UsuarioID"];
                atestado.RubroID = infoAtestado.ObtenerIDdeRubro(Rubro);
                atestado.NumeroAutores = autores.Count();
                // Mappear el atestado una vez que está completo.
                // Esta operación es muy frágil, y podría llevar a errores de llaves en la BD.
                Atestado atestado_mapped = AutoMapper.Mapper.Map<AtestadoDTO, Atestado>(atestado);
                infoAtestado.GuardarAtestado(atestado_mapped);
                // Obtener y guardar información adicional del atestado.
                atestado.AtestadoID = atestado_mapped.AtestadoID;
                Fecha fecha = AutoMapper.Mapper.Map<AtestadoDTO, Fecha>(atestado); 
                infoAtestado.GuardarFecha(fecha);

                // Agregar archivos
                AtestadoShared.obj.guardarArchivos(archivos, infoAtestado, atestado_mapped);

                // Agregar autores
                AtestadoShared.obj.guardarAutores(autores, infoGeneral, infoAtestado, atestado.AutoresEq, atestado_mapped);

                // Limpiar las variables de sesión que contienen a los archivos y autores.
                Session["Archivos"] = new List<ArchivoDTO>();
                Session["Autores"] = new List<AutorDTO>();

                return RedirectToAction("Crear");
            }
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersonaPorTipo(infoAtestado.ObtenerIDdeRubro(Rubro), (int)Session["UsuarioID"]);
            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", atestado.PaisID);
            return View(atestado);
        }

        // GET: ObraArtistica/Editar
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
            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", atestado.PaisID);
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersonaPorTipo(infoAtestado.ObtenerIDdeRubro(Rubro), (int)Session["UsuarioID"]);
            // Guardar el estado de los archivos previos a su edición.
            archivosOld = new List<ArchivoDTO>();
            List<ArchivoDTO> tmpList = infoAtestado.CargarArchivosDeAtestado(id);
            tmpList.ForEach((item) => { archivosOld.Add(new ArchivoDTO(item)); });
            Session["Archivos"] = infoAtestado.CargarArchivosDeAtestado(id);
            Session["Autores"] = infoAtestado.CargarAutoresAtestado(atestado.AtestadoID);
            indexarListas();
            return View(atestado_mapped);
        }

        // Indexar las listas de autores y archivos con números.
        private void indexarListas()
        {
            int cont = 1;
            List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];
            List<AutorDTO> autores = (List<AutorDTO>)Session["Autores"];

            foreach (ArchivoDTO archivo in archivos)
                archivo.numArchivo = cont++;

            cont = 1;
            foreach (AutorDTO autor in autores)
                autor.numAutor = cont++;

            Session["Archivos"] = archivos;
            Session["Autores"] = autores;
        }

        // POST: ObraArtistica/Editar
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Lugar,CantidadHoras,Archivos,CatalogoTipo,Archivos,AtestadoID,AtestadoXPersona,Editorial,Enlace,HoraCreacion,Nombre,NumeroAutores,Observaciones,PaisID,Persona,PersonaID,RubroID,Website,Fecha,DominioIdioma,Persona,Rubro,Pais,InfoEditorial,Archivo,AutoresEq,AutoresCheck")] AtestadoDTO atestado)
        {                                          
            // Check manual para determinar si hay al menos un autor ingresado.
            if (!atestado.AutoresCheck)
                ModelState.AddModelError("AutoresCheck", "El libro debe tener al menos un autor.");
            else if (ModelState.IsValid)
            {
                List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];
                List<AutorDTO> autores = (List<AutorDTO>)Session["Autores"];

                atestado.PersonaID = (int)Session["UsuarioID"];
                atestado.RubroID = infoAtestado.ObtenerIDdeRubro(Rubro);
                atestado.Fecha.FechaID = atestado.AtestadoID;
                infoAtestado.EditarFecha(AutoMapper.Mapper.Map<FechaDTO, Fecha>(atestado.Fecha));
                atestado.HoraCreacion = DateTime.Now;
                atestado.Archivos = infoAtestado.CargarArchivosDeAtestado(atestado.AtestadoID);
                atestado.AtestadoXPersona = AutoMapper.Mapper.Map<List<AtestadoXPersona>, List<AtestadoXPersonaDTO>>(infoAtestado.CargarAtestadoXPersonasdeAtestado(atestado.AtestadoID));
                atestado.NumeroAutores = autores.Count();
                Atestado atestado_mapped = AutoMapper.Mapper.Map<AtestadoDTO, Atestado>(atestado);
                infoAtestado.EditarAtestado(atestado_mapped);

                // Agregar archivos
                AtestadoShared.obj.editarArchivos(archivosOld, archivos, infoAtestado, atestado_mapped);

                // Agregar autores
                AtestadoShared.obj.editarAutores(autores, infoGeneral, infoAtestado, atestado.AutoresEq, atestado_mapped);

                Session["Archivos"] = new List<ArchivoDTO>();
                Session["Autores"] = new List<AutorDTO>();
                archivosOld = new List<ArchivoDTO>();

                return RedirectToAction("Crear");
            }

            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", atestado.PaisID);
            ViewBag.AtestadoID = new SelectList(db.Fecha, "FechaID", "FechaID", atestado.AtestadoID);
            ViewBag.AtestadoID = new SelectList(db.InfoEditorial, "InfoEditorialID", "Editorial", atestado.AtestadoID);
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersonaPorTipo(infoAtestado.ObtenerIDdeRubro(Rubro), (int)Session["UsuarioID"]);
            ViewBag.Autores = infoAtestado.CargarAutoresAtestado(atestado.AtestadoID);
            return View(atestado);
        }

        // GET: ObraArtistica/Borrar
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

        // POST: ObraArtistica/Borrar
        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult Borrar(int id)
        {
            infoAtestado.BorrarAtestado(id);
            return RedirectToAction("Crear");
        }
    }
}
