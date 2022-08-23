using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos.Dtos
{
    public class UsuarioDTO
    {
        public int UsuarioID { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contrasena { get; set; }
        [Compare("Contrasena")]
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string ConfirmarContrasena { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public int CategoriaActual { get; set; }
        public int TipoUsuario { get; set; }
        public string TipoUsuarioNombre { get; set; }
        public string CategoriaActualNombre { get; set; }
        [RegularExpression(@"^(\d{8})$", ErrorMessage = "Número no válido.")]
        public int Telefono { get; set; }

        public int TiempoServido { get; set; }
        public bool esActivo { get; set; }

        public string NombreCompleto()
        {
            return $"{Nombre} {PrimerApellido} {SegundoApellido}";
        }
        public string NombreCorto()
        {
            return $"{Nombre} {PrimerApellido}";
        }

        public string TipoUsuarioToStr()
        {
            if (TipoUsuario == 1)
                return "Admin";
            if (TipoUsuario == 2)
                return "Revisor";
            if (TipoUsuario == 3)
                return "Funcionario";
            else
                return "Tipo usuario inválido.";
        }
    }
}
