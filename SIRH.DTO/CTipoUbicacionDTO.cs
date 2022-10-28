using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CTipoUbicacionDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Tipo de Ubicación")]
        public string DesTipoUbicacion { get; set; }
    }
}
