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
    public class CCalificacionCapacitacionDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("FK_CalificacionNombramiento")]
        public CCalificacionNombramientoFuncionarioDTO CalificacionFuncionario { get; set; }

        [DataMember]
        [DisplayName("FK_TipoPP")]
        public CTipoPoliticaPublicaDTO TipoPP { get; set; }

        [DataMember]
        [DisplayName("Estado")]
        public int IndEstado { get; set; }

        [DataMember]
        [DisplayName("Línea estratégica de la política pública seleccionada")]
        public string DesLinea { get; set; }
        [DataMember]
        [DisplayName("Nombre de la Capacitación")]
        public string DesCapacitacion { get; set; }

        [DataMember]
        [DisplayName("Temas Específicos que debe contener la Capacitación")]
        public string DesTemas { get; set; }

        [DataMember]
        [DisplayName("¿Qué vacío o problema se pretende resolver con ésta actividad de Capacitación?")]
        public string DesObjetivos { get; set; }
    }
}
