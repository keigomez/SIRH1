using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SIRH.DTO
{
    [DataContract]
    public class CErrorDTO : CBaseDTO
    {
        [DataMember]
        public int Codigo { get; set; }
        [DataMember]
        public string Titulo { get; set; }
        [DataMember]
        public string MensajeError { get; set; }
    }
}
