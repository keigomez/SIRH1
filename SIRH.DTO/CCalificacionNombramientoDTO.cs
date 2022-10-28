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
    public class CCalificacionNombramientoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("FK_Calificacion")]
        public CCalificacionDTO CalificacionDTO { get; set; }

        [DataMember]
        [DisplayName("FK_Calificacion")]
        public CCalificacionDTO CalificacionModificadoDTO { get; set; }

        [DataMember]
        [DisplayName("FK_Nombramiento")]
        public CNombramientoDTO NombramientoDTO { get; set; }

        [DataMember]
        [DisplayName("Periodo")]
        public CPeriodoCalificacionDTO Periodo { get; set; }

        [DataMember]
        [DisplayName("UsrEvaluador")]
        public string UsrEvaluadorDTO { get; set; }

        [DataMember]
        [DisplayName("FecCreacion")]
        public DateTime FecCreacionDTO { get; set; }

        [DataMember]
        [DisplayName("IndEstado")]
        public int IndEstadoDTO { get; set; }

        [DataMember]
        [DisplayName("JUSTIFICACIONES Y OBSERVACIONES GENERALES DE LA JEFATURA")]
        public string ObsGeneralDTO { get; set; }

        [DataMember]
        [DisplayName("CAPACITACIÓN Y OTRAS MEDIDAS DE MEJORAMIENTO")]
        public string ObsCapacitacionDTO { get; set; }

        [DataMember]
        [DisplayName("JUSTIFICACIONES DE CAPACITACIÓN")]
        public string ObsJustificacionCapacitacionDTO { get; set; }

        [DataMember]
        [DisplayName("Jefe Inmediato")]
        public CFuncionarioDTO JefeInmediato { get; set; }

        [DataMember]
        [DisplayName("Jefe Superior")]
        public CFuncionarioDTO JefeSuperior { get; set; }

        [DataMember]
        [DisplayName("IndFormulario")]
        public int IndFormularioDTO { get; set; }

        [DataMember]
        [DisplayName("IndEntregado")]
        public bool IndEntregadoDTO { get; set; }

        [DataMember]
        [DisplayName("IndConformidad")]
        public bool IndConformidadDTO { get; set; }

        [DataMember]
        [DisplayName("IndRatificacion")]
        public int IndRatificacionDTO { get; set; }

        [DataMember]
        [DisplayName("FecRatificacion")]
        public DateTime? FecRatificacionDTO { get; set; }

        [DataMember]
        [DisplayName("Detalle Calificación")]
        public List<CDetalleCalificacionNombramientoDTO> DetalleCalificacion { get; set; }

        [DataMember]
        [DisplayName("Calificación Modificada")]
        public List<CDetalleCalificacionNombramientoDTO> DetalleCalificacionModificado { get; set; }
        
        [DataMember]
        [DisplayName("Necesidades de Capacitación")]
        public List<CCalificacionCapacitacionDTO> DetalleCapacitacion { get; set; }

        [DataMember]
        [DisplayName("Nota")]
        public decimal Nota { get; set; }

        [DataMember]
        public CFuncionarioDTO Funcionario { get; set; }

        [DataMember]
        public CPuestoDTO Puesto { get; set; }

        [DataMember]
        public string NombreFormulario { get; set; }

    }
}