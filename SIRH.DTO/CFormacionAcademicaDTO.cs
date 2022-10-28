using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    
  [DataContract]
    public class CFormacionAcademicaDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Fecha de registro")]
        public DateTime Fecha { get; set; }        
        public CFuncionarioDTO Funcionario { get; set; }
        
    }
}