using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Atestados.Datos.Modelo;
using Atestados.Negocios.Negocios;
using Atestados.Objetos.Dtos;
using Newtonsoft.Json;

namespace Atestados.UI.Controllers.Atestados
{
    public class EducacionContinuadaController : Controller
    {
        private AtestadosEntities db = new AtestadosEntities();
        private InformacionAtestado infoAtestado = new InformacionAtestado();
        private InformacionGeneral infoGeneral = new InformacionGeneral();
        private readonly string Rubro = "Cursos de educación continuada";
        public static List<ArchivoDTO> archivosOld = null;

        // GET: EducacionContinuada
        public ActionResult Index()
        {
            return View(infoAtestado.CargarAtestadosDeTipo(infoAtestado.ObtenerIDdeRubro(Rubro)));
        }

        // GET: EducacionContinuada/Ver
        public ActionResult Ver(int? id)
        {
            UsuarioDTO usuario = (UsuarioDTO)Session["Usuario"];

            // Validar los datos ingresados.
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            AtestadoDTO atestado = infoAtestado.CargarAtestado(id);

            if (atestado == null || atestado.RubroID != infoAtestado.ObtenerIDdeRubro(Rubro))
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

        // GET: EducacionContinuada/Crear
        public ActionResult Crear()
        {
            AtestadoDTO educacionContinuada = new AtestadoDTO();
            educacionContinuada.FechaFinal = DateTime.Now;
            educacionContinuada.FechaInicio = DateTime.Now;
            educacionContinuada.NumeroAutores = 1;
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersonaPorTipo(infoAtestado.ObtenerIDdeRubro(Rubro), (int)Session["UsuarioID"]);

            // Limpiar las listas de archivos y autores por si tienen basura.
            Session["Autores"] = new List<AutorDTO>();
            Session["Archivos"] = new List<ArchivoDTO>();

            return View(educacionContinuada);
        }

        // POST: EducacionContinuada/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "Fecha,Archivos,AtestadoID,AtestadoXPersona,Enlace,HoraCreacion,Nombre,CantidadHoras,Observaciones,Persona,PersonaID,RubroID,FechaInicio,FechaFinal,Lugar,NumeroAutores,AutoresEq,AutoresCheck")] AtestadoDTO atestado)
        {
            // Check manual para determinar si hay al menos un autor ingresado.
            if (!atestado.AutoresCheck)
                ModelState.AddModelError("AutoresCheck", "El curso de educación continuada debe tener al menos un autor.");
            else
            if (ModelState.IsValid)
            {
                List<AutorDTO> autores = (List<AutorDTO>)Session["Autores"];
                List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];

                atestado.PersonaID = (int)Session["UsuarioID"];
                atestado.RubroID = infoAtestado.ObtenerIDdeRubro(Rubro);
                atestado.PaisID = infoAtestado.ObtenerIDdePais("costa rica"); // Costa Rica
                Atestado atestado_mapped = AutoMapper.Mapper.Map<AtestadoDTO, Atestado>(atestado);
                infoAtestado.GuardarAtestado(atestado_mapped);
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
            return View(atestado);
        }

        // GET: EducacionContinuada/Borrar
        public ActionResult Borrar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atestado atestado = infoAtestado.CargarAtestadoParaBorrar(id);
            if (atestado == null || atestado.RubroID != infoAtestado.ObtenerIDdeRubro(Rubro))
            {
                return HttpNotFound();
            }
            ViewBag.Autores = infoAtestado.CargarAutoresAtestado(id);
            return View(atestado);
        }

        // POST: EducacionContinuada/Borrar
        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult Borrar(int id)
        {
            infoAtestado.BorrarAtestado(id);
            return RedirectToAction("Crear");
        }

        // GET: EducacionContinuada/Editar
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Asegurarse que los autores y archivos no son nulos.
            if (Session["Autores"] == null)
                Session["Autores"] = new List<AutorDTO>();
            if (Session["Archivos"] == null)
                Session["Archivos"] = new List<ArchivoDTO>();

            Atestado atestado = infoAtestado.CargarAtestadoParaEditar(id);
            AtestadoDTO atestado_mapped = AutoMapper.Mapper.Map<Atestado, AtestadoDTO>(atestado);
            if (atestado == null || atestado.RubroID != infoAtestado.ObtenerIDdeRubro(Rubro))
            {
                return HttpNotFound();
            }
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

        // POST: EducacionContinuada/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Lugar,Archivos,CantidadHoras,AtestadoID,AtestadoXPersona,Enlace,HoraCreacion,Nombre,NumeroAutores,Observaciones,PaisID,Persona,PersonaID,RubroID,Website,Fecha,DominioIdioma,Persona,Rubro,Pais,InfoEditorial,Archivo,AutoresEq,AutoresCheck")] AtestadoDTO atestado)
        {
            // Check manual para determinar si hay al menos un autor ingresado.
            if (!atestado.AutoresCheck)
                ModelState.AddModelError("AutoresCheck", "El libro debe tener al menos un autor.");
            else if (ModelState.IsValid)
            {
                List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];
                List<AutorDTO> autores = (List<AutorDTO>)Session["Autores"];

                atestado.PersonaID = (int)Session["UsuarioID"]; // cambiar por sesion
                atestado.RubroID = infoAtestado.ObtenerIDdeRubro(Rubro);
                atestado.PaisID = infoAtestado.ObtenerIDdePais("costa rica"); // Costa Rica
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

            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersonaPorTipo(infoAtestado.ObtenerIDdeRubro(Rubro), (int)Session["UsuarioID"]);
            ViewBag.Autores = infoAtestado.CargarAutoresAtestado(atestado.AtestadoID);
            return View(atestado);
        }
    }
}