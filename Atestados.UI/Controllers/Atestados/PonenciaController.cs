using Atestados.Datos.Modelo;
using Atestados.Negocios.Negocios;
using Atestados.Objetos.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Atestados.UI.Controllers.Atestados
{
    public class PonenciaController : Controller
    {
        private AtestadosEntities db = new AtestadosEntities();
        private InformacionAtestado infoAtestado = new InformacionAtestado();
        private InformacionGeneral infoGeneral = new InformacionGeneral();

        private string Rubro = "Ponencia"; // esta como otras obras profesionales

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

        // GET: Ponencia
        public ActionResult Index()
        {
            return View(infoAtestado.CargarAtestadosDeTipo(infoAtestado.ObtenerIDdeRubro(Rubro)));
        }

        // GET: Ponencia/Details/5
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

        // GET: Ponencia/Create
        public ActionResult Crear()
        {
            PonenciaDTO ponencia = new PonenciaDTO();
            ponencia.FechaPonencia = DateTime.Now;

            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", infoAtestado.ObtenerIDdePais("costa rica"));
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersonaPorTipo(infoAtestado.ObtenerIDdeRubro(Rubro), (int)Session["UsuarioID"]);

            if (Session["Archivos"] == null)
            {
                Session["Archivos"] = new List<ArchivoDTO>();
            }

            return View(ponencia);
        }

        // POST: Ponencia/Creaar
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "FechaPonencia,AtestadoID,Nombre,NumeroAutores,Observaciones,HoraCreacion,Enviado,Descargado,CantidadHoras,Lugar,CatalogoTipo,Enlace,PaisID,PersonaID,RubroID")] PonenciaDTO atestado)
        {
            if (ModelState.IsValid)
            {
                atestado.PersonaID = (int)Session["UsuarioID"]; // cambiar por sesion
                atestado.RubroID = infoAtestado.ObtenerIDdeRubro(Rubro);
                atestado.PaisID = infoAtestado.ObtenerIDdePais("costa rica"); // Costa Rica
                Atestado a = AutoMapper.Mapper.Map<PonenciaDTO, Atestado>(atestado);
                infoAtestado.GuardarAtestado(a);
                atestado.AtestadoID = a.AtestadoID;
                Fecha fecha = AutoMapper.Mapper.Map<PonenciaDTO, Fecha>(atestado);
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
            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", atestado.PaisID);
            return View(atestado);
        }

        // GET: Ponencia/Edit/5
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Session["Archivos"] == null)
            {
                Session["Archivos"] = new List<ArchivoDTO>();
            }

            Atestado atestado = infoAtestado.CargarAtestadoParaEditar(id);
            AtestadoDTO a = AutoMapper.Mapper.Map<Atestado, AtestadoDTO>(atestado);
            if (atestado == null)
            {
                return HttpNotFound();
            }
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersonaPorTipo(infoAtestado.ObtenerIDdeRubro(Rubro), (int)Session["UsuarioID"]);
            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", atestado.PaisID);
            return View(a);
        }

        // POST: Ponencia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Lugar,CantidadHoras,Archivos,AtestadoID,AtestadoXPersona,Editorial,Enlace,HoraCreacion,Nombre,NumeroAutores,Observaciones,PaisID,Persona,PersonaID,RubroID,Website,Fecha,DominioIdioma,Persona,Rubro,Pais,InfoEditorial,Archivo")] AtestadoDTO atestado)
        {
            if (ModelState.IsValid)
            {
                atestado.PersonaID = (int)Session["UsuarioID"]; // cambiar por sesion
                atestado.RubroID = infoAtestado.ObtenerIDdeRubro(Rubro);
                atestado.PaisID = infoAtestado.ObtenerIDdePais("costa rica"); // Costa Rica
                atestado.Fecha.FechaID = atestado.AtestadoID;
                atestado.Fecha.FechaInicio = DateTime.Now;
                infoAtestado.EditarFecha(AutoMapper.Mapper.Map<FechaDTO, Fecha>(atestado.Fecha));
                atestado.HoraCreacion = DateTime.Now;
                atestado.Archivos = infoAtestado.CargarArchivosDeAtestado(atestado.AtestadoID);
                infoAtestado.EditarAtestado(AutoMapper.Mapper.Map<AtestadoDTO, Atestado>(atestado));

                List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];
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
            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", atestado.PaisID);
            return View(atestado);
        }

        // GET: Libro/Borrar
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

        // POST: Libro/Borrar
        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult Borrar(int id)
        {
            infoAtestado.BorrarAtestado(id);
            return RedirectToAction("Crear");
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Ponencia/Evaluar
        public ActionResult Evaluar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UsuarioDTO usuario = (UsuarioDTO)Session["Usuario"];
            Session["TipoUsuario"] = usuario.TipoUsuario;
            Session["idAtestado"] = id;
            Session["idUsuario"] = usuario.UsuarioID;

            //EvaluaciónXAtestado e = infoAtestado.ObtenerEvaluacionXAtestado((int)id, usuario.UsuarioID);

            ViewBag.Revisor = infoGeneral.CargarPersona(usuario.UsuarioID);
            ViewBag.Atestado = infoAtestado.CargarAtestado(id);
            
            AtestadoDTO atestado = infoAtestado.CargarAtestado(id);
            if (atestado == null)
            {
                return HttpNotFound();
            }
            EvaluacionXAtestadoDTO evaluacion = new EvaluacionXAtestadoDTO();
            return View(evaluacion);
        }

        // evaluar
        // POST: Ponencia/Evaluar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Evaluar([Bind(Include = "PorcentajeObtenido, Observaciones")] EvaluacionXAtestadoDTO evaluacion)
        {
            if (ModelState.IsValid)
            {
                if (evaluacion.Observaciones == null) evaluacion.Observaciones = "N/A";

                evaluacion.AtestadoID = (int)Session["idAtestado"];
                evaluacion.PersonaID = (int)Session["idUsuario"];

                EvaluaciónXAtestado e = new EvaluaciónXAtestado()
                {
                    AtestadoID = evaluacion.AtestadoID,
                    PersonaID = evaluacion.PersonaID,
                    PorcentajeObtenido = (float)evaluacion.PorcentajeObtenido,
                    Observaciones = evaluacion.Observaciones
                };

                db.EvaluaciónXAtestado.Add(e);

                db.SaveChanges();

                AtestadoDTO atestado = infoAtestado.CargarAtestado((int)Session["idAtestado"]);

                return RedirectToAction("Ver", new { id = (int)Session["idAtestado"] });
            }
            return View(evaluacion);
        }


    }
}