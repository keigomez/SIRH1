using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
    [DataContract(IsReference = true)]
    public class CDeduccionTemporalDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Cantidad de días")]
        public decimal Dias { get; set; }

        [DataMember]
        [DisplayName("Cantidad de horas")]
        public decimal Horas { get; set; }

        [DataMember]
        [DisplayName("Número de documento")]
        public string NumeroDocumento { get; set; }

        [DataMember]
        [DisplayName("Explicación")]
        public string Explicacion { get; set; }

        [DataMember]
        [DisplayName("Fecha de rige")]
        public DateTime FechaRige { get; set; }

        [DataMember]
        [DisplayName("Fecha de vencimiento")]
        public DateTime? FechaVence { get; set; }

        [DataMember]
        [DisplayName("Monto deducción")]
        public decimal MontoDeduccion { get; set; }

        [DataMember]
        [DisplayName("Fecha de registro")]
        public int MesProceso { get; set; }

        [DataMember]
        [DisplayName("Periodo de aplicación (AAAAMMQQ)")]
        public string Periodo { get; set; }

        [DataMember]
        [DisplayName("Fecha actualización")]
        public DateTime FechaActualizacion { get; set; }

        [DataMember]
        [DisplayName("Estado")]
        public int Estado { get; set; }

        [DataMember]
        public CTipoDeduccionTemporalDTO DatoTipoDeduccionTemporal { get; set; }
    }
}
