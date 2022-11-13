using Atestados.Datos.Modelo;
using Atestados.Negocios.Negocios;
using Atestados.Objetos.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IronXL;
using System.Text;
using AutoMapper;
using System.Web.Routing;

namespace Atestados.UI.Controllers
{
    public class ComisionController : Controller
    {
        private readonly InformacionAtestado infoAtestado = new InformacionAtestado();
        private readonly InformacionGeneral infoGeneral = new InformacionGeneral();
        private AtestadosEntities db = new AtestadosEntities();

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

        public ActionResult Index()
        {
            Session["TipoUsuarioNombre"] = "Revisor";

            List<EnviadoDTO> enviados = infoAtestado.PersonasEntregaron();
            return View(enviados);
        }

        public FileResult Descarga(int id)
        {
            EnviadoDTO enviado = infoAtestado.ObtenerEnviado(id);
            List<Archivo> archivos = new List<Archivo>();
            foreach (Atestado atestado in enviado.Atestados)
            {
                archivos.AddRange(atestado.Archivo.ToList());
            }
            byte[] file = GetZipArchive(archivos);
            return File(file, "application/zip", $"{enviado.Persona.Nombre}_{enviado.Persona.PrimerApellido}_{enviado.Persona.SegundoApellido}.zip");
        }

        public FileResult GenerarExcel(int id)
        {
            EnviadoDTO enviado = infoAtestado.ObtenerEnviado(id);
            var csv = new StringBuilder();
            var linea = $"Nombre;Rubro;Enlace;Observaciones";
            csv.AppendLine(linea);
            foreach (Atestado atestado in enviado.Atestados)
            {
                linea = $"{atestado.Nombre};{infoAtestado.ObtenerRubro(atestado.RubroID)};{atestado.Enlace};{atestado.Observaciones}";
                csv.AppendLine(linea);
                foreach (Archivo archivo in atestado.Archivo)
                {
                    linea = $";{archivo.Nombre}";
                    csv.AppendLine(linea);
                }
            }

            return File(Encoding.Unicode.GetBytes(csv.ToString()), "text/csv", $"{enviado.Persona.Nombre}_{enviado.Persona.PrimerApellido}_{enviado.Persona.SegundoApellido}.csv");
        }

        private static byte[] GetZipArchive(List<Archivo> files)
        {
            byte[] archiveFile;
            using (var archiveStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        var zipArchiveEntry = archive.CreateEntry(file.Nombre, CompressionLevel.Fastest);
                        using (var zipStream = zipArchiveEntry.Open())
                            zipStream.Write(file.Datos, 0, file.Datos.Length);
                    }
                }

                archiveFile = archiveStream.ToArray();
            }

            return archiveFile;
        }

        public void EvaluarAtestado(float nota, int idAtestado, int idRevisor, string Observaciones)
        {
            EvaluacionXAtestadoDTO evaluacion = new EvaluacionXAtestadoDTO();
            evaluacion.AtestadoID = idAtestado;
            evaluacion.PersonaID = idRevisor;
            evaluacion.Observaciones = Observaciones;

            EvaluaciónXAtestado e = Mapper.Map<EvaluacionXAtestadoDTO, EvaluaciónXAtestado>(evaluacion);

            db.EvaluaciónXAtestado.Add(e);
            db.SaveChanges();

        }

        // POST: Atestado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Evaluar([Bind(Include = "PorcentajeObtenido, Observaciones")] EvaluacionXAtestadoDTO evaluacion)
        {
            evaluacion.AtestadoID = (int)Session["idAtestado"];
            evaluacion.PersonaID = (int)Session["UsuarioID"];
            evaluacion.Observaciones = (string)Session["observaciones"];
            evaluacion.PorcentajeObtenido = (float)Session["nota"];

            EvaluaciónXAtestado e = Mapper.Map<EvaluacionXAtestadoDTO, EvaluaciónXAtestado>(evaluacion);

            db.EvaluaciónXAtestado.Add(e);
            db.SaveChanges();

            AtestadoDTO atestado = infoAtestado.CargarAtestado((int)Session["idAtestado"]);

            return View(atestado);
        }

        [HttpGet]
        public ActionResult VerPuntos(int id)
        {
            ViewBag.PuntosXRubroDTOs = infoAtestado.CargarPuntosPersona(id);
            var persona = infoGeneral.CargarPersona(id);
            var categoria = db.TipoCategoria.Where(c => c.TipoCategoriaID == persona.CategoriaActual).FirstOrDefault();
            ViewBag.TipoCategoria = categoria;
            return View(persona);
        }
    }
}