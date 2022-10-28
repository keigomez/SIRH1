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
    public class CRegistroIncapacidadDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Salario Bruto")]
        public decimal MtoSalario { get; set; }
        [DataMember]
        [DisplayName("Fecha Rige")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecRige { get; set; }
        [DataMember]
        [DisplayName("Fecha Vence")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecVence { get; set; }
        [DataMember]
        public CTipoIncapacidadDTO TipoIncapacidad { get; set; }
        [DataMember]
        public CNombramientoDTO Nombramiento { get; set; }
        [DataMember]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observaciones")]
        public string ObsIncapacidad { get; set; }
        [DataMember]
        [DisplayName("Número Incapacidad")]
        public string CodIncapacidad { get; set; }
        [DisplayName("Número Caso")]
        public string CodNumeroCaso { get; set; }

        [DataMember]
        public int EstadoIncapacidad { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public string DetalleEstadoIncapacidad { get; set; }

        [DataMember]
        public List<CDetalleIncapacidadDTO> Detalles { get; set; }

        [DataMember]
        [DisplayName("Total Subsidio")]
        public decimal MontoTotalSubsidio { get; set; }

        [DataMember]
        [DisplayName("Total Rebaja")]
        public decimal MontoTotalRebaja { get; set; }

        [DataMember]
        public int IndProrroga { get; set; }

        [DataMember]
        public double NumDiasOrigen { get; set; }

        [DataMember]
        public double NumDiasTotal { get; set; }

    }
}