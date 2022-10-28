using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CDistritoDTO : CBaseDTO
    {
        [DataMember]
        public CCantonDTO Canton { get; set; }
        [DataMember]
        [DisplayName("Distrito")]
        public string NomDistrito { get; set; }
        [DataMember]
        [DisplayName("Codigo Postal")]
        public string CodPostalDistrito { get; set; }

    }
}
