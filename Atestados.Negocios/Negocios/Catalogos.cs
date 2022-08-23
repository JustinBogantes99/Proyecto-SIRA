using Atestados.Datos.Modelo;
using Atestados.Objetos.Dtos;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Negocios.Negocios
{
    public class Catalogos
    {
        private AtestadosEntities db = new AtestadosEntities();

        #region TipoRubro
        public List<TipoRubroDTO> CargarTiposDeRubros()
        {
            //var config = new MapperConfiguration(cfg => cfg.Create)

            List<TipoRubro> listaTipoRubro = db.TipoRubro.ToList();

            List<TipoRubroDTO> listaTipoRubrosDto = AutoMapper.Mapper.Map<List<TipoRubro>, List<TipoRubroDTO>>(listaTipoRubro);

            return listaTipoRubrosDto;
        }

        public TipoRubroDTO CargarTipoRubro(int? id)
        {
            TipoRubro rubro = db.TipoRubro.Find(id);

            if (rubro == null)
                return null;

            TipoRubroDTO rubroDto = AutoMapper.Mapper.Map<TipoRubro, TipoRubroDTO>(rubro);
            return rubroDto;
        }

        public TipoRubro CargarTipoRubroParaEditar(int? id)
        {
            TipoRubro tipoRubro = db.TipoRubro.Find(id);

            if (tipoRubro == null)
                return null;

            return tipoRubro;

        }

        public TipoRubro CargarTipoRubroParaBorrar(int? id)
        {
            TipoRubro tipoRubro = db.TipoRubro.Find(id);

            if (tipoRubro == null)
                return null;

            return tipoRubro;

        }

        public void GuardarTipoRubro(TipoRubro tipoRubro)
        {
            db.TipoRubro.Add(tipoRubro);
            db.SaveChanges();
        }

        public void EditarTipoRubro(TipoRubro tipoRubro)
        {
            db.Entry(tipoRubro).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void BorrarTipoRubro(int id)
        {
            TipoRubro tipoRubro = db.TipoRubro.Find(id);
            db.TipoRubro.Remove(tipoRubro);
            db.SaveChanges();
        }

        #endregion

        #region Rubro
        public List<RubroDTO> CargarRubros()
        {
            //var config = new MapperConfiguration(cfg => cfg.Create)

            List<Rubro> listaRubro = db.Rubro.ToList();

            List<RubroDTO> listaRubrosDto = AutoMapper.Mapper.Map<List<Rubro>, List<RubroDTO>>(listaRubro);

            return listaRubrosDto;
        }

        public RubroDTO CargarRubro(int? id)
        {
            Rubro rubro = db.Rubro.Find(id);

            if (rubro == null)
                return null;

            RubroDTO rubroDto = AutoMapper.Mapper.Map<Rubro, RubroDTO>(rubro);
            return rubroDto;
        }

        public Rubro CargarRubroParaEditar(int? id)
        {
            Rubro rubro = db.Rubro.Find(id);

            if (rubro == null)
                return null;

            return rubro;

        }

        public Rubro CargarRubroParaBorrar(int? id)
        {
            Rubro rubro = db.Rubro.Find(id);

            if (rubro == null)
                return null;

            return rubro;

        }

        public void GuardarRubro(Rubro rubro)
        {
            db.Rubro.Add(rubro);
            db.SaveChanges();
        }

        public void EditarRubro(Rubro rubro)
        {
            db.Entry(rubro).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void BorrarRubro(int id)
        {
            Rubro rubro = db.Rubro.Find(id);
            db.Rubro.Remove(rubro);
            db.SaveChanges();
        }


        #endregion

        #region Pais

        public List<PaisDTO> CargarPaises()
        {
            //var config = new MapperConfiguration(cfg => cfg.Create)

            List<Pais> listaPais = db.Pais.ToList();

            List<PaisDTO> listaPaissDto = AutoMapper.Mapper.Map<List<Pais>, List<PaisDTO>>(listaPais);

            return listaPaissDto;
        }

        public PaisDTO CargarPais(int? id)
        {
            Pais pais = db.Pais.Find(id);

            if (pais == null)
                return null;

            PaisDTO paisDto = AutoMapper.Mapper.Map<Pais, PaisDTO>(pais);
            return paisDto;
        }

        public Pais CargarPaisParaEditar(int? id)
        {
            Pais pais = db.Pais.Find(id);

            if (pais == null)
                return null;

            return pais;

        }

        public Pais CargarPaisParaBorrar(int? id)
        {
            Pais pais = db.Pais.Find(id);

            if (pais == null)
                return null;

            return pais;

        }

        public void GuardarPais(Pais pais)
        {
            db.Pais.Add(pais);
            db.SaveChanges();
        }

        public void EditarPais(Pais pais)
        {
            db.Entry(pais).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void BorrarPais(int id)
        {
            Pais pais = db.Pais.Find(id);
            db.Pais.Remove(pais);
            db.SaveChanges();
        }

        #endregion

        #region Idioma
        public List<IdiomaDTO> CargarIdiomas()
        {
            //var config = new MapperConfiguration(cfg => cfg.Create)

            List<Idioma> listaIdioma = db.Idioma.ToList();

            List<IdiomaDTO> listaIdiomasDto = AutoMapper.Mapper.Map<List<Idioma>, List<IdiomaDTO>>(listaIdioma);

            return listaIdiomasDto;
        }

        public IdiomaDTO CargarIdioma(int? id)
        {
            Idioma idioma = db.Idioma.Find(id);

            if (idioma == null)
                return null;

            IdiomaDTO idiomaDto = AutoMapper.Mapper.Map<Idioma, IdiomaDTO>(idioma);
            return idiomaDto;
        }

        public Idioma CargarIdiomaParaEditar(int? id)
        {
            Idioma idioma = db.Idioma.Find(id);

            if (idioma == null)
                return null;

            return idioma;

        }

        public Idioma CargarIdiomaParaBorrar(int? id)
        {
            Idioma idioma = db.Idioma.Find(id);

            if (idioma == null)
                return null;

            return idioma;

        }

        public void GuardarIdioma(Idioma idioma)
        {
            db.Idioma.Add(idioma);
            db.SaveChanges();
        }

        public void EditarIdioma(Idioma idioma)
        {
            db.Entry(idioma).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void BorrarIdioma(int id)
        {
            Idioma idioma = db.Idioma.Find(id);
            db.Idioma.Remove(idioma);
            db.SaveChanges();
        }


        #endregion
    }
}
