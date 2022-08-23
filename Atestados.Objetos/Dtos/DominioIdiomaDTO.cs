using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos.Dtos
{
    public class DominioIdiomaDTO
    {
        public int DominioIdiomaID { get; set; }
        public int IdiomaID { get; set; }
        public string Idioma { get; set; }
        public int Lectura { get; set; }
        public int Escrito { get; set; }
        public int Auditiva { get; set; }
        public int Oral { get; set; }
        public string Atestado { get; set; }
    }
}
