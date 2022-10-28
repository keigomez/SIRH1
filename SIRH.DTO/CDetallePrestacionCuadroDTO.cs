using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CDetallePrestacionCuadroDTO : CBaseDTO
    {
        [DataMember]
        public CTipoPrestacionCuadroDTO TipoCuadro { get; set; }
        [DataMember]
        public CPrestacionLegalDTO Prestacion { get; set; }

        [DataMember]
        [DisplayName("Fecha Pago")]
        public DateTime FecPeriodo { get; set; }

        [DataMember]
        [DisplayName("Sueldo Total")]
        public decimal MtoSalario { get; set; }

        [DataMember]
        [DisplayName("Extra")]
        public decimal MtoExtra { get; set; }

        [DataMember]
        [DisplayName("Feriado")]
        public decimal MtoFeriado { get; set; }

        [DataMember]
        [DisplayName("Monto Total")]
        public decimal MtoTotal { get; set; }

        [DataMember]
        [DisplayName("Salario Escolar")]
        public decimal MtoSalarioEscolar { get; set; }

        [DataMember]
        [DisplayName("Periodo Vacaciones")]
        public string PeriodoVacaciones { get; set; }

        [DataMember]
        [DisplayName("Saldo Vacaciones")]
        public decimal NumSaldoVacaciones { get; set; }

        [DataMember]
        [DisplayName("Monto Vacaciones")]
        public decimal MtoSaldoVacaciones { get; set; }
    }
}