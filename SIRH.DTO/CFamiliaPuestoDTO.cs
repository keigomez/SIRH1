using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CFamiliaPuestoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Descripción")]
        public string DesFamilia { get; set; }
        [DataMember]
        [DisplayName("Observaciones")]
        public string DesObservaciones { get; set; }
    }
}
