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
    public class CCatalogoDiaDTO : CBaseDTO
    {
        [DataMember]
        public CTipoDiaDTO TipoDia { get; set; }
        [DataMember]
        [DisplayName("Descripción")]
        [Required(ErrorMessage = "La descripción del día es requerida.")]
        public string DescripcionDia { get; set; }
        [DataMember]
        [DisplayName("Mes")]
        public string Mes { get; set; }
        [DataMember]
        [DisplayName("Día")]
        public string Dia { get; set; }
    }
}
