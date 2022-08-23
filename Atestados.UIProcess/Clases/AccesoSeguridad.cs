using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Atestados.UIProcess.WCF_Seguridad;
using Atestados.UIProcess.WCF_ServicioAtestados;


namespace Atestados.UIProcess.Clases
{
    public class AccesoSeguridad
    {
        #region Instancias


        WCF_Seguridad.TEC_IWCF_SeguridadClient WCF_Seguridad = new TEC_IWCF_SeguridadClient();
        WCF_Personal.WCFPersonaClient WCF_Personal = new WCF_Personal.WCFPersonaClient();
        //WCF_Atestados.AtestadosAPIClient WCF_Atestados = new UIProcess.WCF_Atestados.AtestadosAPIClient();
        WCF_Seguridad.TEC_Credenciales SegCredenciales = new TEC_Credenciales();
        WCF_ServicioAtestados.ServiciosAtestadosClient WCF_ServiciosAtestados = new ServiciosAtestadosClient();

        #endregion //región Instancias

        #region Métodos

        #region Seguridad

        #region Autenticación

        /// <summary>
        /// Consulta la información del usuario
        /// </summary>
        /// <param name="idUsuarioConsultar"></param>
        /// <param name="idUsuarioLogueado"></param>
        /// <param name="nombreUsuarioLogueado"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public WCF_Seguridad.TEC_MensajeOfTEC_Usuario ConsultarUsuario(int idUsuarioConsultar, string correoElectronico,
            int idUsuarioLogueado, string nombreUsuarioLogueado, string ipAddress, string sessionId)
        {
            SegCredenciales.CodigoAplicacionLogueado =
                Convert.ToInt32((ConfigurationManager.AppSettings["CodigoAplicacionLogueado"]));
            SegCredenciales.UsuarioAplicacionLogueado = ConfigurationManager.AppSettings["UsuarioAplicacionLogueado"];
            SegCredenciales.ContrasenaUsuarioAplicacionLogueado =
                ConfigurationManager.AppSettings["ContrasenaUsuarioAplicacionLogueado"];
            SegCredenciales.IdUsuarioLogueado = idUsuarioLogueado;
            SegCredenciales.UsuarioLogueado = nombreUsuarioLogueado;
            SegCredenciales.IpAddress = ipAddress;
            SegCredenciales.SessionId = sessionId;
            return WCF_Seguridad.Usu_ConsultarUsuario(idUsuarioConsultar, correoElectronico, SegCredenciales);
        }

        /// <summary>
        /// Realiza la autenticacion del usuario
        /// </summary>
        /// <param name="correoElectronico"></param>
        /// <param name="contrasena"></param>
        /// <param name="idUsuarioLogueado"></param>
        /// <param name="nombreUsuarioLogueado"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public WCF_Seguridad.TEC_MensajeOfTEC_Usuario AutenticarUsuario(string correoElectronico, string contrasena,
            int idUsuarioLogueado, string nombreUsuarioLogueado, string ipAddress, string sessionId)
        {
            SegCredenciales.CodigoAplicacionLogueado =
                Convert.ToInt32((ConfigurationManager.AppSettings["CodigoAplicacionLogueado"]));
            SegCredenciales.UsuarioAplicacionLogueado = ConfigurationManager.AppSettings["UsuarioAplicacionLogueado"];
            SegCredenciales.ContrasenaUsuarioAplicacionLogueado =
                ConfigurationManager.AppSettings["ContrasenaUsuarioAplicacionLogueado"];
            SegCredenciales.IdUsuarioLogueado = idUsuarioLogueado;
            SegCredenciales.UsuarioLogueado = nombreUsuarioLogueado;
            SegCredenciales.IpAddress = ipAddress;
            SegCredenciales.SessionId = sessionId;
            return WCF_Seguridad.Usu_AutenticarUsuario(correoElectronico, contrasena, SegCredenciales);
        }

        public WCF_Seguridad.TEC_MensajeOfListOfTEC_Permiso ConsultarPermisosUsuario(int idRol, int idUsuario, int idPadre, int idPermiso, int idUsuarioLogueado, string usuarioLogueado, string ipAddress, string sessionId)
        {
            SegCredenciales.CodigoAplicacionLogueado = Convert.ToInt32((ConfigurationManager.AppSettings["CodigoAplicacionLogueado"]));
            SegCredenciales.UsuarioAplicacionLogueado = ConfigurationManager.AppSettings["UsuarioAplicacionLogueado"];
            SegCredenciales.ContrasenaUsuarioAplicacionLogueado = ConfigurationManager.AppSettings["ContrasenaUsuarioAplicacionLogueado"];
            SegCredenciales.IpAddress = ipAddress;
            SegCredenciales.IdUsuarioLogueado = idUsuarioLogueado;
            SegCredenciales.UsuarioLogueado = usuarioLogueado;
            SegCredenciales.SessionId = sessionId;
            return WCF_Seguridad.Usu_ConsultarPermisosUsuario(SegCredenciales.CodigoAplicacionLogueado, idRol, idUsuario, idPadre, idPermiso, SegCredenciales);
        }


        public WCF_Seguridad.TEC_MensajeOfListOfTEC_Rol ConsultarRolesUsuario(int idUsuario, int idUsuarioLogueado, string usuarioLogueado, string ipAddress, string sessionId)
        {
            SegCredenciales.CodigoAplicacionLogueado = Convert.ToInt32((ConfigurationManager.AppSettings["CodigoAplicacionLogueado"]));
            SegCredenciales.UsuarioAplicacionLogueado = ConfigurationManager.AppSettings["UsuarioAplicacionLogueado"];
            SegCredenciales.ContrasenaUsuarioAplicacionLogueado = ConfigurationManager.AppSettings["ContrasenaUsuarioAplicacionLogueado"];
            SegCredenciales.IpAddress = ipAddress;
            SegCredenciales.IdUsuarioLogueado = idUsuarioLogueado;
            SegCredenciales.UsuarioLogueado = usuarioLogueado;
            SegCredenciales.SessionId = sessionId;
            return WCF_Seguridad.Usu_ConsultarRolesUsuario(SegCredenciales.CodigoAplicacionLogueado, idUsuario,  SegCredenciales);
        }

        #endregion //región Autenticación

        #region Bitacoras
        #region Sistema
        /// <summary>
        /// Consulta de los movimientos realizados en el sistema
        /// </summary>
        /// <param name="idTipoMovimiento"></param>
        /// <param name="idAplicacion"></param>
        /// <param name="usuarioCreacion"></param>
        /// <param name="esError"></param>
        /// <param name="ipOrigen"></param>
        /// <param name="fechaCreacion"></param>
        /// <param name="idUsuarioLogueado"></param>
        /// <param name="usuarioLogueado"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public WCF_Seguridad.TEC_MensajeOfListOfTEC_Bitacora ConsultarBitacoraGeneral(int idBitacora, int idTipoMovimiento, int idAplicacion, string usuarioCreacion, bool? esError, string ipOrigen, DateTime? fechaCreacion, int idUsuarioLogueado, string usuarioLogueado, string ipAddress, string sessionId, DateTime? fechaFin)
        {
            SegCredenciales.CodigoAplicacionLogueado = Convert.ToInt32((ConfigurationManager.AppSettings["CodigoAplicacionLogueado"]));
            SegCredenciales.UsuarioAplicacionLogueado = ConfigurationManager.AppSettings["UsuarioAplicacionLogueado"];
            SegCredenciales.ContrasenaUsuarioAplicacionLogueado = ConfigurationManager.AppSettings["ContrasenaUsuarioAplicacionLogueado"];
            SegCredenciales.IpAddress = ipAddress;
            SegCredenciales.IdUsuarioLogueado = idUsuarioLogueado;
            SegCredenciales.UsuarioLogueado = usuarioLogueado;
            SegCredenciales.SessionId = sessionId;
            return WCF_Seguridad.Seg_ConsultarBitacora(idBitacora, idTipoMovimiento, idAplicacion, usuarioCreacion, esError, ipOrigen, fechaCreacion, SegCredenciales, fechaFin);
        }

        //public WCF_Seguridad.TEC_MensajeOfString InsertarBitacoraSistema(string pDescripcion, int idTipoMovimiento, string pModulo, string pMetodo, bool esError, int idUsuarioLogueado, string usuarioLogueado, string ipAddress, string sessionId, string ObjXML)
        //{
        //    SegCredenciales.CodigoAplicacionLogueado = Convert.ToInt32((ConfigurationManager.AppSettings["CodigoAplicacionLogueado"]));
        //    SegCredenciales.UsuarioAplicacionLogueado = ConfigurationManager.AppSettings["UsuarioAplicacionLogueado"];
        //    SegCredenciales.ContrasenaUsuarioAplicacionLogueado = ConfigurationManager.AppSettings["ContrasenaUsuarioAplicacionLogueado"];
        //    SegCredenciales.IpAddress = ipAddress;
        //    SegCredenciales.IdUsuarioLogueado = idUsuarioLogueado;
        //    SegCredenciales.UsuarioLogueado = usuarioLogueado;
        //    SegCredenciales.SessionId = sessionId;
        //    return WCF_Seguridad.Seg_InsertarBitacora(pDescripcion, pModulo, pMetodo, idTipoMovimiento, esError, ObjXML, SegCredenciales);
        //}


        #endregion //región sistema

        #region Usuarios
        public WCF_Seguridad.TEC_MensajeOfListOfTEC_MovimientoUsuario ConsultarBitacoraUsuarios(int idMovimientoUsuario, int idTipoMovimiento, int idUsuario, string usuarioCreacion, DateTime? fechaCreacion, string idSession, string ipOrigen, string macAddress, int idUsuarioLogueado, string usuarioLogueado, string ipAddress, string sessionId, DateTime? fechaFin)
        {
            SegCredenciales.CodigoAplicacionLogueado = Convert.ToInt32((ConfigurationManager.AppSettings["CodigoAplicacionLogueado"]));
            SegCredenciales.UsuarioAplicacionLogueado = ConfigurationManager.AppSettings["UsuarioAplicacionLogueado"];
            SegCredenciales.ContrasenaUsuarioAplicacionLogueado = ConfigurationManager.AppSettings["ContrasenaUsuarioAplicacionLogueado"];
            SegCredenciales.IpAddress = ipAddress;
            SegCredenciales.IdUsuarioLogueado = idUsuarioLogueado;
            SegCredenciales.UsuarioLogueado = usuarioLogueado;
            SegCredenciales.SessionId = sessionId;
            return WCF_Seguridad.Seg_ConsultarMovUsuario(idMovimientoUsuario, idTipoMovimiento, idUsuario, usuarioCreacion, fechaCreacion, idSession, ipOrigen, macAddress, SegCredenciales, fechaFin);
        }

        public WCF_Seguridad.TEC_MensajeOfTEC_RespuestaUsuario UsuariosDisponibles(int IdUsuario, int IdTipoUsuario, string CorreoElectronico, bool Estado, bool UsuarioBloqueado, string ipAddress, string sessionId, int idUsuarioLogueado, string usuarioLogueado)
        {
            SegCredenciales.CodigoAplicacionLogueado = Convert.ToInt32((ConfigurationManager.AppSettings["CodigoAplicacionLogueado"]));
            SegCredenciales.UsuarioAplicacionLogueado = ConfigurationManager.AppSettings["UsuarioAplicacionLogueado"];
            SegCredenciales.ContrasenaUsuarioAplicacionLogueado = ConfigurationManager.AppSettings["ContrasenaUsuarioAplicacionLogueado"];
            SegCredenciales.IpAddress = ipAddress;
            SegCredenciales.IdUsuarioLogueado = idUsuarioLogueado;
            SegCredenciales.UsuarioLogueado = usuarioLogueado;
            SegCredenciales.SessionId = sessionId;

            return WCF_Seguridad.Usu_ListaUsuarios(IdUsuario, IdTipoUsuario, CorreoElectronico, Estado, UsuarioBloqueado, SegCredenciales);
        }

        #endregion //región usuarios
        #endregion //región bitácoras

        #endregion //región seguridad


        #region Atestados

        public object ObtenerConfiguracion(int? idConfiguracion)
        {
            return WCF_ServiciosAtestados.ConsultarConfiguracion(idConfiguracion);
        }

        #endregion


        #region Personal
        public WCF_Personal.TEC_MensajeOfTEC_RPersona ConsultarIdPersona(int idUsuarioSeguridad)
        {
            return WCF_Personal.AutenticarSeguridad(null, idUsuarioSeguridad, null, null, 1);
        }

        #endregion

        #endregion //región métodos

    }
}
