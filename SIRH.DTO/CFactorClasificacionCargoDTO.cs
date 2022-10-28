using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.DTO
{
    [DataContract]
    public class CFactorClasificacionCargoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Independencia")]
        public string Independencia { get; set; }

        [DataMember]
        [DisplayName("Supervision ejercida")]
        public string SupervisionEjercida { get; set; }

        [DataMember]
        [DisplayName("Lugares")]
        public string Lugares { get; set; }

        [DataMember]
        [DisplayName("Ambiente")]
        public string Ambiente { get; set; }

        [DataMember]
        [DisplayName("Condiciones")]
        public string Condiciones { get; set; }

        [DataMember]
        [DisplayName("Modalidad de trabajo")]
        public string ModalidadTrabajo { get; set; }

        [DataMember]
        [DisplayName("Impacto de la gestión")]
        public string ImpactoGestion { get; set; }

        [DataMember]
        [DisplayName("Relaciones de trabajo")]
        public string RelacionesTrabajo { get; set; }

        [DataMember]
        [DisplayName("Activos, equipos, insumos")]
        public string ActivosEquiposInsumos { get; set; }

        [DataMember]
        public CCargoDTO Cargo { get; set; }

    }
}
