using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CTipoExtraDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Tipo de Extra")]
        public string DesTipExtra { get; set; }
    }
}
