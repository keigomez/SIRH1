using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CSeccionDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Sección")]
        public string NomSeccion { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public int IndEstSeccion { get; set; }
    }
}
