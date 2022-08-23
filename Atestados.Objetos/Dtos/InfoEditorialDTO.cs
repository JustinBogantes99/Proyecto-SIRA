using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos.Dtos
{
    public class InfoEditorialDTO
    {
        public int InfoEditorialID { get; set; }
        public string Editorial { get; set; }
        [Url]
        public string Website { get; set; }
        public string Atestado { get; set; }
    }
}
