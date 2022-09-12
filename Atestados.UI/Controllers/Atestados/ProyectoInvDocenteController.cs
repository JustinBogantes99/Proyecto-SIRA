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
using Atestados.Objetos.Dtos.Atestados;
using Newtonsoft.Json;
 

namespace Atestados.UI.Controllers.Atestados
{
    public class ProyectoInvDocenteController : Controller
    {

        private AtestadosEntities db = new AtestadosEntities();
        private InformacionAtestado infoAtestado = new InformacionAtestado();
        private InformacionGeneral infoGeneral = new InformacionGeneral();
        private string Rubro = "Proyectos de innovación docente"; // esta como otras obras profesionales
        public static List<ArchivoDTO> archivosOld = null;

        // GET: ProyectoInvDocente
        public ActionResult Index()
        {
            return View(infoAtestado.CargarAtestadosDeTipo(infoAtestado.ObtenerIDdeRubro(Rubro)));
        }

        // GET: ProyectoInvDocente/Ver
        public ActionResult Ver(int? id)
        {
            UsuarioDTO usuario = (UsuarioDTO)Session["Usuario"];

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

        // GET: ProyectoInvDocente/Crear
        public ActionResult Crear()
        {
            ProyectosInnDocDTO proyecto = new ProyectosInnDocDTO();
            proyecto.FechaInicio = DateTime.Now;
            proyecto.FechaFinal = DateTime.Now;
            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", infoAtestado.ObtenerIDdePais("costa rica"));
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersonaPorTipo(infoAtestado.ObtenerIDdeRubro(Rubro), (int)Session["UsuarioID"]);

            // Limpiar las listas de archivos y autores por si tienen basura.
            Session["Archivos"] = new List<ArchivoDTO>();// Limpiar las listas de archivos y autores por si tienen basura.
            Session["Autores"] = new List<AutorDTO>();
            return View(proyecto);
        }

        // POST: ProyectoInvDocente/crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "AtestadoID,Codigo,FechaInicio,FechaFinal,Nombre,NumeroAutores,Observaciones,HorasInvertidasTotales,HorasInvertidasXSemana,MesesInvertidos,Codigo,Lugar,CatalogoTipo,Enlace,PaisID,PersonaID,RubroID,AutoresEq,AutoresCheck")] ProyectosInnDocDTO atestado)
        {
            if (ModelState.IsValid)
                ModelState.AddModelError("AutoresCheck", "El Proyecto debe tener al menos un encargado.");
            else
            if (ModelState.IsValid)
            {
                List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];
                List<AutorDTO> autores = (List<AutorDTO>)Session["Autores"];
                
                // Obtener el id del usuario que está agregando el atestado.
                atestado.PersonaID = (int)Session["UsuarioID"];
                atestado.RubroID = infoAtestado.ObtenerIDdeRubro(Rubro);
                atestado.PaisID = infoAtestado.ObtenerIDdePais("costa rica");//si es necesario seleccionar pais se va
                                                                             // Mappear el atestado una vez que está completo.
                                                                             // Esta operación es muy frágil, y podría llevar a errores de llaves en la BD.
                Atestado atestado_mapped = AutoMapper.Mapper.Map<ProyectosInnDocDTO, Atestado>(atestado);
                infoAtestado.GuardarAtestado(atestado_mapped);
                // Obtener y guardar información adicional del atestado.
                atestado.AtestadoID = atestado_mapped.AtestadoID;
                Fecha fecha = AutoMapper.Mapper.Map<ProyectosInnDocDTO, Fecha>(atestado);
                infoAtestado.GuardarFecha(fecha);

                // Agregar archivos
                AtestadoShared.obj.guardarArchivos(archivos, infoAtestado, atestado_mapped);

                // Limpiar las variables de sesión que contienen a los archivos y autores.
                Session["Archivos"] = new List<ArchivoDTO>();
                Session["Autores"] = new List<AutorDTO>();
                return RedirectToAction("Crear");
            }
                
            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", atestado.PaisID);
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersonaPorTipo(infoAtestado.ObtenerIDdeRubro(Rubro), (int)Session["UsuarioID"]);
            return View(atestado);
        }
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
        // GET: ProyectoInvDocente/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProyectoInvDocente/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ProyectosInvEx/Borrar
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
            return View(atestado);
        }

        // POST: ProyectosInvEx/Borrar
        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult Borrar(int id)
        {
            infoAtestado.BorrarAtestado(id);
            return RedirectToAction("Crear");
        }
    }
}
