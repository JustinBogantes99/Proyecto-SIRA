using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atestados.Datos.Modelo;

namespace Atestados.Objetos.Dtos
{
    public class RequisitoDTO
    {
        public int RequisitoID { get; set; }
        public string Descripcion { get; set; }
        public int RubricaID { get; set; }
        public Rubrica Rubrica { get; set; }
    }
}
