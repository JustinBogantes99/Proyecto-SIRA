using Atestados.Datos.Modelo;
using Atestados.Negocios.Negocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atestados.Objetos.Dtos;
using System.Net;

namespace Atestados.UI.Controllers.Atestados
{
    public class FortalecimientoInvExtController : Controller
    {
        private AtestadosEntities db = new AtestadosEntities();
        private InformacionAtestado infoAtestado = new InformacionAtestado();
        private InformacionGeneral infoGeneral = new InformacionGeneral();

        private readonly string Rubro = "Actividades para el fortalecimiento de la investigación y la extensión";

        public static List<ArchivoDTO> archivosOld = null;

        // GET: FortalecimientoInvExt
        public ActionResult Index()
        {
            return View(infoAtestado.CargarAtestadosDeTipo(infoAtestado.ObtenerIDdeRubro(Rubro)));
        }

        // GET: FortalecimientoInvExt/Ver/:id
        //probar
        public ActionResult Ver(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AtestadoDTO atestado = infoAtestado.CargarAtestado(id);
            if (atestado == null)
            {
                return HttpNotFound();
            }
            return View(atestado);
        }

        // GET: FortalecimientoInvExt/Crear
        public ActionResult Crear()
        {
            AtestadoDTO actFortalecimiento = new AtestadoDTO();
            actFortalecimiento.NumeroAutores = 1;
            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", infoAtestado.ObtenerIDdePais("costa rica"));
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersonaPorTipo(infoAtestado.ObtenerIDdeRubro(Rubro), (int)Session["UsuarioID"]);

            // Limpiar las listas de archivos y autores por si tienen basura.
            Session["Autores"] = new List<AutorDTO>();
            Session["Archivos"] = new List<ArchivoDTO>();

            return View(actFortalecimiento);
        }

        // POST: FortalecimientoInvExt/Crear
        [HttpPost]
        public ActionResult Crear([Bind(Include = "Archivos,AtestadoID,AtestadoXPersona,Enlace,HoraCreacion,Nombre,NumeroAutores,Observaciones,PaisID,Persona,PersonaID,RubroID,Website,AutoresEq,AutoresCheck")] AtestadoDTO atestado)
        {
            if (ModelState.IsValid)
            {
                atestado.PersonaID = (int)Session["UsuarioID"]; // cambiar por sesion
                atestado.RubroID = infoAtestado.ObtenerIDdeRubro(Rubro);
                atestado.PaisID = infoAtestado.ObtenerIDdePais("costa rica"); // Costa Rica
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

        // GET: FortalecimientoInvExt/Editar/:id
        public ActionResult Editar(int? id)
        {
            return View();
        }

        // POST: FortalecimientoInvExt/Editar/:id
        [HttpPost]
        public ActionResult Editar(int id, FormCollection collection)
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

        // GET: FortalecimientoInvExt/Borrar/:id
        public ActionResult Borrar(int? id)
        {
            return View();
        }

        // POST: FortalecimientoInvExt/Borrar/:id
        [HttpPost]
        public ActionResult Borrar(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
