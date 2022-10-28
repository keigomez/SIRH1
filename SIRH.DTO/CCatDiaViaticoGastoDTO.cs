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
    public class CCatDiaViaticoGastoDTO : CBaseDTO
    {
        [DataMember]
        public CTipoDiaDTO TipoDia { get; set; }
        [DataMember]
        [DisplayName("Descripción")]
        [Required(ErrorMessage = "La descripción del día es requerida.")]
        public string DescripcionDia { get; set; }
      
        [DataMember]
        [DisplayName("Día")]
        public DateTime Dia { get; set; }

        [DataMember]
        [DisplayName("Rebajo")]
        public bool IndRebajo { get; set; }

        [DataMember]
        [DisplayName("Aplica Viático")]
        public bool IndViatico { get; set; }

        [DataMember]
        [DisplayName("Aplica Gasto")]
        public bool IndGasto { get; set; }

    }
}
