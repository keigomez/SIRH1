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
    public class CDetalleCambioPedimentoDTO : CBaseDTO
    {
        [DataMember]
        [Required]
        [DisplayName("N° de Resolución de cambio")]
        public string NumeroResolucionCambio { get; set; }
        [DataMember]
        [Required]
        [DisplayName("Fecha de resolución")]
        public DateTime FechaResolucionCambio { get; set; }
        [DataMember]
        [Required]
        [DisplayName("Observaciones")]
        public string DetalleCAmbio { get; set; }
        [DataMember]
        public CTipoResolucionPedimentoDTO TipoResolucion { get; set; }
    }
}
