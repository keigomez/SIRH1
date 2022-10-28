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
    public class CDetalleDesarraigoEliminacionDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("FK_Desarraigo")]
        public CDesarraigoDTO Desarraigo { get; set; }
        [DataMember]
        public CEstadoDesarraigoDTO EstadoDesarraigo { get; set; }
        [DataMember]
        [DisplayName("Observaciones")]
        public string ObsEliminacion { get; set; }
        [DataMember]
        [DisplayName("Fecha Eliminación")]
        public DateTime FecEliminacion { get; set; }
        [DataMember]
        [DisplayName("Fecha Final")]
        public DateTime FecRegistro { get; set; }
    }
}

