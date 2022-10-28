using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CMetaIndividualInformeDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Meta")]
        public CMetaIndividualCalificacionDTO Meta { get; set; }

        [DataMember]
        [DisplayName("Fecha")]
        public DateTime FecMes { get; set; }

        [DataMember]
        [DisplayName("Resultado")]
        public decimal NumIndicador { get; set; }

        [DataMember]
        [DisplayName("Resultado Producción")]
        public decimal NumResultadoProduccion { get; set; }

        [DataMember]
        [DisplayName("Comentario")]
        public string DesInforme { get; set; }

        [DataMember]
        [DisplayName("IndCompleto")]
        public int IndCompleto { get; set; }

        [DataMember]
        [DisplayName("IndEstado")]
        public int IndEstado { get; set; }

        [DataMember]
        [DisplayName("Observaciones")]
        public string Observaciones { get; set; }
    }
}