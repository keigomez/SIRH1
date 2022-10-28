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
    public class CHistorialEstadoCivilDTO : CBaseDTO
    {
        [DataMember]
        public CCatEstadoCivilDTO CatEstadoCivil { get; set; }
        [DataMember]
        public CFuncionarioDTO Funcionario { get; set; }
        [DataMember]
        [DisplayName("Fecha de Inicio")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecIncio { get; set; }
        [DataMember]
        [DisplayName("Fecha de Fin")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecFin { get; set; }

    }
}
