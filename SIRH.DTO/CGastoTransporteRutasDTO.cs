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
    public class CGastoTransporteRutasDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("FK_GastoTransporte")]
        public CGastoTransporteDTO Gasto { get; set; }

        [DataMember]
        [DisplayName("FK_Estado")]
        public CEstadoGastoTransporteDTO Estado { get; set; }

        [DataMember]
        [DisplayName("FecRige")]
        public DateTime FecRige { get; set; }

        [DataMember]
        [DisplayName("FecVence")]
        public DateTime FecVence { get; set; }

        [DataMember]
        [DisplayName("MonDiario")]
        public decimal MonDiario { get; set; }

        [DataMember]
        public List<CDetalleAsignacionGastoTransporteDTO> Detalle { get; set; }
    }
}
