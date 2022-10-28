using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CDivisionDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("División")]
        public string NomDivision { get; set; }
    }
}
