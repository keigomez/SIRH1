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
    public class CPagoFeriadoDTO : CBaseDTO
    {
        [DataMember]
        public CPagoExtraordinarioDTO PagoExtraordinario { get; set; }
        [DataMember]
        [DisplayName("Salario bruto")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal MontoSalaroBruto { get; set; }
        [DataMember]
        [DisplayName("Sub-total de días")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal MontoSubtotalDia { get; set; }
        [DataMember]
        [DisplayName("Salario escolar")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal MontoSalarioEscolar { get; set; }
        [DataMember]
        [DisplayName("Diferencia líquida")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal MontoDiferenciaLiquida { get; set; }
        [DataMember]
        [DisplayName("Aguinaldo proporcional")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal MontoAguinaldoProporcional { get; set; }
        [DataMember]
        [DisplayName("Total de deducciones")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal MontoDeduccionPatronal { get; set; }
        [DataMember]
        [DisplayName("Total de deducciones")]
        [DisplayFormat( DataFormatString = "{0:N2}")]
        public decimal MontoDeduccionObrero { get; set; }
        [DataMember]
        [DisplayName("Total")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal MontoDeTotal { get; set; }
        [DataMember]
        public int EstadoTramite { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public string DetalleEstado{ get; set; }
        [DataMember]
        [DisplayName("Observaciones")]
        public string ObsevacionTramite{ get; set; }
    }
}
