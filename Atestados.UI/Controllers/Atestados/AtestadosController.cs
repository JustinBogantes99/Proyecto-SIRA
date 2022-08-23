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
using Atestados.UI.Controllers.Atestados;
using Newtonsoft.Json;

namespace Atestados.UI.Controllers
{
    public class AtestadosController : Controller
    {
        private static int archCont = 0;
        private AtestadosEntities db = new AtestadosEntities();
        private InformacionAtestado infoAtestado = new InformacionAtestado();
        private InformacionGeneral infoGeneral = new InformacionGeneral();
        public static List<ArchivoDTO> archivosOld = null;

        // GET: Atestados
        public ActionResult Index()
        {
            return View(infoAtestado.CargarAtestados());
        }

        // GET: Atestados/Ver
        public ActionResult Ver(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            AtestadoDTO atestado = infoAtestado.CargarAtestado(id);
            if (atestado == null)
                return HttpNotFound();
            return View(atestado);
        }

        // GET: Atestados/Crear
        public ActionResult Crear()
        {
            AtestadoDTO atestado = new AtestadoDTO();
            atestado.FechaFinal = DateTime.Now;
            atestado.FechaInicio = DateTime.Now;
            atestado.NumeroAutores = 1;
            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", infoAtestado.ObtenerIDdePais("costa rica"));
            ViewBag.RubroID = new SelectList(db.Rubro, "RubroID", "Nombre");
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersona((int)Session["UsuarioID"]);

            // Limpiar las listas de archivos y autores por si tienen basura.
            Session["Archivos"] = new List<ArchivoDTO>();

            return View(atestado);
        }

        // POST: Atestados/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "AtestadoID,Nombre,Archivos,NumeroAutores,Observaciones,HoraCreacion,Enviado,Descargado,CantidadHoras,Lugar,CatalogoTipo,Enlace,PaisID,PersonaID,RubroID")] AtestadoDTO atestado)
        {
            if (ModelState.IsValid)
            {
                List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];

                // Obtener el id del usuario que está agregando el atestado.
                atestado.PersonaID = (int)Session["UsuarioID"];
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

                Session["Archivos"] = new List<ArchivoDTO>();

                return RedirectToAction("Crear");
            }

            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", atestado.PaisID);
            ViewBag.RubroID = new SelectList(db.Rubro, "RubroID", "Nombre", atestado.RubroID);
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersona((int)Session["UsuarioID"]);
            return View(atestado);
        }

        // GET: Atestados/Editar
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

        // Indexar las listas de autores y archivos con números.
        private void indexarListas()
        {
            int cont = 1;
            List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];

            foreach (ArchivoDTO archivo in archivos)
                archivo.numArchivo = cont++;

            Session["Archivos"] = archivos;
        }

        // POST: Atestados/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Annio,Archivos,AtestadoID,AtestadoXPersona,Editorial,Enlace,HoraCreacion,Nombre,NumeroAutores,Observaciones,PaisID,Persona,PersonaID,RubroID,Website,Fecha,DominioIdioma,Persona,Rubro,AtestadoXPersona,Pais,InfoEditorial,Archivo")] AtestadoDTO atestado)
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
                AtestadoShared.obj.editarArchivos(archivosOld ,archivos, infoAtestado, atestado_mapped);

                Session["Archivos"] = new List<ArchivoDTO>();
                archivosOld = new List<ArchivoDTO>();

                return RedirectToAction("Crear");
            }

            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", infoAtestado.ObtenerIDdePais("costa rica"));
            ViewBag.RubroID = new SelectList(db.Rubro, "RubroID", "Nombre");
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersona((int)Session["UsuarioID"]);
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

        // GET: Atestados/Evaluar
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
            Session["autoresAtestado"] = infoAtestado.ObtenerAutores((int)id);

            List<EvaluaciónXAtestado> e = infoAtestado.ObtenerEvaluacionXAtestadoRevisor((int)id, usuario.UsuarioID);

            ViewBag.Revisor = infoGeneral.CargarPersona(usuario.UsuarioID);
            ViewBag.Atestado = infoAtestado.CargarAtestado(id);

            if (e != null)
            {
                ViewBag.Evaluaciones = e;
                //EvaluacionXAtestadoDTO edto = AutoMapper.Mapper.Map<EvaluaciónXAtestado, EvaluacionXAtestadoDTO>(e);
                //ViewBag.Evaluacion = edto;
                //return View(evaluacion);
                //foreach (EvaluaciónXAtestado evaluacion in e)
                //{
                //    ViewBag.Evaluaciones.a
                //}
            }

            AtestadoDTO atestado = infoAtestado.CargarAtestado(id);
            if (atestado == null)
            {
                return HttpNotFound();
            }
            EvaluacionXAtestadoDTO evaluacion = new EvaluacionXAtestadoDTO();
            evaluacion.Atestado = atestado;
            return View(evaluacion);
        }

        // evaluar
        // POST: Atestados/Evaluar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Evaluar([Bind(Include = "Observaciones")] EvaluacionXAtestadoDTO evaluacion)
        {
            if (ModelState.IsValid)
            {
                if (evaluacion.Observaciones == null) evaluacion.Observaciones = "N/A";

                evaluacion.AtestadoID = (int)Session["idAtestado"];
                evaluacion.PersonaID = (int)Session["idUsuario"];

                List<PersonaDTO> autores = (List<PersonaDTO>)Session["autoresAtestado"];
                List<EvaluacionXAtestadoDTO> puntos = (List<EvaluacionXAtestadoDTO>)Session["puntosAutores"];

                foreach (EvaluacionXAtestadoDTO eval in puntos)
                {
                    /*
                    float porcentaje = 0;
                    //int idAutor;
                    foreach (EvaluacionXAtestadoDTO eval in puntos)
                    {
                        if (eval.AutorID == autor.PersonaID)
                        {
                            porcentaje = eval.PorcentajeObtenido;
                        }
                    }
                    */

                    EvaluaciónXAtestado e = new EvaluaciónXAtestado()
                    {
                        AtestadoID = evaluacion.AtestadoID,
                        PersonaID = evaluacion.PersonaID,
                        AutorID = eval.AutorID,
                        PorcentajeObtenido = eval.PorcentajeObtenido,
                        Observaciones = evaluacion.Observaciones
                    };

                    List<EvaluaciónXAtestado> evaluacionesActuales = infoAtestado.ObtenerEvaluacionXAtestado(e.AtestadoID, e.PersonaID, e.AutorID);
                    EvaluaciónXAtestado evaluacionActual = null;
                    if (evaluacionesActuales.Count > 0)
                    {
                        evaluacionActual = evaluacionesActuales.First();
                    }
                    if (evaluacionActual != null)
                    {
                        infoAtestado.ModificarEvaluacion(e.AtestadoID, e.PersonaID, e.AutorID, e.PorcentajeObtenido);
                    }
                    else
                    {
                        db.EvaluaciónXAtestado.Add(e);
                        db.SaveChanges();
                    }


                }


                AtestadoDTO atestado = infoAtestado.CargarAtestado((int)Session["idAtestado"]);

                // Create dictionary of values
                Dictionary<string, string> values = new Dictionary<string, string>();
                values.Add("Libro", "Libro");
                values.Add("Artículo", "Articulo");
                values.Add("Obra didáctica", "ObraDidactica");
                values.Add("Obra administrativa de desarrollo", "ObraAdministrativa");

                String controlador = values[atestado.Rubro.Nombre];

                return RedirectToAction("Ver", controlador, new { id = (int)Session["idAtestado"] });
            }
            return View(evaluacion);
        }

        //POST: Atestados/ObtenerEvaluaciones
        public ActionResult ObtenerEvaluaciones()
        {


            var idAtestado = (int)Session["idAtestado"];
            var idRevisor = (int)Session["idUsuario"];

            List<EvaluaciónXAtestado> e = infoAtestado.ObtenerEvaluacionXAtestadoRevisor(idAtestado, idRevisor);

            List<EvaluacionXAtestadoDTO> evaluaciones = new List<EvaluacionXAtestadoDTO>();

            foreach(EvaluaciónXAtestado evaluacion in e)
            {
                evaluaciones.Add(AutoMapper.Mapper.Map<EvaluaciónXAtestado, EvaluacionXAtestadoDTO>(evaluacion));
            }

            if (evaluaciones == null)
            {
                return Json(new
                {
                    evaluaciones = false
                });
            }

            var json = JsonConvert.SerializeObject(evaluaciones);

            return Json(new
            {
                evaluaciones = evaluaciones
            });


            
        }

        // POST: Atestados/ObtenerAutores
        [HttpPost]
        public JsonResult ObtenerAutores()
        {
            var autores = Session["autoresAtestado"];
            //var jsonTest = JsonConvert.SerializeObject(autores);
            return Json(autores);
        }

        // POST: Atestados/AsignarPuntos
        [HttpPost]
        public void AsignarPuntos(List<EvaluacionXAtestadoDTO> evaluacionData)
        {
            List<EvaluacionXAtestadoDTO> puntos = new List<EvaluacionXAtestadoDTO>();

            foreach (EvaluacionXAtestadoDTO evaluacion in evaluacionData)
            {

                EvaluacionXAtestadoDTO e = new EvaluacionXAtestadoDTO()
                {
                    AutorID = (int)evaluacion.AutorID,
                    PorcentajeObtenido = (int)evaluacion.PorcentajeObtenido
                };

                puntos.Add(e);

            }

            Session["puntosAutores"] = puntos;

            //Object[] autores = (Object[])Session["autoresAtestado"];

            //Object[] newautores; 
            /*
            foreach (Object autor in autores)
            {
                foreach (EvaluacionXAtestadoDTO e in json)
                {
                    if (e.PersonaID == autor.)
                }
                autor.PorcentajeObtenido
            }
            */
            //List<EvaluacionXAtestadoDTO> puntos = new List<EvaluacionXAtestadoDTO>();

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Métodos Compartidos
        // Métodos utilizados por AtestadoShared.js
        [HttpPost]
        public JsonResult getAutores()
        {

            var autores = Session["Autores"];

            return Json(autores);
        }

        [HttpPost]
        public JsonResult UsuarioPorEmail(UsuarioDTO usuarioData)
        {

            var email = usuarioData.Email;

            UsuarioDTO usuario = infoGeneral.UsuarioPorEmail(email);

            if (usuario == null)
            {
                return Json(new
                {
                    usuario = false
                });
            }

            var json = JsonConvert.SerializeObject(usuario);

            return Json(new
            {
                usuario = json
            });

        }

        [HttpPost]
        // Calcular el porcentaje restante para los autores con porcentaje manual.
        public JsonResult calcularPorcentajes()
        {
            List<AutorDTO> autores = (List<AutorDTO>)Session["Autores"];
            int size = autores.Count;
            int counter = 0;

            // Retornar 100 si no hay autores.
            if (size == 0)
                return Json(new
                { p = 100 });

            // Iterar para sumar el porcentaje total de todos los autores.
            foreach (AutorDTO autor in autores)
                counter += (int)autor.Porcentaje;

            counter = 100 - counter;

            return Json(new
            { p = counter });
        }

        // Calcular los porcentajes equitativos.
        public void calcularPorcentajesEq(List<AutorDTO> autores)
        {
            int size = autores.Count;
            int perc = 100 / size;

            foreach (AutorDTO autor in autores)
                autor.Porcentaje = perc;

            Session["Autores"] = autores;
        }

        [HttpPost]
        public JsonResult checkFuncionario(UsuarioDTO funcionarioMail)
        {

            var email = funcionarioMail.Email;

            UsuarioDTO usuario = infoGeneral.UsuarioPorEmail(email);

            if (usuario == null)
                return Json(new
                { usuario = false });
            else
                return Json(new
                { usuario = true });
        }

        [HttpPost]
        // Agregar un Autor externo al Tec.
        public ActionResult agregarAutor(AutorDTO autorData)
        {
            AutorDTO autor = new AutorDTO()
            {
                numAutor = autorData.numAutor,
                Nombre = autorData.Nombre,
                PrimerApellido = autorData.PrimerApellido,
                SegundoApellido = autorData.SegundoApellido,
                Porcentaje = autorData.Porcentaje,
                Email = autorData.Email,
                PersonaID = autorData.PersonaID,
                esFuncionario = autorData.esFuncionario,
                porcEquitativo = autorData.porcEquitativo
            };

            List<AutorDTO> autores = (List<AutorDTO>)Session["Autores"];
            autores.Add(autor);
            if (autor.porcEquitativo)
                calcularPorcentajesEq(autores);
            else
                Session["Autores"] = autores;
            return PartialView("_AutoresTabla");
        }

        [HttpPost]
        // Agregar un autor funcionario.
        public ActionResult agregarFuncionario(AutorDTO autorData)
        {
            var email = autorData.Email;

            UsuarioDTO usuario = infoGeneral.UsuarioPorEmail(email);

            AutorDTO autor = new AutorDTO()
            {
                Nombre = usuario.Nombre,
                PrimerApellido = usuario.PrimerApellido,
                SegundoApellido = usuario.SegundoApellido,
                PersonaID = usuario.UsuarioID,
                numAutor = autorData.numAutor,
                Porcentaje = autorData.Porcentaje,
                Email = autorData.Email,
                esFuncionario = autorData.esFuncionario,
                porcEquitativo = autorData.porcEquitativo
            };

            List<AutorDTO> autores = (List<AutorDTO>)Session["Autores"];
            autores.Add(autor);
            if (autor.porcEquitativo)
                calcularPorcentajesEq(autores);
            else
                Session["Autores"] = autores;
            return PartialView("_AutoresTabla");
        }

        [HttpPost]
        public ActionResult borrarAutor(AutorDTO autorData)
        {
            var id = autorData.numAutor;

            List<AutorDTO> autores = (List<AutorDTO>)Session["Autores"];

            autores.RemoveAll(a => a.numAutor == id);

            Session["Autores"] = autores;

            return PartialView("_AutoresTabla");
        }

        [HttpGet]
        public void enviarArchCont(int num)
        {
            archCont = num;
        }

        [HttpPost]
        public ActionResult subirArchivo(HttpPostedFileBase archivo)
        {
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(archivo.InputStream))
            {
                bytes = br.ReadBytes(archivo.ContentLength);
            }

            ArchivoDTO ar = new ArchivoDTO
            {
                numArchivo = archCont,
                Nombre = Path.GetFileName(archivo.FileName),
                TipoArchivo = archivo.ContentType,
                Datos = bytes
            };

            List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];
            archivos.Add(ar);
            Session["Archivos"] = archivos;

            return PartialView("_ArchivosTabla");
        }

        [HttpGet]
        public FileResult descargarArchivo(int numArch)
        {
            List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];
            ArchivoDTO archivo = null;
            foreach (ArchivoDTO arch in archivos)
            {
                if (arch.numArchivo == numArch)
                {
                    archivo = arch;
                    break;
                }

            }
            //ArchivoDTO archivo = infoAtestado.CargarArchivo(numArch);
            return File(archivo.Datos, archivo.TipoArchivo, archivo.Nombre);
        }

        [HttpPost]
        public ActionResult borrarArchivo(ArchivoDTO arch)
        {
            var numArch= arch.numArchivo;

            List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];

            archivos.RemoveAll(a => a.numArchivo == numArch);

            Session["Archivos"] = archivos;

            return PartialView("_ArchivosTabla");
        }


        #endregion

    }
}
