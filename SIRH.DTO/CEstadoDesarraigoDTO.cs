using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CEstadoDesarraigoDTO:CBaseDTO
    {

        [DataMember]
        [DisplayName("Estado del Desarraigo")]
        public string NomEstadoDesarraigo { set; get; }
        
    }
}
