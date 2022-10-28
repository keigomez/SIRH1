using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CDesgloseSalarialDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Período")]
        public string Periodo { get; set; }
        [DataMember]
        [DisplayName("Fecha Carga")]
        public DateTime FecCarga { get; set; }
        //[DataMember]
        //[DisplayName("Monto Total")]
        //public decimal MontoTotal { get; set; }
        [DataMember]
        [DisplayName("Monto salario bruto")]
        public decimal MontoSalarioBruto { get; set; }
        //[DataMember]
        //[DisplayName("Monto salario neto")]
        //public decimal MontoSalarioNeto { get; set; }
        [DataMember]
        public CNombramientoDTO Nombramiento { get; set; }

    }
}
