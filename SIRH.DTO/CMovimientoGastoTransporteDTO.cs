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
    public class CMovimientoGastoTransporteDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("[FK_GastoTransporte]")]
        public CGastoTransporteDTO GastoTransporteDTO { get; set; }

        //No existe este campo en la BD para gasto de transporte
        [DataMember]
        [DisplayName("Fecha")]
        public DateTime FecMovimientoDTO { get; set; }

        [DataMember]
        [DisplayName("Nombre Tipo")]
        public int Nomtipo { get; set; }
        [DataMember]
        [DisplayName("Observaciones")]
        public string ObsObservacionesDTO { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public int EstadoDTO { get; set; }

        [DataMember]
        public List<CDetalleDeduccionGastoTransporteDTO> Deducciones { get; set; }
    }
}
