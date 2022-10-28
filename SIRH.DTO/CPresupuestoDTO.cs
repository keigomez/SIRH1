using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CPresupuestoDTO : CBaseDTO
    {
        [DataMember]
        public CProgramaDTO Programa { get; set; }
        [DataMember]
        [DisplayName("Área")]
        public string Area{ get; set; }
        [DataMember]
        [DisplayName("Actividad")]
        public string Actividad { get; set; }
        [DataMember]
        [DisplayName("Unidad Presupuestaria")]
        public string IdUnidadPresupuestaria { get; set; }
        [DataMember]
        [DisplayName("Dirección Presupuestaria")]
        public string IdDireccionPresupuestaria { get; set; }
        [DataMember]
        [DisplayName("Código Presupuestario")]
        public string CodigoPresupuesto { get; set; }
        [DataMember]
        public string text { get; set; }
    }
}
