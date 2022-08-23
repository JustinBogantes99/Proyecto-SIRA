using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Atestados.Objetos;
using Atestados.Objetos.Clases;

namespace Atestados.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServiciosAtestados" in both code and config file together.
    [ServiceContract]
    public interface IServiciosAtestados
    {
        #region Configuración
        [OperationContract]
        oRespuesta<List<Objetos.Clases.Configuracion>> ConsultarConfiguracion(int? idConfiguracion);
        #endregion

    }
}
