using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CContenidoPresupuestarioDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("N° Resolución")]
        public string NumeroResolucion { get; set; }
        [DataMember]
        [DisplayName("Fecha de actualización")]
        public DateTime FechaActualizacion { get; set; }
        [DataMember]
        [DisplayName("Fecha de Rige")]
        public DateTime FechaRige { get; set; }

        [DataMember]
        [DisplayName("Fecha de vencimiento")]
        public DateTime FechaVencimiento { get; set; }
    }
}
