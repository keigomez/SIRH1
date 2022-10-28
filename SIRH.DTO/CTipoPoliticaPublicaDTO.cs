using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CTipoPoliticaPublicaDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Tipo de Política Pública")]
        public string DescripcionTipoPP { get; set; }

        [DataMember]
        [DisplayName("Tipo de Política Pública")]
        public string SiglaTipoPP { get; set; }
    }
}
