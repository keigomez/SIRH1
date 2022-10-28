using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CTipoPuestoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Tipo de Puesto")]
        public string DescripcionTipoPuesto { get; set; }
    }
}
