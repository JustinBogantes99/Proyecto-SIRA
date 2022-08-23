using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos.Dtos
{
    public class ArchivoDTO
    {
        public int numArchivo { get; set; }
        public int ArchivoID { get; set; }
        public bool Obligatorio { get; set; }
        public string Nombre { get; set; }
        public byte[] Datos { get; set; }
        public string TipoArchivo { get; set; }
        public int AtestadoID { get; set; }
        public ArchivoDTO() { }

        public ArchivoDTO(int numArchivo, int archivoID, bool obligatorio, string nombre, byte[] datos, string tipoArchivo, int atestadoID)
        {
            this.numArchivo = numArchivo;
            ArchivoID = archivoID;
            Obligatorio = obligatorio;
            Nombre = nombre;
            Datos = datos;
            TipoArchivo = tipoArchivo;
            AtestadoID = atestadoID;
        }

        public ArchivoDTO(ArchivoDTO archOld)
        {
            numArchivo = archOld.numArchivo;
            ArchivoID = archOld.ArchivoID;
            Obligatorio = archOld.Obligatorio;
            Nombre = archOld.Nombre;
            Datos = archOld.Datos;
            TipoArchivo = archOld.TipoArchivo;
            AtestadoID = archOld.AtestadoID;
        }

    }
}
