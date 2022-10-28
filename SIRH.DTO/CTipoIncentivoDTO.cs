using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CTipoIncentivoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Tipo de Incentivo")]
        public string DesIncentivo { get; set; }
    }
}
