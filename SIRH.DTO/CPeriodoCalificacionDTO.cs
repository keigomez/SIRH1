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
    public class CPeriodoCalificacionDTO : CBaseDTO
    {
      
        [DataMember]
        [DisplayName("Fecha Rige")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecRige { get; set; }

        [DataMember]
        [DisplayName("Fecha Vence")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FecVence { get; set; }

        [DataMember]
        [DisplayName("Fecha Rige Regla Técnica")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecRigeReglaTecnica { get; set; }

        [DataMember]
        [DisplayName("Fecha Vence Regla Técnica")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FecVenceReglaTecnica { get; set; }

    }
}