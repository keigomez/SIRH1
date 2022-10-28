using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CEscalaSalarialDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Categoría")]
        public int CategoriaEscala { get; set; }
        [DataMember]
        [DisplayName("Salario Base")]
        public decimal SalarioBase { get; set; }
        [DataMember]
        [DisplayName("Monto anualidad")]
        public decimal MontoAumentoAnual { get; set; }
        [DataMember]
        public CPeriodoEscalaSalarialDTO Periodo { get; set; }
    }
}
