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
    public class CUbicacionPuestoDTO : CBaseDTO
    {
        [DataMember]
        public CTipoUbicacionDTO TipoUbicacion { get; set; }
        [DataMember]
        public CDistritoDTO Distrito { get; set; }
        [DataMember]
        //[Required(ErrorMessage = "Debe indicar la justificación o motivo que sustenta el cambio de ubicación de este puesto")]
        [DisplayName("Observaciones")]
        public string ObsUbicacionPuesto { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public string EstadoUbicacionPuesto { get; set; }
        [DataMember]
        [DisplayName("Fecha actualizacion")]
        public DateTime FechaActualizacion { get; set; }
    }
}
