using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CCalificacionDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Calificación")]
        public string DesCalificacion { get; set; }
    }
}
