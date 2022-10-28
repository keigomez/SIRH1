using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SIRH.DTO
{
    [DataContract]
    public class CEstudioPuestoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("N° de Oficio")]
        public string NumeroOficio { get; set; }
        [DataMember]
        [DisplayName("N° de Resolución")]
        public string NumeroResolucion { get; set; }
        [DataMember]
        [DisplayName("Fecha de Resolución")]
        public DateTime FechaResolucion { get; set; }
        [DataMember]
        public CPuestoDTO Puesto { get; set; }
        [DataMember]
        [DisplayName("Estado del Puesto")]
        public string EstadoDelPuesto { get; set; }
        [DataMember]
        [DisplayName("Observaciones del estudio de puesto")]
        public string ObsDeEstudioPuesto { get; set; }
    }
}
    

