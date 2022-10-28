using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CEstadoPuestoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Estado de Puesto")]
        public string DesEstadoPuesto { get; set; }
    }
}
