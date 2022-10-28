using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CCatEstadoCivilDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Estado Civil")]
        public string DesEstadoCivil { get; set; }
        
    }
}
