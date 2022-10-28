using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    public class CEstadoFuncionarioDTO : CBaseDTO
    {
        [DataMember] 
        [DisplayName( "Estado Funcionario")]        
        public string DesEstadoFuncionario { get; set; }
    }
}
