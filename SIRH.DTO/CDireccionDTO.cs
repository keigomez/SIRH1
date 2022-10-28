using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CDireccionDTO : CBaseDTO
    {
        [DataMember]
        public CDistritoDTO Distrito { get; set; }
        [DataMember]      
        public CFuncionarioDTO Funcionario { get; set; }
        [DataMember]
        [DisplayName("Dirección Domicilio")]
        public string DirExacta { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public int IndEstDireccion { get; set; }
    }
}
