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
    public class CDetalleEvaluacionDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("FK_CatalogoPregunta")]
        public CCatalogoPreguntaDTO CatalogoPreguntaDTO { get; set; }
        [DataMember]
        [DisplayName("FK_DetalleCalificacionNombramiento")]
        public CDetalleCalificacionNombramientoDTO DetalleCalificacionNombramientoDTO { get; set; }

        [DataMember]
        [DisplayName("NumNotasPregunta")]
        public string NumNotasPorPreguntaDTO { get; set; }

    }
}
