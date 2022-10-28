using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SIRH.DTO
{
    [DataContract]
    public class CBitacoraUsuarioDTO : CBaseDTO
    {
        [DataMember]
        public int CodigoAccion { get; set; }

        [DataMember]
        public int CodigoEntidad { get; set; }

        [DataMember]
        public int CodigoModulo { get; set; }

        [DataMember]
        public DateTime FechaEjecucion { get; set; }

        [DataMember]
        public int CodigoObjetoEntidad { get; set; }

        [DataMember]
        public string[] EntidadesAfectadas { get; set; }

        [DataMember]
        public CUsuarioDTO Usuario { get; set; }
    }
}
