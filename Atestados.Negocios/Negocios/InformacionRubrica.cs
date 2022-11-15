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
    public class InformacionRubrica
    {
        private readonly AtestadosEntities db = new AtestadosEntities();

        #region Requisito

        public Requisito CargarRequisito(int? id)
        {
            return db.Requisito.Find(id);
        }

        public void BorrarRequisito(int? id)
        {
            db.Requisito.RemoveRange(db.Requisito.Where(r => r.RequisitoID == id));
            db.SaveChanges();
        }

        public void EditarRequisito(Requisito requisito)
        {
            db.Entry(requisito).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void GuardarRequisito(Requisito requisito)
        {
            db.Requisito.Add(requisito);
            db.SaveChanges();
        }

        public List<Requisito> CargarRequisitosDeRubrica(int? id)
        {
            return db.Requisito.Where(r => r.RubricaID == id).ToList();
        }

        public void BorrarRequisitosDeRubrica(int? id)
        {
            db.Requisito.RemoveRange(db.Requisito.Where(r => r.RubricaID == id));
            db.SaveChanges();
        }

        #endregion

        #region SeleccionPuntaje

        public SeleccionPuntaje CargarSeleccionPuntaje(int? id)
        {
            return db.SeleccionPuntaje.Find(id);
        }

        public void BorrarSeleccionPuntaje(int? id)
        {
            db.SeleccionPuntaje.RemoveRange(db.SeleccionPuntaje.Where(s => s.SeleccionPuntajeID == id));
            db.SaveChanges();
        }

        public void EditarSeleccionPuntaje(SeleccionPuntaje seleccionPuntaje)
        {
            db.Entry(seleccionPuntaje).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void GuardarSeleccionPuntaje(SeleccionPuntaje seleccionPuntaje)
        {
            db.SeleccionPuntaje.Add(seleccionPuntaje);
            db.SaveChanges();
        }

        public List<SeleccionPuntaje> CargarSeleccionPuntajesDeRubrica(int? id)
        {
            return db.SeleccionPuntaje.Where(s => s.RubricaID == id).ToList();
        }

        public void BorrarSeleccionPuntajesDeRubrica(int? id)
        {
            db.SeleccionPuntaje.RemoveRange(db.SeleccionPuntaje.Where(s => s.RubricaID == id));
            db.SaveChanges();
        }

        #endregion

        #region Rubrica

        public Rubrica CargarRubrica(int? id)
        {
            return db.Rubrica.Find(id);
        }

        public Rubrica CargarRubricaActualPorRubro(int? idRubro)
        {
            return db.Rubrica
                .Where(ru => ru.RubroID == idRubro)
                .OrderByDescending(ru => ru.Fecha)
                .Take(1).First();
        }

        public void BorrarRubrica(int? id)
        {
            db.Rubrica.RemoveRange(db.Rubrica.Where(r => r.RubricaID == id));
            db.SaveChanges();
        }

        public void EditarRubrica(Rubrica rubrica)
        {
            db.Entry(rubrica).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void GuardarRubrica(Rubrica rubrica)
        {
            db.Rubrica.Add(rubrica);
            db.SaveChanges();
        }

        #endregion

        #region Evaluacion

        public void GuardarEvaluacion(Evaluacion evaluacion)
        {
            db.Evaluacion.RemoveRange(db.Evaluacion.Where(e => e.AtestadoID == evaluacion.AtestadoID));
            db.Evaluacion.Add(evaluacion);
            db.SaveChanges();
        }

        #endregion

    }
}
