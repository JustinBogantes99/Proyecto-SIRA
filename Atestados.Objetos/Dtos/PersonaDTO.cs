using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos.Dtos
{
    public class PersonaDTO
    {
        public int PersonaID { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string Email { get; set; }
        public int CategoriaActual { get; set; }
        public int TipoUsuario { get; set; }
        public int Telefono { get; set; }

        public int TiempoServido { get; set; }
        public List<AtestadoDTO> PorEnviar { get; set; }
        public bool esActivo { get; set; }

        // Función utilizada obtener el nombre de la vista de un atestado, para
        // poder redireccionar a una vista de dicho atestado. Obtiene el ID del
        // atestados y retorna el nombre interno de la vista. Como Rubro es una
        // tabla catálogo en la base de datos, se espera que se siemrpe tengan
        // los mismos IDs. En caso de que los IDs de esa tabla se modifiquen,
        // esta función tendrá que ser modificada también.
        public string getNombreVista(int id)
        {
            switch (id)
            {
                case 1:
                    return "Libro";
                case 2:
                    return "Articulo";
                case 5:
                    return "ObraDidactica";
                case 6:
                    return "ObraAdministrativa";
                case 7:
                    return "OtrasObrasProf";
                case 9:
                    return "ProyectosInvEx";
                case 34:
                    return "Idioma";
                default:
                    return "Atestados";
            }
        }
    }

}
