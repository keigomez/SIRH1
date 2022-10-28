using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SIRH.DTO
{
    [DataContract]
    public class CHorarioRangoDTO : CBaseDTO
    {
        [DataMember]
        public byte Dia { set; get; }
        [DataMember]
        public bool Rango { set; get; }
        // ----- se mantienen mientras investigo si se ocupan los  siguientes datos -----
        [DataMember]
        public DateTime JornadaEntradaA { get; set; }
        [DataMember]
        public DateTime JornadaSalidaA { get; set; }
        [DataMember]
        public DateTime JornadaEntradaD { get; set; }
        [DataMember]
        public DateTime JornadaSalidaD { get; set; }
    }
}