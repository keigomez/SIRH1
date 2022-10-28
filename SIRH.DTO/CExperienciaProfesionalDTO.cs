using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{    
    [DataContract]
    public class CExperienciaProfesionalDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Fecha de registro")]
        public DateTime FechaReg { get; set; }
        [DataMember]
        [DisplayName("Puntos asignados")]
        public int IndicadorPtsAsignados { get; set; }
        [DataMember]
        [DisplayName("Tipo de Experiencia")]
        public int TipoExp { get; set; }
        [DataMember]
        [DisplayName("Observaciones de Experiencia")]
        public string ObservacionesExp { get; set; }
        [DataMember]
        [DisplayName("Número de Resolución")]
        public string NumeroResoluc { get; set; }
        [DataMember]
        public CFormacionAcademicaDTO FormacionAcademica { get; set; }

    }
}
