using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CFactorDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Factor")]
        public string DescripcionFactor{get; set;}
        [DataMember]       
        public CTituloFactorDTO TituloFactor { get; set; }
    }
}
