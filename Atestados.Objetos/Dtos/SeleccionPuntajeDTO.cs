using Atestados.Datos.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos.Dtos
{
    public class SeleccionPuntajeDTO
    {
        public int SeleccionPuntajeID { get; set; }
        public string Descripcion { get; set; }
        public double Puntaje { get; set; }
        public int RubricaID { get; set; }
        public virtual Rubrica Rubrica { get; set; }
    }
}
