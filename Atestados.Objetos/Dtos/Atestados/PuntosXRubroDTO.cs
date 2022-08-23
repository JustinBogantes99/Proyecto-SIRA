using Atestados.Datos.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos.Dtos
{
    public class PuntosXRubroDTO
    {
        public string Rubro { get; set; }
        public double PuntosPasoActual { get; set; }
        public double PuntosMaximosPasoActual { get; set; }
        public double PuntosAcumulados { get; set; }
        public double PuntosMaximosAcumulados { get; set; }
    }
}
