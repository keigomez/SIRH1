using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CTipoJornadaDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Tipo de Jornada")]
        public string DescripcionJornada { get; set; }
        [DataMember]
        [DisplayName("Inicio de Jornada")]
        public string InicioJornada { get; set; }
        [DataMember]
        [DisplayName("Fin de Jornada")]
        public string FinJornada { get; set; }
        [DataMember]
        [DisplayName("Jornada Acumulativa")]
        public bool JornadaAcumulativa { get; set; }
        [DataMember]
        [DisplayName("Día Libre")]
        public string DiaLibre { get; set; }
    }
}
