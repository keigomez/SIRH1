using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
    [DataContract]
    public class CPedimentoPuestoDTO : CBaseDTO
    {
        [DataMember]
        [Required]
        [DisplayName("N° de Pedimento")]
        public string NumeroPedimento { get; set; }
        [DataMember]
        [DisplayName("Fecha del pedimento")]
        public DateTime FechaPedimento { get; set; }
        [DataMember]
        [Required]
        [DisplayName("Observaciones del Pedimento")]
        public string ObservacionesPedimento { get; set; }
        [DataMember]
        public CEstadoPedimentoDTO EstadoPedimento { get; set; }
    }
}
