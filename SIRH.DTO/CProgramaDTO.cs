using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CProgramaDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Descripción del Programa")]
        public string DesPrograma { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public int IndEstPrograma { get; set; }
    }
}
