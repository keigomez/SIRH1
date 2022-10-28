using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CCartaPresentacionDTO:CBaseDTO
    {
        [DataMember]
        [DisplayName("N° de Carta")]
        public string NumeroCarta { get; set; }
        [DataMember]
        [DisplayName("Fecha de Carta")]
        public DateTime FechaCarta { get; set; }
        [DataMember]
        [DisplayName("Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }
        [DataMember]
        [DisplayName("Fecha de Vencimiento")]
        public DateTime FechaVencimiento { get; set; }
        [DataMember]
        public CNombramientoDTO Nombramiento { get; set; }

    }
}
