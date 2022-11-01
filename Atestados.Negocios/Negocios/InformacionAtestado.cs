using Atestados.Datos.Modelo;
using Atestados.Objetos.Dtos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Atestados.Utilitarios.Puntos;

namespace Atestados.Negocios.Negocios
{
    public class InformacionAtestado
    {
        private readonly AtestadosEntities db = new AtestadosEntities();
        private readonly InformacionGeneral info = new InformacionGeneral();

        #region Atestado

        public int ObtenerIDdeRubro(string rubro) {
            List<Rubro> rubros = db.Rubro.Where(l => l.Nombre.Contains(rubro.Trim().ToLower())).ToList();
            return rubros.Count > 0 ? rubros.FirstOrDefault().RubroID : -1;
        }

        public string ObtenerRubro(int id)
        {
            return db.Rubro.Where(r => r.RubroID == id).FirstOrDefault().Nombre;
        }

        /*
        public int ObtenerIDdeTipoPuntaje()
        {
            List<TipoPuntaje> listaRubros =  db.TipoPuntaje.Include(a => a.Nombre).ToList();
            List<TipoPuntaje> listaRubrosDTO = AutoMapper.Mapper.Map<List<TipoPuntaje>, List<TipoPuntaje>>(listaRubros);
            return listaRubrosDTO;
            
        }*/

        public List<AtestadoDTO> CargarAtestadosDePersonaEnviados(int personaID, int v)
        {
            List<Atestado> listaAtestado = db.Atestado.Where(a => a.PersonaID == personaID && a.Enviado == v).ToList();

            List<AtestadoDTO> listaAtestadosDto = AutoMapper.Mapper.Map<List<Atestado>, List<AtestadoDTO>>(listaAtestado);

            return listaAtestadosDto;
        }

        public int ObtenerIDdePais(string pais)
        {
            List<Pais> rubros = db.Pais.Where(l => l.Nombre.Contains(pais.Trim().ToLower())).ToList();
            return rubros.Count > 0 ? rubros.FirstOrDefault().PaisID : -1;
        }

        public AtestadoDTO CargarAtestado(int? id)
        {
            Atestado atestado = db.Atestado.Find(id);

            if (atestado == null)
                return null;

            AtestadoDTO atestadosDto = AutoMapper.Mapper.Map<Atestado, AtestadoDTO>(atestado);

            return atestadosDto;

        }

        public Atestado CargarAtestadoParaEditar(int? id)
        {
            Atestado atestado = db.Atestado.Find(id);

            if (atestado == null)
                return null;

            return atestado;

        }

        public Atestado CargarAtestadoParaBorrar(int? id)
        {
            Atestado atestado = db.Atestado.Find(id);

            if (atestado == null)
                return null;

            return atestado;

        }

        public List<AtestadoDTO> CargarAtestados()
        {

            List<Atestado> listaAtestado = db.Atestado.Include(a => a.Pais).Include(a => a.Persona).Include(a => a.Rubro).Include(a => a.DominioIdioma).Include(a => a.Fecha).Include(a => a.InfoEditorial).ToList();

            List<AtestadoDTO> listaAtestadosDto = AutoMapper.Mapper.Map<List<Atestado>, List<AtestadoDTO>>(listaAtestado);

            return listaAtestadosDto;

        }

        public List<AtestadoDTO> CargarAtestadosDeTipo(int? rubroId)
        {

            List<Atestado> listaAtestado = db.Atestado.Where(l => l.RubroID == rubroId).ToList();

            List<AtestadoDTO> listaAtestadosDto = AutoMapper.Mapper.Map<List<Atestado>, List<AtestadoDTO>>(listaAtestado);

            return listaAtestadosDto;

        }

        public List<AtestadoDTO> CargarAtestadosDePersona(int? personaID)
        {

            List<Atestado> listaAtestado = db.Atestado.Where(a => a.PersonaID == personaID).ToList();

            List<AtestadoDTO> listaAtestadosDto = AutoMapper.Mapper.Map<List<Atestado>, List<AtestadoDTO>>(listaAtestado);

            return listaAtestadosDto;

        }

        public List<AtestadoDTO> CargarAtestadosDePersonaPorTipo(int? rubroId, int? personaID)
        {

            List<Atestado> listaAtestado = db.Atestado.Where(a => a.RubroID == rubroId && a.PersonaID == personaID).ToList();

            List<AtestadoDTO> listaAtestadosDto = AutoMapper.Mapper.Map<List<Atestado>, List<AtestadoDTO>>(listaAtestado);

            return listaAtestadosDto;

        }


        public void GuardarAtestado(Atestado atestado)
        {
            db.Atestado.Add(atestado);
            db.SaveChanges();
        }

        public void EditarAtestado(Atestado atestado)
        {
            //db.Entry(atestado).State = EntityState.Modified;
            db.Set<Atestado>().AddOrUpdate(atestado);
            db.SaveChanges();
        }

        public void BorrarAtestado(int id)
        {
            Atestado atestado = db.Atestado.Find(id);
            db.Atestado.Remove(atestado);
            db.SaveChanges();
        }

        public List<Atestado> ObtenerAtestadosEnviados()
        {
            List<Atestado> query = db.Atestado.Where(a => a.Enviado == 1).ToList();
            return query;
        }

        public List<EvaluaciónXAtestado> ObtenerEvaluacionesAtestado(int id)
        {
            List<EvaluaciónXAtestado> query = db.EvaluaciónXAtestado.Where(a => a.AtestadoID == id).ToList();
            return query;
        }

        public List<EvaluaciónXAtestado> ObtenerEvaluacionesAtestadoPorAutor(int id, int idAutor)
        {
            List<EvaluaciónXAtestado> query = db.EvaluaciónXAtestado.Where(a => a.AtestadoID == id && a.AutorID == idAutor).ToList();
            return query;
        }

        public List<EvaluaciónXAtestado> ObtenerEvaluacionXAtestadoRevisor(int idAtestado, int idPersona)
        {
            List<EvaluaciónXAtestado> query = db.EvaluaciónXAtestado.Where(a => a.AtestadoID == idAtestado && a.PersonaID == idPersona).ToList();
            return query;
        }

        public List<EvaluaciónXAtestado> ObtenerEvaluacionXAtestado(int idAtestado, int idPersona, int idAutor)
        {
            List<EvaluaciónXAtestado> query = db.EvaluaciónXAtestado.Where(a => a.AtestadoID == idAtestado && a.PersonaID == idPersona && a.AutorID == idAutor).ToList();
            return query;
        }

        public void BorrarEvaluacion(int idAtestado, int idPersona, int idAutor)
        {
            EvaluaciónXAtestado e = db.EvaluaciónXAtestado.Find(idAtestado, idPersona, idAutor);
            db.EvaluaciónXAtestado.Remove(e);
            db.SaveChanges();
        }

        public void ModificarEvaluacion(int idAtestado, int idPersona, int idAutor, double porcentajeObtenido)
        {
            EvaluaciónXAtestado e = db.EvaluaciónXAtestado.Find(idAtestado, idPersona, idAutor);
            e.PorcentajeObtenido = porcentajeObtenido;
            db.SaveChanges();
        }

        public List<PersonaDTO> ObtenerAutores(int idAtestado)
        {
            List<AtestadoXPersona> query = db.AtestadoXPersona.Where(a => a.AtestadoID == idAtestado).ToList();
            List<PersonaDTO> personas = new List<PersonaDTO>();
            foreach (var e in query)
            {
                var p = db.Persona.Find(e.PersonaID);
                personas.Add(AutoMapper.Mapper.Map<Persona, PersonaDTO>(p));
            }
            return personas;
        }
        public List<EnviadoDTO> PersonasEntregaron()
        {
            List<Atestado> enviados = ObtenerAtestadosEnviados();
            Dictionary<int, List<Atestado>> temp = new Dictionary<int, List<Atestado>>();

            foreach (Atestado atestado in enviados)
            {
                if(!temp.ContainsKey(atestado.PersonaID))
                {
                    temp.Add(atestado.PersonaID, new List<Atestado>());
                }

                temp[atestado.PersonaID].Add(atestado);
            }

            return temp.Select(kvp => new EnviadoDTO() { 
                PersonaID = kvp.Key,
                Atestados = kvp.Value,
                Persona = info.CargarPersona(kvp.Key)
            }).ToList();
        }

        public EnviadoDTO ObtenerEnviado(int personaID)
        {
            List<EnviadoDTO> enviados = PersonasEntregaron();
            return enviados.FirstOrDefault(x => x.PersonaID == personaID);
        }

        public float ObtenerNotaAtestado(AtestadoDTO atestado)
        {
            var id = atestado.AtestadoID;
            List<EvaluaciónXAtestado> evaluaciones = ObtenerEvaluacionesAtestado(id);
            return CalcularPonderado(evaluaciones);
        }

        public float ObtenerNotaAtestadoPorAutor(AtestadoDTO atestado, int idAutor)
        {
            var id = atestado.AtestadoID;
            List<EvaluaciónXAtestado> evaluaciones = ObtenerEvaluacionesAtestadoPorAutor(id, idAutor);
            return CalcularPonderado(evaluaciones);
        }

        private float CalcularPonderado(List<EvaluaciónXAtestado> evaluaciones){
            if (evaluaciones.Count == 0)
            {
                return 0;
            }

            var porcentaje = .0;

            foreach (EvaluaciónXAtestado evaluacion in evaluaciones)
            {
                porcentaje += evaluacion.PorcentajeObtenido;
            }

            var r = porcentaje / evaluaciones.Count;

            return (float)r;
        }

        public List<PuntosXRubroDTO> CargarPuntosPersona(int id)
        {
            // Obtener la categoría de la persona
            var persona = db.Persona.Find(id);
            var categoria = persona.TipoCategoria.Nombre;

            List<PuntosXRubroDTO> puntosXRubroDTOs = new List<PuntosXRubroDTO>();

            // Libro
            PuntosXRubroDTO puntosLibroInfo = new PuntosXRubroDTO();
            puntosLibroInfo.Rubro = "Libro";
            double puntosLibro = CalcularPuntosPorPersonaYRubro(id, "Libro");
            switch (categoria)
            {
                case "Primera":
                    puntosLibroInfo.PuntosMaximosPasoActual = Puntos.Libro.MAXIMO_PROFESIONAL_2;
                    break;
                case "Segunda":
                    puntosLibroInfo.PuntosMaximosPasoActual = Puntos.Libro.MAXIMO_PROFESIONAL_3;
                    break;
                case "Tercera":
                    puntosLibroInfo.PuntosMaximosPasoActual = Puntos.Libro.MAXIMO_PROFESIONAL_4;
                    break;
                default:
                    break;
            }
            if (puntosLibro > puntosLibroInfo.PuntosMaximosPasoActual && puntosLibroInfo.PuntosMaximosPasoActual != 0)
            {
                puntosLibroInfo.PuntosPasoActual = puntosLibroInfo.PuntosMaximosPasoActual;
            }
            else
            {
                puntosLibroInfo.PuntosPasoActual = puntosLibro;
            }
            puntosLibroInfo.PuntosMaximosAcumulados = Puntos.Libro.MAXIMO_PROFESIONAL_4;
            if (puntosLibro > puntosLibroInfo.PuntosMaximosAcumulados && puntosLibroInfo.PuntosMaximosAcumulados != 0)
            {
                puntosLibroInfo.PuntosAcumulados = puntosLibroInfo.PuntosMaximosAcumulados;
            }
            else
            {
                puntosLibroInfo.PuntosAcumulados = puntosLibro;
            }

            // Artículo
            PuntosXRubroDTO puntosArticuloInfo = new PuntosXRubroDTO();
            puntosArticuloInfo.Rubro = "Artículo";
            double puntosArticulo = CalcularPuntosPorPersonaYRubro(id, "Artículo");
            switch (categoria)
            {
                case "Primera":
                    puntosArticuloInfo.PuntosMaximosPasoActual = Puntos.Articulo.MAXIMO_PROFESIONAL_2;
                    break;
                case "Segunda":
                    puntosArticuloInfo.PuntosMaximosPasoActual = Puntos.Articulo.MAXIMO_PROFESIONAL_3;
                    break;
                case "Tercera":
                    puntosArticuloInfo.PuntosMaximosPasoActual = Puntos.Articulo.MAXIMO_PROFESIONAL_4;
                    break;
                default:
                    break;
            }
            if (puntosArticulo > puntosArticuloInfo.PuntosMaximosPasoActual && puntosArticuloInfo.PuntosMaximosPasoActual != 0)
            {
                puntosArticuloInfo.PuntosPasoActual = puntosArticuloInfo.PuntosMaximosPasoActual;
            }
            else
            {
                puntosArticuloInfo.PuntosPasoActual = puntosArticulo;
            }
            puntosArticuloInfo.PuntosMaximosAcumulados = Puntos.Articulo.MAXIMO_PROFESIONAL_4;
            if (puntosArticulo > puntosArticuloInfo.PuntosMaximosAcumulados && puntosArticuloInfo.PuntosMaximosAcumulados != 0)
            {
                puntosArticuloInfo.PuntosAcumulados = puntosArticuloInfo.PuntosMaximosAcumulados;
            }
            else
            {
                puntosArticuloInfo.PuntosAcumulados = puntosArticulo;
            }

            // Obra didáctica
            PuntosXRubroDTO puntosObraDidacticaInfo = new PuntosXRubroDTO();
            puntosObraDidacticaInfo.Rubro = "Obra didáctica";
            double puntosObraDidactica = CalcularPuntosPorPersonaYRubro(id, "Obra didáctica");
            switch (categoria)
            {
                case "Primera":
                    puntosObraDidacticaInfo.PuntosMaximosPasoActual = Puntos.ObraDidactica.MAXIMO_PROFESIONAL_2;
                    break;
                case "Segunda":
                    puntosObraDidacticaInfo.PuntosMaximosPasoActual = Puntos.ObraDidactica.MAXIMO_PROFESIONAL_3;
                    break;
                case "Tercera":
                    puntosObraDidacticaInfo.PuntosMaximosPasoActual = Puntos.ObraDidactica.MAXIMO_PROFESIONAL_4;
                    break;
                default:
                    break;
            }
            if (puntosObraDidactica > puntosObraDidacticaInfo.PuntosMaximosPasoActual && puntosObraDidacticaInfo.PuntosMaximosPasoActual != 0)
            {
                puntosObraDidacticaInfo.PuntosPasoActual = puntosObraDidacticaInfo.PuntosMaximosPasoActual;
            }
            else
            {
                puntosObraDidacticaInfo.PuntosPasoActual = puntosObraDidactica;
            }
            puntosObraDidacticaInfo.PuntosMaximosAcumulados = Puntos.Articulo.MAXIMO_PROFESIONAL_4;
            if (puntosObraDidactica > puntosObraDidacticaInfo.PuntosMaximosAcumulados && puntosObraDidacticaInfo.PuntosMaximosAcumulados != 0)
            {
                puntosObraDidacticaInfo.PuntosAcumulados = puntosObraDidacticaInfo.PuntosMaximosAcumulados;
            }
            else
            {
                puntosObraDidacticaInfo.PuntosAcumulados = puntosObraDidactica;
            }

            // Obra administrativa de desarrollo
            PuntosXRubroDTO puntosObraAdmInfo = new PuntosXRubroDTO();
            puntosObraAdmInfo.Rubro = "Obra administrativa de desarrollo";
            double puntosObraAdm = CalcularPuntosPorPersonaYRubro(id, "Obra administrativa de desarrollo");
            switch (categoria)
            {
                case "Primera":
                    puntosObraAdmInfo.PuntosMaximosPasoActual = Puntos.ObraAdministrativa.MAXIMO_PROFESIONAL_2;
                    break;
                case "Segunda":
                    puntosObraAdmInfo.PuntosMaximosPasoActual = Puntos.ObraAdministrativa.MAXIMO_PROFESIONAL_3;
                    break;
                case "Tercera":
                    puntosObraAdmInfo.PuntosMaximosPasoActual = Puntos.ObraAdministrativa.MAXIMO_PROFESIONAL_4;
                    break;
                default:
                    break;
            }
            if (puntosObraAdm > puntosObraAdmInfo.PuntosMaximosPasoActual && puntosObraAdmInfo.PuntosMaximosPasoActual != 0)
            {
                puntosObraAdmInfo.PuntosPasoActual = puntosObraAdmInfo.PuntosMaximosPasoActual;
            }
            else
            {
                puntosObraAdmInfo.PuntosPasoActual = puntosObraAdm;
            }
            puntosObraAdmInfo.PuntosMaximosAcumulados = Puntos.Articulo.MAXIMO_PROFESIONAL_4;
            if (puntosObraAdm > puntosObraAdmInfo.PuntosMaximosAcumulados && puntosObraAdmInfo.PuntosMaximosAcumulados != 0)
            {
                puntosObraAdmInfo.PuntosAcumulados = puntosObraAdmInfo.PuntosMaximosAcumulados;
            }
            else
            {
                puntosObraAdmInfo.PuntosAcumulados = puntosObraAdm;
            }

            // Total
            PuntosXRubroDTO puntosTotalInfo = new PuntosXRubroDTO();
            puntosTotalInfo.Rubro = "Total";
            // Sumar los puntos de todos los tipos de rubros
            double puntosTotalPasoActual = puntosLibroInfo.PuntosPasoActual + puntosArticuloInfo.PuntosPasoActual + puntosObraDidacticaInfo.PuntosPasoActual + puntosObraAdmInfo.PuntosPasoActual;
            double puntosTotalAcumulados = puntosLibroInfo.PuntosAcumulados + puntosArticuloInfo.PuntosAcumulados + puntosObraDidacticaInfo.PuntosAcumulados + puntosObraAdmInfo.PuntosAcumulados;
            switch (categoria)
            {
                case "Primera":
                    puntosTotalInfo.PuntosMaximosPasoActual = Puntos.Total.MAXIMO_PROFESIONAL_2;
                    break;
                case "Segunda":
                    puntosTotalInfo.PuntosMaximosPasoActual = Puntos.Total.MAXIMO_PROFESIONAL_3;
                    break;
                case "Tercera":
                    puntosTotalInfo.PuntosMaximosPasoActual = Puntos.Total.MAXIMO_PROFESIONAL_4;
                    break;
                default:
                    break;
            }
            if (puntosTotalPasoActual > puntosTotalInfo.PuntosMaximosPasoActual && puntosTotalInfo.PuntosMaximosPasoActual != 0)
            {
                puntosTotalInfo.PuntosPasoActual = puntosTotalInfo.PuntosMaximosPasoActual;
            }
            else
            {
                puntosTotalInfo.PuntosPasoActual = puntosTotalPasoActual;
            }
            puntosTotalInfo.PuntosMaximosAcumulados = Puntos.Total.MAXIMO_PROFESIONAL_4;
            if (puntosTotalAcumulados > puntosTotalInfo.PuntosMaximosAcumulados && puntosTotalInfo.PuntosMaximosAcumulados != 0)
            {
                puntosTotalInfo.PuntosAcumulados = puntosTotalInfo.PuntosMaximosAcumulados;
            }
            else
            {
                puntosTotalInfo.PuntosAcumulados = puntosTotalAcumulados;
            }

            // Añadir a lista
            puntosXRubroDTOs.Add(puntosTotalInfo);
            puntosXRubroDTOs.Add(puntosLibroInfo);
            puntosXRubroDTOs.Add(puntosArticuloInfo);
            puntosXRubroDTOs.Add(puntosObraDidacticaInfo);
            puntosXRubroDTOs.Add(puntosObraAdmInfo);

            return puntosXRubroDTOs;
        }

        private List<Atestado> ObtenerAtestadosPorPersona(int idPersona)
        {
            return db.Atestado.Where(a => a.PersonaID == idPersona).ToList();
        }

        private List<Atestado> ObtenerAtestadosPorPersonaYNombreRubro(int idPersona, string rubro)
        {
            return db.Atestado.Where(a => a.PersonaID == idPersona && a.Rubro.Nombre == rubro).ToList();
        }

        private double CalcularPuntosPorPersona(int idPersona)
        {
            List<Atestado> atestados = ObtenerAtestadosPorPersona(idPersona);
            return CalcularPuntosAtestados(atestados, idPersona);
        }

        private double CalcularPuntosPorPersonaYRubro(int idPersona, string rubro)
        {
            List<Atestado> atestados = ObtenerAtestadosPorPersonaYNombreRubro(idPersona, rubro);
            return CalcularPuntosAtestados(atestados, idPersona);
        }

        private double CalcularPuntosAtestados(List<Atestado> atestados, int idPersona)
        {
            double puntos = 0;

            foreach (Atestado atestado in atestados)
            {
                puntos += CalcularPuntosAtestadoPorAutor(atestado.AtestadoID, idPersona);
            }

            return puntos;
        }

        public double CalcularPuntosAtestadoPorAutor(int? idAtestado, int idAutor)
        {
            Atestado atestado = db.Atestado.Find(idAtestado);
            AtestadoDTO atestadoDTO = Mapper.Map<Atestado, AtestadoDTO>(atestado);
            AtestadoXPersona atestadoXPersona = db.AtestadoXPersona.Find(idAtestado, idAutor);
            int maximo;
            if (atestadoXPersona == null)
            {
                return 0;
            }
            else if (atestado.Rubro.Nombre == "Libro")
            {
                return (double)((ObtenerNotaAtestadoPorAutor(atestadoDTO, idAutor) * Puntos.Libro.MAXIMO_POR_ATESTADO / 100) * (atestadoXPersona.Porcentaje / 100));
            }
            else if (atestado.Rubro.Nombre == "Artículo")
            {
                if (atestado.NumeroAutores <= 1)
                {
                    maximo = Puntos.Articulo.MAXIMO_POR_ATESTADO_UN_AUTOR;
                }
                else if (atestado.NumeroAutores == 2)
                {
                    maximo = Puntos.Articulo.MAXIMO_POR_ATESTADO_DOS_AUTORES;
                }
                else
                {
                    maximo = Puntos.Articulo.MAXIMO_POR_ATESTADO_TRES_O_MAS_AUTORES;
                }
                return (double)((ObtenerNotaAtestadoPorAutor(atestadoDTO, idAutor) * maximo / 100) * (atestadoXPersona.Porcentaje / 100));
            }
            else if (atestado.Rubro.Nombre == "Obra didáctica")
            {
                return (double)((ObtenerNotaAtestadoPorAutor(atestadoDTO, idAutor) * Puntos.ObraDidactica.MAXIMO_POR_ATESTADO / 100) * (atestadoXPersona.Porcentaje / 100));
            }
            else if (atestado.Rubro.Nombre == "Obra administrativa de desarrollo")
            {
                return (double)((ObtenerNotaAtestadoPorAutor(atestadoDTO, idAutor) * Puntos.ObraAdministrativa.MAXIMO_POR_ATESTADO / 100) * (atestadoXPersona.Porcentaje / 100));
            }
            return 0;
        }


        public List<double> CargarNotasPonderadasAutores(int? id)
        {
            List<AutorDTO> autores = CargarAutoresAtestado(id);
            List<double> notasPonderadas = new List<double>();
            Atestado atestado = db.Atestado.Find(id);
            AtestadoDTO atestadoDTO = Mapper.Map<Atestado, AtestadoDTO>(atestado);
            foreach (AutorDTO autor in autores)
            {
                notasPonderadas.Add(ObtenerNotaAtestadoPorAutor(atestadoDTO, autor.PersonaID));
            }
            return notasPonderadas;
        }

        public List<double> CargarPuntosAutores(int? id)
        {
            List<AutorDTO> autores = CargarAutoresAtestado(id);
            List<double> puntos = new List<double>();
            foreach (AutorDTO autor in autores)
            {
                puntos.Add(CalcularPuntosAtestadoPorAutor(id, autor.PersonaID));
            }
            return puntos;
        }


        #endregion

        #region InfoEditorial
        public InfoEditorialDTO CargarInfoEditorial(int? id)
        {
            InfoEditorial infoEditorial = db.InfoEditorial.Find(id);

            if (infoEditorial == null)
                return null;

            InfoEditorialDTO infoEditorialsDto = AutoMapper.Mapper.Map<InfoEditorial, InfoEditorialDTO>(infoEditorial);

            return infoEditorialsDto;

        }

        public InfoEditorial CargarInfoEditorialParaEditar(int? id)
        {
            InfoEditorial infoEditorial = db.InfoEditorial.Find(id);

            if (infoEditorial == null)
                return null;

            return infoEditorial;

        }

        public InfoEditorial CargarInfoEditorialParaBorrar(int? id)
        {
            InfoEditorial infoEditorial = db.InfoEditorial.Find(id);

            if (infoEditorial == null)
                return null;

            return infoEditorial;

        }

        public List<InfoEditorialDTO> CargarInfoEditoriales()
        {

            List<InfoEditorial> listaInfoEditorial = db.InfoEditorial.Include(i => i.Atestado).ToList();

            List<InfoEditorialDTO> listaInfoEditorialsDto = AutoMapper.Mapper.Map<List<InfoEditorial>, List<InfoEditorialDTO>>(listaInfoEditorial);

            return listaInfoEditorialsDto;

        }

        public void GuardarInfoEditorial(InfoEditorial infoEditorial)
        {
            db.InfoEditorial.Add(infoEditorial);
            db.SaveChanges();
        }

        public void EditarInfoEditorial(InfoEditorial infoEditorial)
        {
            db.Entry(infoEditorial).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void BorrarInfoEditorial(int id)
        {
            InfoEditorial infoEditorial = db.InfoEditorial.Find(id);
            db.InfoEditorial.Remove(infoEditorial);
            db.SaveChanges();
        }
        #endregion

        #region Fecha
        public FechaDTO CargarFecha(int? id)
        {
            Fecha fecha = db.Fecha.Find(id);

            if (fecha == null)
                return null;

            FechaDTO fechasDto = AutoMapper.Mapper.Map<Fecha, FechaDTO>(fecha);

            return fechasDto;

        }

        public Fecha CargarFechaParaEditar(int? id)
        {
            Fecha fecha = db.Fecha.Find(id);

            if (fecha == null)
                return null;

            return fecha;

        }

        public Fecha CargarFechaParaBorrar(int? id)
        {
            Fecha fecha = db.Fecha.Find(id);

            if (fecha == null)
                return null;

            return fecha;

        }

        public List<FechaDTO> CargarFechas()
        {

            List<Fecha> listaFecha = db.Fecha.Include(f => f.Atestado).ToList();

            List<FechaDTO> listaFechasDto = AutoMapper.Mapper.Map<List<Fecha>, List<FechaDTO>>(listaFecha);

            return listaFechasDto;

        }

        public void GuardarFecha(Fecha fecha)
        {
            db.Fecha.Add(fecha);
            db.SaveChanges();
        }

        public void EditarFecha(Fecha fecha)
        {
            db.Entry(fecha).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void BorrarFecha(int id)
        {
            Fecha fecha = db.Fecha.Find(id);
            db.Fecha.Remove(fecha);
            db.SaveChanges();
        }
        #endregion

        #region Archivo

        public void BorrarArchivos(int? id)
        {
            db.Archivo.RemoveRange(db.Archivo.Where(x => x.AtestadoID == id));
            db.SaveChanges();
        }

        public ArchivoDTO CargarArchivo(int? id)
        {
            Archivo archivo = db.Archivo.Find(id);

            if (archivo == null)
                return null;

            ArchivoDTO archivosDto = AutoMapper.Mapper.Map<Archivo, ArchivoDTO>(archivo);

            return archivosDto;

        }

        public Archivo CargarArchivoParaEditar(int? id)
        {
            Archivo archivo = db.Archivo.Find(id);

            if (archivo == null)
                return null;

            return archivo;

        }

        public Archivo CargarArchivoParaBorrar(int? id)
        {
            Archivo archivo = db.Archivo.Find(id);

            if (archivo == null)
                return null;

            return archivo;

        }

        public List<ArchivoDTO> CargarArchivos()
        {

            List<Archivo> listaArchivo = db.Archivo.ToList();

            List<ArchivoDTO> listaArchivosDto = AutoMapper.Mapper.Map<List<Archivo>, List<ArchivoDTO>>(listaArchivo);

            return listaArchivosDto;

        }

        public List<ArchivoDTO> CargarArchivosDeAtestado(int? id)
        {

            List<Archivo> listaArchivo = db.Archivo.Where(archivo => archivo.AtestadoID == id).ToList();

            List<ArchivoDTO> listaArchivosDto = AutoMapper.Mapper.Map<List<Archivo>, List<ArchivoDTO>>(listaArchivo);

            return listaArchivosDto;

        }

        public void GuardarArchivo(Archivo archivo)
        {
            db.Archivo.Add(archivo);
            db.SaveChanges();
        }

        public void EditarArchivo(Archivo archivo)
        {
            db.Entry(archivo).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void BorrarArchivo(int id)
        {
            Archivo archivo = db.Archivo.Find(id);
            db.Archivo.Remove(archivo);
            db.SaveChanges();
        }
        #endregion

        #region AtestadoXPersona

        public List<AutorDTO> CargarAutoresAtestado(int? id)
        {
            var query = db.Persona
                    .Join(db.AtestadoXPersona,
                          m => m.PersonaID,
                          v => v.PersonaID,
                          (m, v) => new { m, v })
                    .Where(x => x.v.AtestadoID == id)
                    .Select(x => new AutorDTO() { 
                        Nombre = x.m.Nombre,
                        PrimerApellido = x.m.PrimerApellido,
                        SegundoApellido = x.m.SegundoApellido,
                        Email = x.m.Email,
                        Porcentaje = (double)x.v.Porcentaje,
                        PersonaID = x.m.PersonaID,
                        AtestadoID = x.v.AtestadoID,
                        Departamento = x.v.Departamento
                    });

            return query.ToList();
        }

        public List<AtestadoXPersona> CargarAtestadoXPersonasdeAtestado(int? id)
        {
            return db.AtestadoXPersona.Where(x => x.AtestadoID == id).ToList();
        }

        public AtestadoXPersonaDTO CargarAtestadoXPersona(int? id)
        {
            AtestadoXPersona atestadoXPersona = db.AtestadoXPersona.Find(id);

            if (atestadoXPersona == null)
                return null;

            AtestadoXPersonaDTO atestadoXPersonasDto = AutoMapper.Mapper.Map<AtestadoXPersona, AtestadoXPersonaDTO>(atestadoXPersona);

            return atestadoXPersonasDto;

        }

        public AtestadoXPersona CargarAtestadoXPersonaParaEditar(int? id)
        {
            AtestadoXPersona atestadoXPersona = db.AtestadoXPersona.Find(id);

            if (atestadoXPersona == null)
                return null;

            return atestadoXPersona;

        }

        public AtestadoXPersona CargarAtestadoXPersonaParaBorrar(int? id)
        {
            AtestadoXPersona atestadoXPersona = db.AtestadoXPersona.Find(id);

            if (atestadoXPersona == null)
                return null;

            return atestadoXPersona;

        }

        public List<AtestadoXPersonaDTO> CargarAtestadoXPersonas()
        {

            List<AtestadoXPersona> listaAtestadoXPersona = db.AtestadoXPersona.Include(a => a.Atestado).Include(a => a.Persona).ToList();

            List<AtestadoXPersonaDTO> listaAtestadoXPersonasDto = AutoMapper.Mapper.Map<List<AtestadoXPersona>, List<AtestadoXPersonaDTO>>(listaAtestadoXPersona);

            return listaAtestadoXPersonasDto;

        }

        public void GuardarAtestadoXPersona(AtestadoXPersona atestadoXPersona)
        {
            db.AtestadoXPersona.Add(atestadoXPersona);
            db.SaveChanges();
        }

        public void EditarAtestadoXPersona(AtestadoXPersona atestadoXPersona)
        {
            db.Entry(atestadoXPersona).State = EntityState.Modified;
            db.SaveChanges();
        }

        // Borra las personas asociadas a un atestado por medio de atestadoID.
        public void BorrarAtestadoXPersona(int atestadoID)
        {
            var personasXAtestado = db.AtestadoXPersona.Where(x => x.AtestadoID == atestadoID);
            foreach (var persona in personasXAtestado)
            {
                db.AtestadoXPersona.Remove(persona);
            }
            db.SaveChanges();
        }
        // Old version
        //public void BorrarAtestadoXPersona(int id)
        //{
        //    AtestadoXPersona atestadoXPersona = db.AtestadoXPersona.Find(id);
        //    db.AtestadoXPersona.Remove(atestadoXPersona);
        //    db.SaveChanges();
        //}
        #endregion

        #region DominioIdioma

        public int ObtenerIdioma(string nombre)
        {
            List<Idioma> idioma = db.Idioma.Where(l => l.Nombre.Contains(nombre.Trim().ToLower())).ToList();
            return idioma.Count > 0 ? idioma.FirstOrDefault().IdiomaID : -1;
        }

        public DominioIdiomaDTO CargarDominioIdioma(int? id)
        {
            DominioIdioma dominioIdioma = db.DominioIdioma.Find(id);

            if (dominioIdioma == null)
                return null;

            DominioIdiomaDTO dominioIdiomasDto = AutoMapper.Mapper.Map<DominioIdioma, DominioIdiomaDTO>(dominioIdioma);

            return dominioIdiomasDto;

        }

        public DominioIdioma CargarDominioIdiomaParaEditar(int? id)
        {
            DominioIdioma dominioIdioma = db.DominioIdioma.Find(id);

            if (dominioIdioma == null)
                return null;

            return dominioIdioma;

        }

        public DominioIdioma CargarDominioIdiomaParaBorrar(int? id)
        {
            DominioIdioma dominioIdioma = db.DominioIdioma.Find(id);

            if (dominioIdioma == null)
                return null;

            return dominioIdioma;

        }

        public List<DominioIdiomaDTO> CargarDominioIdiomas()
        {

            List<DominioIdioma> listaDominioIdioma = db.DominioIdioma.Include(a => a.Atestado).Include(a => a.Idioma).ToList();

            List<DominioIdiomaDTO> listaDominioIdiomasDto = AutoMapper.Mapper.Map<List<DominioIdioma>, List<DominioIdiomaDTO>>(listaDominioIdioma);

            return listaDominioIdiomasDto;

        }

        public void GuardarDominioIdioma(DominioIdioma dominioIdioma)
        {
            db.DominioIdioma.Add(dominioIdioma);
            db.SaveChanges();
        }

        public void EditarDominioIdioma(DominioIdioma dominioIdioma)
        {
            db.Entry(dominioIdioma).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void BorrarDominioIdioma(int id)
        {
            DominioIdioma dominioIdioma = db.DominioIdioma.Find(id);
            db.DominioIdioma.Remove(dominioIdioma);
            db.SaveChanges();
        }

        #endregion
    }
}
