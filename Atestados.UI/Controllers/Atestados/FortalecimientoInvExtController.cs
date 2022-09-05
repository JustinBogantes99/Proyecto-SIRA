using Atestados.Datos.Modelo;
using Atestados.Negocios.Negocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atestados.Objetos.Dtos;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace Atestados.UI.Controllers.Atestados
{
    public class FortalecimientoInvExtController : Controller
    {
        private AtestadosEntities db = new AtestadosEntities();
        private InformacionAtestado infoAtestado = new InformacionAtestado();
        private InformacionGeneral infoGeneral = new InformacionGeneral();

        private readonly string Rubro = "Actividades para el fortalecimiento de la investigación y la extensión";

        //public static List<ArchivoDTO> archivosOld = null;

        // GET: FortalecimientoInvExt
        public ActionResult Index()
        {
            return View(infoAtestado.CargarAtestadosDeTipo(infoAtestado.ObtenerIDdeRubro(Rubro)));
        }

        // GET: FortalecimientoInvExt/Ver/:id
        //probar
        public ActionResult Ver(int? id)
        {
            return View();
        }

        // GET: FortalecimientoInvExt/Crear
        public ActionResult Crear()
        {
            AtestadoDTO actFortalecimiento = new AtestadoDTO();
            actFortalecimiento.FechaFinal = DateTime.Now;
            actFortalecimiento.FechaInicio = DateTime.Now;
            actFortalecimiento.NumeroAutores = 1;

            // Limpiar las listas de archivos
            if (Session["Archivos"] == null)
            {
                Session["Archivos"] = new List<ArchivoDTO>();
            }

            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersonaPorTipo(infoAtestado.ObtenerIDdeRubro(Rubro), (int)Session["UsuarioID"]);

            return View(actFortalecimiento);
        }

        // POST: FortalecimientoInvExt/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "Fecha,Archivos,AtestadoID,Enlace,HoraCreacion,Nombre,CantidadHoras,Observaciones,Persona,PersonaID,RubroID,FechaInicio,FechaFinal,Lugar,NumeroAutores")] AtestadoDTO atestado)
        {
            if (ModelState.IsValid)
            {
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

                List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];
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

        [HttpPost]
        public JsonResult Cargar(HttpPostedFileBase archivo)
        {
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(archivo.InputStream))
            {
                bytes = br.ReadBytes(archivo.ContentLength);
            }

            if (Session["Archivos"] == null)
            {
                Session["Archivos"] = new List<ArchivoDTO>();
            }

            ArchivoDTO ar = new ArchivoDTO
            {
                Nombre = Path.GetFileName(archivo.FileName),
                TipoArchivo = archivo.ContentType,
                Datos = bytes
            };
            List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];
            archivos.Add(ar);
            Session["Archivos"] = archivos;

            var jsonTest = JsonConvert.SerializeObject(ar);

            return Json(new
            {
                archivoJson = jsonTest
            });
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
