using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CMotivoBajaDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        [DataMember]
        [DisplayName("Observaciones")]
        public string Observaciones { get; set; }

    }
}
