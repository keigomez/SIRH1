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
    public class CComponenteSalarialDTO : CBaseDTO
    {
        [DataMember]
        public CObjetoGastoDTO ObjetoGasto { get; set; }
        [DataMember]
        [DisplayName("Descipción del Componente Salarial")]
        public string DesComponenteSalarial { get; set; }
        [DataMember]
        [DisplayName("Tipo de Componente")]
        public int TipComponenteSalarial { get; set; }
    }
}
