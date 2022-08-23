using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Atestados.Utilitarios.Constantes;
namespace Atestados.Datos.Clases
{
    public class AdConfiguracion
    {
        #region Consultar
        /// <summary>
        /// Obtiene los participantes de una reunión
        /// </summary>
        /// <returns></returns>
        public Tuple<List<pr_ObtieneConfiguracion_Result>, int, string> ConsultarConfiguracion(int? idConfiguracion)
        {
            try
            {
                BDAtestadosMVC_ServiciosEntities db = new BDAtestadosMVC_ServiciosEntities();
                ObjectParameter codigo = new ObjectParameter("Codigo", typeof(int));
                ObjectParameter mensaje = new ObjectParameter("Mensaje", typeof(string));

                var qConsulta = db.pr_ObtieneConfiguracion(idConfiguracion, codigo, mensaje).ToList();

                //Validación de Éxito
                if (Convert.ToInt32(codigo.Value) == Constantes.Respuesta.CODIGOERROR)
                {
                    var tResultadoErroneo = new Tuple<List<pr_ObtieneConfiguracion_Result>, int, string>(null, Convert.ToInt32(codigo.Value), mensaje.Value.ToString());
                    return tResultadoErroneo;
                }
                else
                {
                    var tResultado = new Tuple<List<pr_ObtieneConfiguracion_Result>, int, string>(qConsulta, Convert.ToInt32(codigo.Value), mensaje.Value.ToString());
                    return tResultado;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodBase.GetCurrentMethod().Name.ToString(), ex);
            }
        }


        #endregion
    }
}
