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
    public class COrdenMovimientoDeclaracionDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Fecha de Certificación del Registro Judicial: ")]
        public DateTime FechaCertificacion { get; set; }

        [DataMember]
        [DisplayName("Condición Académica: ")]
        public string Academica { get; set; }

        [DataMember]
        [DisplayName("Experiencia: ")]
        public string Experiencia { get; set; }

        [DataMember]
        [DisplayName("Capacitación: ")]
        public string Capacitacion { get; set; }

        [DataMember]
        [DisplayName("Licencias o permisos especiales: ")]
        public string Licencias { get; set; }

        [DataMember]
        [DisplayName("Colegiaturas: ")]
        public string Colegiaturas { get; set; }
    }
}
