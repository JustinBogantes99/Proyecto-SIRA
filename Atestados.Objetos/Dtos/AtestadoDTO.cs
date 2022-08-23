using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos.Dtos
{
    public class AtestadoDTO
    {
        public bool AutoresCheck { get; set; }
        public bool AutoresEq { get; set; }
        public int AtestadoID { get; set; }
        [Required]
        [StringLength(250)]
        public string Nombre { get; set; }
        [DisplayName("Número de autores")]
        public int NumeroAutores { get; set; }
        [StringLength(1000)]
        public string Observaciones { get; set; }
        public DateTime HoraCreacion { get; set; }
        public int Enviado { get; set; }
        public int Descargado { get; set; }
        public int CantidadHoras { get; set; }
        [StringLength(250)]
        public string Lugar { get; set; }
        [StringLength(100)]
        public string CatalogoTipo { get; set; }
        [StringLength(250)]
        public string Enlace { get; set; }
        public int RubroID { get; set; }
        public int PaisID { get; set; }
        public int PersonaID { get; set; }
        [StringLength(100)]
        [RegularExpression(@"^(\d{4}-\d{4}-\d{4})$|^(\d{7})$", ErrorMessage = "No es un código válido. Formato: ####-####-#### o #######")]
        public string Codigo { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaFinal { get; set; }

        public List<ArchivoDTO> Archivos { get; set; }
        public PaisDTO Pais { get; set; }
        public PersonaDTO Persona { get; set; }
        public RubroDTO Rubro { get; set; }
        public List<AtestadoXPersonaDTO> AtestadoXPersona { get; set; }
        public DominioIdiomaDTO DominioIdioma { get; set; }
        public FechaDTO Fecha { get; set; }
        public InfoEditorialDTO InfoEditorial { get; set; }
        public bool MarcarEnviado { get; set; }
        public bool MarcarDescargado { get; set; }
    }
}
