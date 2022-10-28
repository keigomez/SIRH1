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
    public class CCalificacionObservacionesDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("FK_CalificacionNombramiento")]
        public CCalificacionNombramientoFuncionarioDTO CalificacionFuncionarioDTO { get; set; }

        [DataMember]
        [DisplayName("Observaciones")]
        public string Observacion { get; set; }

        [DataMember]
        [DisplayName("Fecha Registro")]
        public DateTime FecRegistro { get; set; }

        [DataMember]
        [DisplayName("Estado")]
        public int IndEstado { get; set; }
 
    }
}
