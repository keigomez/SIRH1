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
    public class CNombramientoDTO : CBaseDTO
    {
        [DataMember]
        public CEstadoNombramientoDTO EstadoNombramiento { get; set; }
        [DataMember]
        public CPuestoDTO Puesto { get; set; }
        [DataMember]
        public CFuncionarioDTO Funcionario { get; set; }
        [DataMember]
        [DisplayName("Fecha Rige")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecRige { get; set; }
        [DataMember]
        [DisplayName("Fecha Vence")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecVence { get; set; }
        [DataMember]
        [DisplayName("Fecha NombramientoDTO")]
        public DateTime FecNombramiento { get; set; }
    } 
}
