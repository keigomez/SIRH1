using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CUbicacionRealDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Ubicación Real")]
        public string NomUbicacionReal { get; set; }

    }
}
