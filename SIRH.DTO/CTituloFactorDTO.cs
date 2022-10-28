using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CTituloFactorDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Título")]
        public string DescripcionTitulo { get; set; }              
        
    }
}
