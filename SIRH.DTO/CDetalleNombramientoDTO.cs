using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CDetalleNombramientoDTO : CBaseDTO
    {
        [DataMember]
        public DateTime FecCreacion { get; set; }

        [DataMember]
        [DisplayName("Observaciones")]
        public string ObservacionesTipoJornada { get; set; }
    }
}


   

        
