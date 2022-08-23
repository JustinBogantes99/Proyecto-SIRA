using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos.Dtos
{
    public class AtestadoXPersonaDTO
    {
        public int AtestadoID { get; set; }
        public int PersonaID { get; set; }
        public double Porcentaje { get; set; }
        public string Departamento { get; set; }
        public virtual string Atestado { get; set; }
        public virtual string Persona { get; set; }
    }
}
