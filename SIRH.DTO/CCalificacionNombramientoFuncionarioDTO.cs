using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CCalificacionNombramientoFuncionarioDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Periodo")]
        public CPeriodoCalificacionDTO Periodo { get; set; }

        [DataMember]
        [DisplayName("FK_Nombramiento")]
        public CNombramientoDTO Nombramiento { get; set; }

        [DataMember]
        [DisplayName("FK_Puesto")]
        public CPuestoDTO Puesto { get; set; }

        [DataMember]
        [DisplayName("FK_Funcionario")]
        public CFuncionarioDTO Funcionario { get; set; }

        [DataMember]
        [DisplayName("FecRige")]
        public DateTime FecRige { get; set; }

        [DataMember]
        [DisplayName("FecRige")]
        public DateTime FecVence { get; set; }

        [DataMember]
        [DisplayName("IndSeccion")]
        public int indSeccion { get; set; }

        [DataMember]
        [DisplayName("IndDepartamento")]
        public int indDepartamento { get; set; }

        [DataMember]
        [DisplayName("IndDireccion")]
        public int indDireccion { get; set; }

        [DataMember]
        [DisplayName("IndDivision")]
        public int indDivision { get; set; }

        [DataMember]
        [DisplayName("IndOcupacion")]
        public int indOcupacion { get; set; }

        [DataMember]
        [DisplayName("NumTipo")]
        public int NumTipo { get; set; }

        [DataMember]
        [DisplayName("AutoEvaluación")]
        public decimal PorAutoEvaluacion { get; set; }

        [DataMember]
        [DisplayName("DetallePuesto")]
        public int indDetallePuesto { get; set; }

        [DataMember]
        [DisplayName("Jefe Inmediato")]
        public CFuncionarioDTO JefeInmediato { get; set; }

        [DataMember]
        [DisplayName("Jefe Superior")]
        public CFuncionarioDTO JefeSuperior { get; set; }


        [DataMember]
        [DisplayName("Observaciones")]
        public List<CCalificacionObservacionesDTO> DetalleObservaciones { get; set; }

        [DataMember]
        [DisplayName("Seguimiento")]
        public List<CDetalleCalificacionSeguimientoDTO> DetalleSeguimientos { get; set; }

        [DataMember]
        [DisplayName("Necesidades de Capacitación")]
        public List<CCalificacionCapacitacionDTO> DetalleCapacitacion { get; set; }
    }
}