using Atestados.Datos.Modelo;
using Atestados.Negocios.Negocios;
using Atestados.Objetos.Dtos;
using Atestados.Objetos.Dtos.Atestados;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace Atestados.UI.Controllers.Atestados
{
    public class ProyectoGradGalarController : Controller
    {
        private AtestadosEntities db = new AtestadosEntities();
        private InformacionAtestado infoAtestado = new InformacionAtestado();
        private InformacionGeneral infoGeneral = new InformacionGeneral();
        public static List<ArchivoDTO> archivosOld = null;
        private string Rubro = "Proyectos de graduación galardonados"; // esta como otras obras profesionales

        public ActionResult Index()
        {
            return View(infoAtestado.CargarAtestadosDeTipo(infoAtestado.ObtenerIDdeRubro(Rubro)));
        }

        // GET: ProyectosGradGalar/Ver
        public ActionResult Ver(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            AtestadoDTO atestado = infoAtestado.CargarAtestado(id);
            if (atestado == null)
                return HttpNotFound();
            return View(atestado);
        }

        public ActionResult Crear()
        {
            ProyectoGalarDTO proyecto = new ProyectoGalarDTO();
            proyecto.FechaInicio = DateTime.Now;
            proyecto.FechaFinal = DateTime.Now;
            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", infoAtestado.ObtenerIDdePais("costa rica"));
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersonaPorTipo(infoAtestado.ObtenerIDdeRubro(Rubro), (int)Session["UsuarioID"]);
            // Limpiar las listas de archivos y autores por si tienen basura.
            Session["Archivos"] = new List<ArchivoDTO>();

            return View(proyecto);
        }

        // POST: ProyectoGradGalar/crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "AutoresCheck,AutoresEqAtestadoID,Codigo,FechaInicio,FechaFinal,Nombre,NumeroAutores,Observaciones,HoraCreacion,Enviado,Descargado,CantidadHoras,Lugar,CatalogoTipo,Enlace,PaisID,PersonaID,RubroID,Archivo")] AtestadoDTO atestado)
        {
            if (ModelState.IsValid)
            {
                atestado.PersonaID = (int)Session["UsuarioID"];
                atestado.RubroID = infoAtestado.ObtenerIDdeRubro(Rubro);
                atestado.PaisID = infoAtestado.ObtenerIDdePais("costa rica"); // Costa Rica
                atestado.NumeroAutores = 1;
                Atestado a = AutoMapper.Mapper.Map<AtestadoDTO, Atestado>(atestado);
                infoAtestado.GuardarAtestado(a);
                atestado.AtestadoID = a.AtestadoID;
                Fecha fecha = AutoMapper.Mapper.Map<AtestadoDTO, Fecha>(atestado);
                infoAtestado.GuardarFecha(fecha);

                List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];
                foreach (ArchivoDTO archivo in archivos)
                {
                    Archivo ar = AutoMapper.Mapper.Map<ArchivoDTO, Archivo>(archivo);
                    ar.AtestadoID = a.AtestadoID;
                    infoAtestado.GuardarArchivo(ar);
                }

                Session["Archivos"] = new List<ArchivoDTO>();

                return RedirectToAction("Crear");
            }
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersonaPorTipo(infoAtestado.ObtenerIDdeRubro(Rubro), (int)Session["UsuarioID"]);
            return View(atestado);
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
                atestado.Fecha.FechaID = atestado.AtestadoID;
                atestado.Fecha.FechaInicio = DateTime.Now;
                infoAtestado.EditarFecha(AutoMapper.Mapper.Map<FechaDTO, Fecha>(atestado.Fecha));
                atestado.HoraCreacion = DateTime.Now;
                atestado.Archivos = infoAtestado.CargarArchivosDeAtestado(atestado.AtestadoID);
                Atestado atestado_mapped = AutoMapper.Mapper.Map<AtestadoDTO, Atestado>(atestado);
                infoAtestado.EditarAtestado(atestado_mapped);

                // Agregar archivos
                AtestadoShared.obj.editarArchivos(archivosOld, archivos, infoAtestado, atestado_mapped);

                Session["Archivos"] = new List<ArchivoDTO>();
                archivosOld = new List<ArchivoDTO>();

                return RedirectToAction("Crear");
            }

            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", infoAtestado.ObtenerIDdePais("costa rica"));
            ViewBag.RubroID = new SelectList(db.Rubro, "RubroID", "Nombre");
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersona((int)Session["UsuarioID"]);
            return View(atestado);
        }

        public ActionResult Editar(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // Asegurarse que los autores y archivos no son nulos.
            if (Session["Archivos"] == null)
                Session["Archivos"] = new List<ArchivoDTO>();

            // Cargar el atestado y verificar que no es nulo.
            Atestado atestado = infoAtestado.CargarAtestadoParaEditar(id);
            if (atestado == null)
                return HttpNotFound();

            AtestadoDTO atestado_mapped = AutoMapper.Mapper.Map<Atestado, AtestadoDTO>(atestado);

            // Cargar y poner los datos adicionales del formulario en la vista.
            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", infoAtestado.ObtenerIDdePais("costa rica"));
            ViewBag.RubroID = new SelectList(db.Rubro, "RubroID", "Nombre", atestado.RubroID);
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersona((int)Session["UsuarioID"]);
            // Guardar el estado de los archivos previos a su edición.
            archivosOld = new List<ArchivoDTO>();
            List<ArchivoDTO> tmpList = infoAtestado.CargarArchivosDeAtestado(id);
            tmpList.ForEach((item) => { archivosOld.Add(new ArchivoDTO(item)); });
            Session["Archivos"] = infoAtestado.CargarArchivosDeAtestado(id);
            Session["Autores"] = infoAtestado.CargarAutoresAtestado(atestado.AtestadoID);
            indexarListas();
            return View(atestado_mapped);
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
