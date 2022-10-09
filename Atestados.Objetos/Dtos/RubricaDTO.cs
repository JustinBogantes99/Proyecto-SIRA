using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos.Dtos
{
    public class RubricaDTO
    {
        public int RubricaID { get; set; }
        public int TipoPuntajeID { get; set; }
        public int ValorPuntaje { get; set; }
        public string DescripcionPuntaje { get; set; } //CONSULTAR
        public int EsManual { get; set; } //CONSULTAR
        public DateTime Fecha { get; set; }
        public int AtestadoID { get; set; }
        public RubroDTO Rubro { get; set; }
        public string Nombre { get; set; }
        [DisplayName("Nombre del Criterio")]
        public string CriterioNombre { get; set; }
        [DisplayName("Valor del Criterio")]
        public int CriterioPuntaje { get; set; }
        public RubricaDTO() { }
    }
}
