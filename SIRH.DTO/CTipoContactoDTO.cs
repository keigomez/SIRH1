using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CTipoContactoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Tipo de Contacto")]
        public string DesTipoContacto { get; set; }
    }
}
