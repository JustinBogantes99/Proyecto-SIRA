using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atestados.Datos.Modelo;
namespace Atestados.Objetos.Dtos
{
    public class RubricaDTO
    {
        public int RubricaID { get; set; }
        public int TipoPuntajeID { get; set; }
        public int PuntajeTiempoID { get; set; }
        public Nullable<double> ValorPuntaje { get; set; }
        public string DescripcionPuntaje { get; set; }
        public DateTime Fecha { get; set; }
        public int RubroID { get; set; }
        public Rubro Rubro { get; set; }
        public TipoPuntaje TipoPuntaje { get; set; }
        public PuntajeTiempo PuntajeTiempo { get; set; }
        public List<Evaluacion> Evaluacion { get; set; }
        public List<Requisito> Requisito { get; set; }
        public List<SeleccionPuntaje> SeleccionPuntaje { get; set; }
        public RubricaDTO() { }
    }
}
