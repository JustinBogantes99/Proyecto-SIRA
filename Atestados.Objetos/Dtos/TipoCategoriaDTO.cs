using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos.Dtos
{
    public class TipoCategoriaDTO
    {
        public int TipoCategoriaID { get; set; }
        public string Nombre { get; set; }
        public string Academicos { get; set; }
        public string Administrativos { get; set; }
    }
}
