using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CAddendumPrestamoPuestoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("N° Addendum")]
        public string NumeroAddendum { get; set; }
        [DataMember]
        [DisplayName("Fecha de Rige")]
        public DateTime FechaRige { get; set; }
        [DataMember]
        [DisplayName("Fecha que Finaliza")]
        public DateTime FechaFin { get; set; }
        [DataMember]
        public CPrestamoPuestoDTO PrestamoPuesto { get; set; }
    }
}
