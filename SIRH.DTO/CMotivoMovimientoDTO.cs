using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CMotivoMovimientoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Motivo Movimiento")]
        public string DesMotivo { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public int IndEstMotivoMovimiento { get; set; }
    }
}
