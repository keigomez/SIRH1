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
    public class CViaticoCorridoReintegroDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("[FK_ViaticoCorrido]")]
        public CViaticoCorridoDTO ViaticoCorridoDTO { get; set; }

        [DataMember]
        [DisplayName("Fecha")]
        public DateTime FecDiaDTO { get; set; }

        [DataMember]
        [DisplayName("Monto")]
        public decimal MonReintegroDTO { get; set; }
        [DataMember]
        [DisplayName("Motivo")]
        public string ObsMotivoDTO { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public int EstadoDTO { get; set; }
    }
}
