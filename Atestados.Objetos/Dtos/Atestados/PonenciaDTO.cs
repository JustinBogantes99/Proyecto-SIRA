using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos.Dtos
{
    public class PonenciaDTO
    {
        public int AtestadoID { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = "Este es un campo requerido")]
        public string Nombre { get; set; }
        [StringLength(1000)]
        public string Observaciones { get; set; }
        public DateTime HoraCreacion { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = "Este es un campo requerido")]
        public string Lugar { get; set; }
        [StringLength(250)]
        [Url]
        public string Enlace { get; set; }
        public int RubroID { get; set; }
        public int PaisID { get; set; }
        public int PersonaID { get; set; }
        [Required(ErrorMessage = "Este es un campo requerido")]
        [DataType(DataType.Date)]
        public DateTime FechaPonencia { get; set; }

        public List<ArchivoDTO> Archivos { get; set; }
    }
}
