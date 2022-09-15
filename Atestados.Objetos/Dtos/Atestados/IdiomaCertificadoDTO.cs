using Atestados.Datos.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Atestados.Objetos.Dtos
{
    public class IdiomaCertificadoDTO
    {
        public int AtestadoID { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = "Este es un campo requerido")]
        public string Nombre { get; set; }
        [StringLength(1000)]
        public string Observaciones { get; set; }
        [StringLength(250)]
        [Url(ErrorMessage = "Introduzca un URL válido")]
        public string Enlace { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Año")]
        public DateTime Annio { get; set; }
        [DisplayName("País")]
        public int PaisID { get; set; }
        public Pais Pais { get; set; }
        public Persona Persona { get; set; }
        public int RubroID { get; set; }
        [DisplayName("Persona")]
        public int PersonaID { get; set; }
        [DisplayName("Instancia certificadora")]
        public string Lugar { get; set; }

        [DisplayName("Idioma")]
        public int IdiomaID { get; set; }
        public int Lectura { get; set; }
        public int Escrito { get; set; }
        public int Auditiva { get; set; }
        public int Oral { get; set; }
        public string Atestado { get; set; }

        public List<ArchivoDTO> Archivos { get; set; }
    }
}
