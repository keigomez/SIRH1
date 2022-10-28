using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CTareasPuestoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("¿Que hace?")]
        public string DescripcionQueHace { get; set; }
        [DataMember]
        [DisplayName("¿Cómo lo hace?")]
        public string DescripcionComoHace { get; set; }
        [DataMember]
        [DisplayName("Frecuencia")]
        public int NumFrecuencia { get; set; }
        [DataMember]
        public CPuestoDTO Puesto { get; set; }
    }
}
