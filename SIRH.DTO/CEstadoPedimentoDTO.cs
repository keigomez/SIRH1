using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CEstadoPedimentoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Estado de pedimento")]
        public string DetalleEstado { get; set; }
    }
}
