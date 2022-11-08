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

        // GET: Articulo/Crear
        public ActionResult Crear()
        {
            RubricaDTO rubrica = new RubricaDTO();

            ViewBag.RubroID = new SelectList(db.Rubro, "RubroID", "Nombre");
            ViewBag.TipoPuntajeID = new SelectList(db.TipoPuntaje, "TipoPuntajeID", "Nombre");
            ViewBag.PuntajeTiempoID = new SelectList(db.PuntajeTiempo, "PuntajeTiempoID", "Nombre");
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersona((int)Session["UsuarioID"]);

            // Limpiar las listas de archivos y autores por si tienen basura.
            Session["Archivos"] = new List<ArchivoDTO>();

            //CREAR UNA LISTA DE COLUMNAS Y LIMPIARLA SI TIENE BASURA ///CAMBIAR RubricaDTO por el real IMPORTANTE////////////////////////////////////////////////////////////////////////////
            Session["Criterios"] = new List<RequisitoDTO>();

            Session["SeleccionPuntajes"] = new List<SeleccionPuntajeDTO>();

            return View(rubrica);
        }

        // POST: Rubrica/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(RubricaDTO rubrica)
        {
            if (ModelState.IsValid)
            {
                rubrica.Fecha = DateTime.Now;
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

                if (rubrica.TipoPuntajeID == 2)
                {
                    List<SeleccionPuntajeDTO> seleccionPuntajes = (List<SeleccionPuntajeDTO>)Session["SeleccionPuntajes"];

                    foreach(SeleccionPuntajeDTO seleccionPuntaje in seleccionPuntajes)
                    {
                        SeleccionPuntaje seleccionPuntaje_mapped = AutoMapper.Mapper.Map<SeleccionPuntajeDTO, SeleccionPuntaje>(seleccionPuntaje);
                        seleccionPuntaje_mapped.RubricaID = RubricaID;
                        infoRubrica.GuardarSeleccionPuntaje(seleccionPuntaje_mapped);
                    }
                }

                Session["Criterios"] = new List<RequisitoDTO>();
                Session["SeleccionPuntajes"] = new List<SeleccionPuntajeDTO>();

                return RedirectToAction("Crear");
            }
            return View(rubrica);
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
        public ActionResult NuevoSeleccionPuntaje(SeleccionPuntajeDTO seleccionPuntajeData)
        {
            List<SeleccionPuntajeDTO> seleccionPuntajes = (List<SeleccionPuntajeDTO>)Session["SeleccionPuntajes"];
            seleccionPuntajes.Add(seleccionPuntajeData);
            Session["SeleccionPuntajes"] = seleccionPuntajes;
            return PartialView("_SeleccionTabla");
        }
    }
}