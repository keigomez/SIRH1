using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CTipoPrestacionDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Tipo de Prestacion")]
        public string DesPrestacion { get; set; }
    }
}
