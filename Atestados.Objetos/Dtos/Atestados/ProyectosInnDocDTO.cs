using Atestados.Datos.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos.Dtos.Atestados
{
    public class ProyectosInnDocDTO
    {
        public bool AutoresCheck { get; set; }
        public bool AutoresEq { get; set; }
        private string Rubro = "Proyectos de innovación docente"; // esta como otras obras profesionales
        public int AtestadoID { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = "Este es un campo requerido")]
        public string Nombre { get; set; }
        [StringLength(1000)]
        public int NumeroAutores { get; set; }
        [StringLength(1000)]
        public string Observaciones { get; set; }
        public int CantidadHoras { get; set; }
        [StringLength(250)]
        public int HorasInvertidasXSemana { get; set; }
        [StringLength(100)]
        public int HorasInvertidasTotales { get; set; }
        [StringLength(100)]
        [RegularExpression(@"^(\d{4}-\d{4}-\d{4})$|^(\d{7})$", ErrorMessage = "No es un código válido. Formato: ####-####-#### o #######")]
        [Required(ErrorMessage = "Este es un campo requerido")]
        public int MesesInvertidos { get; set; }
        public string Codigo { get; set; }
        [StringLength(250)]
        [Url]
        public string Enlace { get; set; }
        public int RubroID { get; set; }
        public int PaisID { get; set; }
        public int PersonaID { get; set; }
        [Required(ErrorMessage = "Este es un campo requerido")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }
        [Required(ErrorMessage = "Este es un campo requerido")]
        [DataType(DataType.Date)]
        public DateTime FechaFinal { get; set; }

        public List<AutorDTO> Autores { get; set; }
        public List<ArchivoDTO> Archivos { get; set; }
        public List<AtestadoXPersonaDTO> AtestadoXPersona { get; set; }
    }
}
