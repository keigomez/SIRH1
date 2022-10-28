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
    public class CDetallePuestoRubroDTO : CBaseDTO
   {
        [DataMember]
        public CComponenteSalarialDTO Componente { get; set; }
        [DataMember]
        public CDetallePuestoDTO DetallePuesto { get; set; }
        
        [DataMember]
        [DisplayName("Porc. Valor")]
        public decimal PorValor { get; set; }


        // Monto es el equivalente al PorValor por el Salario Base
        [DataMember]
        [DisplayName("Monto")]
        public decimal MtoValor { get; set; }

        [DataMember]
        [DisplayName("Fecha Rige")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecRige { get; set; }

        [DataMember]
        [DisplayName("Fecha Vence")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FecVence { get; set; }

    }
}