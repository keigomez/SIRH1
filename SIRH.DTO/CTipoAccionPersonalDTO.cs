using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CTipoAccionPersonalDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Tipo de Acción")]
        public string DesTipoAccion { get; set; }

        [DataMember]
        public int IndCategoria { get; set; }
    }
}