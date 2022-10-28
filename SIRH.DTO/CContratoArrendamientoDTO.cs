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
    public class CContratoArrendamientoDTO:CBaseDTO
    {
        [DataMember]
        [DisplayName("Código de Contrato")]
        public string CodigoContratoArrendamiento { get; set; }
        [DataMember]
        [DisplayName("Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }  
        [DataMember]
        [DisplayName("Fecha Fin")]
        public DateTime FechaFin { get; set; }
        [DataMember]
        [StringLength(50)]
        [DisplayName("Emisor del Contrato")]
        public string EmisorContrato { get; set; }
        [DataMember]
        [DisplayName("Monto del Contrato")]
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal MontoContrato { set; get; }
        [DataMember]
        public CDesarraigoDTO Desarraigo { set; get; }
        public CViaticoCorridoDTO ViaticoCorrido { set; get; }


    }
}
