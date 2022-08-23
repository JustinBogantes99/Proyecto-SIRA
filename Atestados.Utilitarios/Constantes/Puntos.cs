using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Utilitarios.Puntos
{
    public static class Puntos
    {
        public struct Total
        {
            public const int MAXIMO_PROFESIONAL_2 = 24;
            public const int MAXIMO_PROFESIONAL_3 = 72;
            public const int MAXIMO_PROFESIONAL_4 = 144;
        }
        public struct Libro
        {
            public const int MAXIMO_POR_ATESTADO = 14;
            public const int MAXIMO_PROFESIONAL_2 = 24;
            public const int MAXIMO_PROFESIONAL_3 = 72;
            public const int MAXIMO_PROFESIONAL_4 = 144;
        }

        public struct Articulo
        {
            public const int MAXIMO_POR_ATESTADO_UN_AUTOR = 4;
            public const int MAXIMO_POR_ATESTADO_DOS_AUTORES = 6;
            public const int MAXIMO_POR_ATESTADO_TRES_O_MAS_AUTORES = 8;
            public const int MAXIMO_PROFESIONAL_2 = 8;
            public const int MAXIMO_PROFESIONAL_3 = 20;
            public const int MAXIMO_PROFESIONAL_4 = 144;
        }

        public struct ObraDidactica
        {
            public const int MAXIMO_POR_ATESTADO = 4;
            public const int MAXIMO_PROFESIONAL_2 = 4;
            public const int MAXIMO_PROFESIONAL_3 = 10;
            public const int MAXIMO_PROFESIONAL_4 = 12;
        }

        public struct ObraAdministrativa
        {
            public const int MAXIMO_POR_ATESTADO = 10;
            public const int MAXIMO_PROFESIONAL_2 = 24;
            public const int MAXIMO_PROFESIONAL_3 = 72;
            public const int MAXIMO_PROFESIONAL_4 = 144;
        }

        public struct TiempoPasoCategoria
        {
            public const int SIN_CATEGORIA = 0;
            public const int PRIMERA = 3;
            public const int SEGUNDA = 8;
            public const int TERCERA = 14;
            public const int CUARTA = 0;
        }
    }

}
