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
    public class CDetalleEliminacionViaticoCorridoGastoTransporteDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("FK_MovimientoViaticoCorrido")]
        public CMovimientoViaticoCorridoDTO MovimientoViaticoCorridoDTO { get; set; }
        [DataMember]
        [DisplayName("FK_MovimientoGastoTransporte")]
        public CMovimientoGastoTransporteDTO MovimientoGastoTransporteDTO { get; set; }
        [DataMember]
        [DisplayName("Justificacion")]
        public string ObsJustificacionDTO { get; set; }
        [DataMember]
        [DisplayName("Fecha Inicio")]
        public DateTime FecInicioDTO { get; set; }
        [DataMember]
        [DisplayName("Fecha Final")]
        public DateTime FecFinDTO { get; set; }
    }
}

