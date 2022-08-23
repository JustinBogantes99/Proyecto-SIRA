using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Utilitarios.Constantes
{
    public static class Constantes
    {

        public enum TIPO_USUARIO 
        {
            Admin, Revisor, Docente
        }

        public enum RESULTADO_LOGIN
        {
            Exito, ContrasenaIncorrecta, UsuarioNoExiste
        }

        public struct TIPO_CATEGORIA
        {
            public const string
                sin = "Sin Categoría",
                primera = "Primera",
                segunda = "Segunda",
                tercera = "Tercera",
                cuarta = "Cuarta";
        }

        public struct Configuraciones
        {
            public const string CodigoExito = "CodigoExito";
            public const int AnnoMinimoAdmitido = 1900;
            public const string UserDefault = "UserDefaultSistema";
            public const int codigoAplicacionSeguridad = 27; //TODO: Es código de prueba, debe cambiarse por el real
            public const string caracterSplit = ";";
        }

        public struct Respuesta
        {
            public const int CODIGOEXITO = 0;
            public const int CODIGOERROR = -1;
            public const int CODIGOFALLOAtestadosCREACION = -999;
            public const int CODIGOUSUARIOASOCIADOAOTRODOMINIO = 9999;

        }
        public struct ExtensionesTec
        {
            public const string TEC = "tec.ac.cr";
            public const string ITCR = "itcr.ac.cr";
            public const string ESTUDIANTEC = "estudiantec.cr";
        }

        public struct TipoUsuario
        {
            public const string FUNCIONARIO = "Funcionario";
            public const string ESTUDIANTE = "Estudiante";
            public const string AMBOS = "Ambos";
        }

        public struct CodigosBitacora
        {
            public const int CONSULTAR = 1;
            public const int INSERTAR = 2;
            public const int MODIFICAR = 3;
            public const int ELIMINAR = 4;
            public const int ACTIVAR = 5;
            public const int INACTIVAR = 6;
        }

        public struct EquipoEspecial
        {
            public const int CONNECT = 1;
            public const int MEETUP = 2;
            public const int GROUP = 3;
        }
    }

}
