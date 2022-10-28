using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CRescisionDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("N° de Rescisión")]
        public string NumeroRescision { get; set; }
        [DataMember]
        [DisplayName("Fecha de Rescisión")]
        public DateTime FechaRescision { get; set; }
        [DataMember]
        [DisplayName("N° Oficio")]
        public string NumeroOficio { get; set; }
        [DataMember]
        [DisplayName("Fecha de Oficio")]
        public DateTime FechaDeOficio { get; set; }
        [DataMember]
        public CPrestamoPuestoDTO PrestamoPuesto { get; set; }
    }
}
