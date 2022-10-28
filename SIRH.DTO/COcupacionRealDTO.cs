using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class COcupacionRealDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Ocupación")]
        public string DesOcupacionReal { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public int IndEstOcupacion { get; set; }
    }
}
