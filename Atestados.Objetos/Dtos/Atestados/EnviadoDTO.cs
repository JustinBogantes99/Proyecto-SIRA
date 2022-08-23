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
    public class EnviadoDTO
    {
        public int PersonaID { get; set; }
        public PersonaDTO Persona { get; set; }
        public List<Atestado> Atestados { get; set; }
    }
}
