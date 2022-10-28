using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
    [DataContract]
    public class CCaucionDTO : CBaseDTO
    {
        [DataMember]
        [Required]
        //[ReadOnly(true)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Fecha rige")]
        public DateTime FechaEmision { get; set; }
        [DataMember]
        [Required]
        /*[ReadOnly(true)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]*/
        [DisplayName("Fecha vence")]
        public DateTime FechaVence { get; set; }
        [DataMember]
        [Required]
        [DisplayName("Número de póliza")]
        public string NumeroPoliza { get; set; }
        [DataMember]
        public int EstadoPoliza { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public string DetalleEstadoPoliza { get; set; }
        [DataMember]
        [DisplayName("Justificación de anulación")]
        public string ObservacionesPoliza { get; set; }

        [DataMember]
        [Required]
        [DisplayName("N° Oficio de entrega")]
        public string NumeroOficioEntrega { get; set; }

        [DataMember]
        [DisplayName("¿Entrega copia certificada?")]
        public bool CopiaCertificada { get; set; }
    }
}
