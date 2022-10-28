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
    public class CMovimientoViaticoCorridoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("[FK_ViaticoCorrido]")]
        public CViaticoCorridoDTO ViaticoCorridoDTO { get; set; }

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
        public List<CDetalleDeduccionViaticoCorridoDTO> Deducciones { get; set; }
    }
}
