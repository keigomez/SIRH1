using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CResolucionPedimentoDTO : CBaseDTO
    {
        [DataMember]
        [Required]
        [DisplayName("N° de Resolución")]
        public string NumeroResolucion { get; set; }
        [DataMember]
        [Required]
        [DisplayName("Fecha de resolución")]
        public DateTime FechaResolucion { get; set; }
        [DataMember]
        [Required]
        [DisplayName("Observaciones")]
        public string DetalleResolucion { get; set; }
        [DataMember]
        public CTipoResolucionPedimentoDTO TipoResolucion { get; set; }
    }
}
