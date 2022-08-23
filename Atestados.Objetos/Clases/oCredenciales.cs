using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos.Clases
{

    [DataContract]
    [Serializable]
    public class oCredenciales
    {
        [DataMember]
        public int CodigoAplicacionLogueado { get; set; }
        [DataMember]
        public int IdUsuarioLogueado { get; set; }
        [DataMember]
        public string UsuarioLogueado { get; set; }
        [DataMember]
        public string UsuarioAplicacionLogueado { get; set; }
        [DataMember]
        public string ContrasenaUsuarioAplicacionLogueado { get; set; }
        [DataMember]
        public string IpAddress { get; set; }
        [DataMember]
        public string MacAddress { get; set; }
        [DataMember]
        public string SessionId { get; set; }
    }

}
