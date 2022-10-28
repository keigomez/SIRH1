using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CSalarioDTO : CBaseDTO
    {
        [DataMember]
        public CPuestoDTO Puesto { get; set; }

        [DataMember]
        public CDetallePuestoDTO DetallePuesto { get; set; }

        [DataMember]
        [DisplayName("Aumentos Anuales")]
        public decimal MtoAumentosAnuales { get; set; }

        [DataMember]
        [DisplayName("Monto Grado")]
        public decimal MtoGradoGrupo { get; set; }

        [DataMember]
        [DisplayName("Puntos")]
        public decimal NumPuntos { get; set; }

        [DataMember]
        [DisplayName("Monto Prohib./Dedic.")]
        public decimal MtoProhibicion { get; set; }

        [DataMember]
        [DisplayName("Por. Prohib./Dedic")]
        public decimal PorProhibicion { get; set; }

        [DataMember]
        [DisplayName("Otros Sobresueldos")]
        public decimal MtoOtros { get; set; }

        [DataMember]
        [DisplayName("Mto. Total")]
        public decimal MtoTotal { get; set; }

        [DataMember]
        [DisplayName("Mto. Día")]
        public decimal MtoDia { get; set; }

        [DataMember]
        [DisplayName("Mto. Hora")]
        public decimal MtoHora { get; set; }
    }
}