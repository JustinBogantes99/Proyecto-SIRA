using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos.Clases
{
    [DataContract]
    public  class oRespuesta<TipoGenerico>
    {
        [DataMember]
        public int CodigoRespuesta { get; set; }
        [DataMember]
        public string MensajeRespuesta { get; set; }

        [DataMember]
        public TipoGenerico ObjetoRespuesta { get; set; }

        [DataMember]
        public bool Estado { get; set; }
    }
}
