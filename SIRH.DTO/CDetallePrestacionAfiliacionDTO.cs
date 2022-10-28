using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CDetallePrestacionAfiliacionDTO : CBaseDTO
   {
        
        [DataMember]
        public CPrestacionLegalDTO Prestacion { get; set; }

        [DataMember]
        [DisplayName("Afiliado a:")]
        public string DesAfiliacion { get; set; }

        [DataMember]
        [DisplayName("Monto")]
        public decimal MtoAfiliacion { get; set; }
       
    }
}