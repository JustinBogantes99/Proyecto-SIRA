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
using Atestados.Objetos;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Atestados.UI.Controllers.Atestados
{
    public class RubricaController : Controller
    {
        private AtestadosEntities db = new AtestadosEntities();
        private InformacionAtestado infoAtestado = new InformacionAtestado();
        private InformacionGeneral infoGeneral = new InformacionGeneral();
        private Catalogos catalogos = new Catalogos();
        private InformacionRubrica infoRubrica = new InformacionRubrica();
        private readonly string Rubro = "Artículo";
        public static List<ArchivoDTO> archivosOld = null;

        // GET: Articulos
        public ActionResult Index()
        {
            return View(infoAtestado.CargarAtestadosDeTipo(infoAtestado.ObtenerIDdeRubro(Rubro)));
        }

        // GET: Articulo/Ver
        public ActionResult Ver(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            AtestadoDTO atestado = infoAtestado.CargarAtestado(id);
            if (atestado == null)
                return HttpNotFound();
            ViewBag.Autores = infoAtestado.CargarAutoresAtestado(id);
            ViewBag.NotasPonderadas = infoAtestado.CargarNotasPonderadasAutores(id);
            ViewBag.Puntos = infoAtestado.CargarPuntosAutores(id);
            return View(atestado);
        }

        // GET: Articulo/Crear
        public ActionResult Crear()
        {
            RubricaDTO rubrica = new RubricaDTO();
            RequisitoDTO requisito = new RequisitoDTO();

            AtestadoDTO atestado = new AtestadoDTO();
            atestado.FechaFinal = DateTime.Now;
            atestado.FechaInicio = DateTime.Now;
            atestado.NumeroAutores = 1;
            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", infoAtestado.ObtenerIDdePais("costa rica"));
            ViewBag.RubroID = new SelectList(db.Rubro, "RubroID", "Nombre");
            ViewBag.TipoPuntajeID = new SelectList(db.TipoPuntaje, "TipoPuntajeID", "Nombre");
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersona((int)Session["UsuarioID"]);

            // Limpiar las listas de archivos y autores por si tienen basura.
            Session["Archivos"] = new List<ArchivoDTO>();

            //CREAR UNA LISTA DE COLUMNAS Y LIMPIARLA SI TIENE BASURA ///CAMBIAR RubricaDTO por el real IMPORTANTE////////////////////////////////////////////////////////////////////////////
            Session["Criterios"] = new List<RequisitoDTO>();

            return View(rubrica);
        }

        // POST: Rubrica/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(RubricaDTO rubrica)
        {
            if (ModelState.IsValid)
            {
                // Falta validar entre los 3 tipos de puntaje, de momento lo interpreta como si fuera ValorFijo siempre
                List<RequisitoDTO> requisitos = (List<RequisitoDTO>)Session["Criterios"];
                Rubrica rubrica_mapped = AutoMapper.Mapper.Map<RubricaDTO, Rubrica>(rubrica);
                infoRubrica.GuardarRubrica(rubrica_mapped);

                int RubricaID = rubrica_mapped.RubricaID;

                foreach(RequisitoDTO requisito in requisitos)
                {
                    Requisito requisito_mapped = AutoMapper.Mapper.Map<RequisitoDTO, Requisito>(requisito);
                    requisito_mapped.RubricaID = RubricaID;
                    infoRubrica.GuardarRequisito(requisito_mapped);
                }

                Session["Criterios"] = new List<ArchivoDTO>();

                return RedirectToAction("Crear");
            }
            return View(rubrica);
        }

        // GET: Articulo/Editar
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
            ViewBag.AtestadoID = new SelectList(db.Fecha, "FechaID", "FechaID", atestado.AtestadoID);
            ViewBag.AtestadoID = new SelectList(db.InfoEditorial, "InfoEditorialID", "Editorial", atestado.AtestadoID);
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

        // POST: Articulo/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Lugar,CantidadHoras,Archivos,AtestadoID,AtestadoXPersona,Editorial,Enlace,HoraCreacion,Nombre,NumeroAutores,Observaciones,PaisID,Persona,PersonaID,RubroID,Website,Fecha,DominioIdioma,Persona,Rubro,Pais,InfoEditorial,Archivo,AutoresEq,AutoresCheck")] AtestadoDTO atestado)
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
                atestado.Fecha.FechaInicio = DateTime.Now;
                infoAtestado.EditarFecha(AutoMapper.Mapper.Map<FechaDTO, Fecha>(atestado.Fecha));
                atestado.HoraCreacion = DateTime.Now;
                atestado.InfoEditorial.InfoEditorialID = atestado.AtestadoID;
                infoAtestado.EditarInfoEditorial(AutoMapper.Mapper.Map<InfoEditorialDTO, InfoEditorial>(atestado.InfoEditorial));
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
            ViewBag.Autores = infoAtestado.CargarAutoresAtestado(id);
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
        public ActionResult NuevoRequisito(RequisitoDTO requisitoData)
        {
            List<RequisitoDTO> requisitos = (List<RequisitoDTO>)Session["Criterios"];
            requisitos.Add(requisitoData);
            Session["Criterios"] = requisitos;
            return PartialView("_CriteriosRubrica");
        }

        [HttpPost]
        public ActionResult ObtenerTipoPuntaje(TipoPuntajeDTO tipo)
        {
            if (tipo.TipoPuntajeID == 1)
                return PartialView("_ValorFijo");
            else if (tipo.TipoPuntajeID == 2)
                return PartialView("_Seleccion");
            else
                return PartialView("_Producto");
        }

        [HttpPost]
        public ActionResult agregarCriterio()
        {
            return PartialView("_CriteriosRubrica");
        }
    }
}