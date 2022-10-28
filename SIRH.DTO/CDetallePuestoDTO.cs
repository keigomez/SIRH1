using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CDetallePuestoDTO : CBaseDTO
   {
        [DataMember]
        public CSubEspecialidadDTO SubEspecialidad { get; set; }
        [DataMember]
        public COcupacionRealDTO OcupacionReal { get; set; }
        [DataMember]
        public CPuestoDTO Puesto { get; set; }
        [DataMember]
        public CEspecialidadDTO Especialidad { get; set; }
        [DataMember]
        public CEscalaSalarialDTO EscalaSalarial { get; set; }
        [DataMember]
        public CClaseDTO Clase { get; set; } 
        [DataMember]
        [DisplayName("Porc. Prohibición")]
        public decimal PorProhibicion { get; set; }
        [DataMember]
        [DisplayName("Por Dedicacion")]
        public decimal PorDedicacion { get; set; }
        [DataMember]
        [DisplayName("Categoría")]
        public int Categoria { get; set; }

        [DataMember]
        [DisplayName("Fecha Rige")]
        public DateTime? FecRige { get; set; }

        [DataMember]
        [DisplayName("FK_Nombramiento")]
        public int FK_Nombramiento { get; set; }
        [DataMember]
        public List<CDetallePuestoRubroDTO> DetalleRubros { get; set; }
    }
}