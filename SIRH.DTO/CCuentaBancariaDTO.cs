using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CCuentaBancariaDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("N° Cuenta Cliente")]
        public string CtaCliente { get; set; }
        [DataMember]
        public CEntidadFinancieraDTO EntidadFinanciera { get; set; }
        [DataMember]
        public CDetalleContratacionDTO DetalleContratacion { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public int IndEstCuentaBancaria { get; set; }
    }
}

