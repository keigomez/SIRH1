using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CDepartamentoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Departamento")]
        public string NomDepartamento { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public int IndEstDepartamento { get; set; }        
    }
}

