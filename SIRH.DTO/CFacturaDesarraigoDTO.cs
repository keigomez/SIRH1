using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CFacturaDesarraigoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Código de Factura")]
        public string CodigoFactura { get; set; }
        [DataMember]
        [DisplayName("Fecha de la Facturación")]
        public DateTime FechaFacturacion { get; set; }
        [DataMember]
        [StringLength(50)]
        [DisplayName("Emisor de la Factura")]
        public string Emisor { get; set; }
        [DataMember]
        [DisplayName("Monto de la Factura")]
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal MontoFactura { get; set; }
        [DataMember]
        public CDesarraigoDTO Desarraigo { get; set; }
        public CViaticoCorridoDTO ViaticoCorrido { get; set; }
        [DataMember]
        [DisplayName("Concepto")]
        public String ObsConcepto { get; set; }
    }
}
