using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos.Dtos
{
    public class AutorDTO
    {
        public int numAutor { get; set; }
        public int AtestadoID { get; set; }
        public int PersonaID { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string Email { get; set; }
        [Range(0, 100)]
        public double Porcentaje { get; set; }
        [StringLength(250)]
        public string Departamento { get; set; }
        public bool esFuncionario { get; set; }
        public bool porcEquitativo { get; set; }

    }
}
