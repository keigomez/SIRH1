using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CCaracteristicasPuestoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("N° Escala")]
        public int NumEscala { get; set; }
        [DataMember]
        [DisplayName("Caracteristicas")]
        public string Caracteristicas { get; set; }
        [DataMember]
        public CFactorDTO Factor { get; set; }
        [DataMember]
        public CPuestoDTO Puesto { get; set; }
    }
}
