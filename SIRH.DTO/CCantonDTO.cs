using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CCantonDTO : CBaseDTO
    {
        [DataMember]
        public CProvinciaDTO Provincia { get; set; }
        [DataMember] 
        [DisplayName("Cantón")]
        public string NomCanton { get; set; }
        [DataMember]
        [DisplayName("Código Postal")]
        public string CodPostalCanton { get; set; }
    }
}
