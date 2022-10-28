using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CDetalleCalificacionSeguimientoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("FK_CalificacionNombramientoFuncionario")]
        public CCalificacionNombramientoFuncionarioDTO Calificacion { get; set; }

        [DataMember]
        [DisplayName("Estado")]
        public CCatMetaEstadoDTO Estado { get; set; }

        [DataMember]
        [DisplayName("JefeInmediato")]
        public CFuncionarioDTO JefeInmediato { get; set; }

        [DataMember]
        [DisplayName("Fecha Registro")]
        public DateTime FecRegistro { get; set; }

        [DataMember]
        [DisplayName("Oportunidad de Mejora y/o Refuerzo Positivo")]
        public string DesOportunidadMejora { get; set; }

        [DataMember]
        [DisplayName("Plan de Acción")]
        public string DesPlanAccion { get; set; }

        [DataMember]
        [DisplayName("Fecha Cierre Acción")]
        public DateTime FecCierreAccion { get; set; }

        [DataMember]
        [DisplayName("Evidencia")]
        public string DesEvidencia { get; set; }

        [DataMember]
        [DisplayName("Comentario")]
        public string DesComentario { get; set; }
    }
}

