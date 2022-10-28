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
    public class CAccionPersonalDTO : CBaseDTO
    {
        [DataMember]
        public CEstadoBorradorDTO Estado { get; set; }

        [DataMember]
        public CTipoAccionPersonalDTO TipoAccion { get; set; }

        [DataMember]
        public CProgramaDTO Programa { get; set; }
        [DataMember]
        public CSeccionDTO Seccion { get; set; }

        [DataMember]
        public CNombramientoDTO Nombramiento { get; set; }

        [DataMember]
        [DisplayName("Número de Acción")]
        public string NumAccion { get; set; }

        [DataMember]
        [DisplayName("Año Rige")]
        public int AnioRige { get; set; }

        [DataMember]
        [DisplayName("Fecha Rige")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecRige { get; set; }

        [DataMember]
        [DisplayName("Fecha Vence")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FecVence { get; set; }

        [DataMember]
        [DisplayName("Fecha Rige Integra")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecRigeIntegra { get; set; }

        [DataMember]
        [DisplayName("Fecha Vence Integra")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FecVenceIntegra { get; set; }

        [DataMember]
        [DisplayName("Fecha Ult R")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FecUltRige { get; set; }

        [DataMember]
        [DisplayName("Fecha Ult V")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FecUltVence { get; set; }

        [DataMember]
        [DisplayName("Observaciones")]
        public string Observaciones { get; set; }

        [DataMember]
        public int CodigoModulo { get; set; }

        [DataMember]
        public int CodigoObjetoEntidad { get; set; }

        [DataMember]
        public decimal IndDato { get; set; }
    }
}