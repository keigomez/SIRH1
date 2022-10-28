using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
    [DataContract(IsReference = true)]
    public class CDetalleIncapacidadDTO : CBaseDTO
    {
        [DataMember]
        public CRegistroIncapacidadDTO Incapacidad { get; set; }

        [DataMember]
        [DisplayName("Fecha Rige")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public string FecRige { get; set; }
        
        [DataMember]
        [DisplayName("Por Subsidio")]
        public decimal PorSubsidio{ get; set; }
        
        [DataMember]
        [DisplayName("Por Rebaja")]
        public decimal PorRebaja { get; set; }

        [DataMember]
        [DisplayName("Salario por Dia")]
        public decimal MtoSalarioDia { get; set; }

        [DataMember]
        [DisplayName("Subsidio")]
        public decimal MtoSubsidio { get; set; }


        [DataMember]
        [DisplayName("Rebaja")]
        public decimal MtoRebaja { get; set; }

        public double NumDia { get; set; }
    }
}