using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CMetaIndividualCalificacionDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Periodo")]
        public CPeriodoCalificacionDTO Periodo { get; set; }

        [DataMember]
        [DisplayName("Funcionario")]
        public CFuncionarioDTO Funcionario { get; set; }

        [DataMember]
        [DisplayName("Prioridad")]
        public CCatMetaPrioridadDTO Prioridad { get; set; }

        [DataMember]
        [DisplayName("Estado")]
        public CCatMetaEstadoDTO Estado { get; set; }

        [DataMember]
        [DisplayName("Estado")]
        public CTipoIndicadorMetaDTO TipoIndicador { get; set; }

        [DataMember]
        [DisplayName("Meta")]
        public string DesMeta { get; set; }

        [DataMember]
        [DisplayName("Fecha Desde")]
        public DateTime FecDesde { get; set; }

        [DataMember]
        [DisplayName("Fecha Hasta")]
        public DateTime FecHasta { get; set; }

        [DataMember]
        [DisplayName("Fecha Finalizado")]
        public DateTime? FecFinalizado { get; set; }

        [DataMember]
        [DisplayName("Fecha Registro")]
        public DateTime FecRegistro { get; set; }

        [DataMember]
        [DisplayName("Indicador Mensual")]
        public string DesIndicadorMensual { get; set; }

        [DataMember]
        [DisplayName("Num. Indicador")]
        public decimal NumIndicador { get; set; }

        [DataMember]
        [DisplayName("Resultado Producción")]
        public string DesResultadoProduccion { get; set; }

        [DataMember]
        [DisplayName("EsTeletrabajable")]
        public int IndEsTeletrabajable { get; set; }

        [DataMember]
        [DisplayName("Peso")]
        public decimal PorPeso { get; set; }

        [DataMember]
        [DisplayName("Peso")]
        public decimal PorPesoNuevo { get; set; }

        [DataMember]
        [DisplayName("EsModificable")]
        public bool IndModificable { get; set; }

        [DataMember]
        [DisplayName("Observaciones")]
        public string DesObservaciones { get; set; }

        [DataMember]
        [DisplayName("MetaObjetivo")]
        public CMetaObjetivoCalificacionDTO MetaObjetivo { get; set; }

        [DataMember]
        [DisplayName("Jefe Inmediato")]
        public CFuncionarioDTO JefeInmediato { get; set; }

        [DataMember]
        [DisplayName("Evidencias")]
        public List<CMetaIndividualEvidenciaDTO> ListaEvidencias { get; set; }

        [DataMember]
        [DisplayName("Informe")]
        public List<CMetaIndividualInformeDTO> ListaInforme { get; set; }
    }
}
