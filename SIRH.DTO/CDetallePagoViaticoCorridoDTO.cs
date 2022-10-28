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
    public class CDetallePagoViaticoCorridoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("[FK_PagoViaticoCorrido]")]
        public CPagoViaticoCorridoDTO PagoDTO { get; set; }

        [DataMember]
        [DisplayName("[FK_TipoDetallePago]")]
        public CTipoDetallePagoViaticoDTO TipoDetalleDTO { get; set; }

        [DataMember]
        [DisplayName("Día Pago")]
        public string FecDiaPago { get; set; }

        [DataMember]
        [DisplayName("Monto Pago")]
        public decimal MonPago { get; set; }

        [DataMember]
        [DisplayName("Cod Entidad")]
        public int CodEntidad { get; set; }
    }
}
