using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CInformacionContactoDTO : CBaseDTO
    {
        [DataMember]
        public CTipoContactoDTO TipoContacto { get; set; }
        [DataMember]
        public CFuncionarioDTO Funcionario { get; set; }
        [DataMember]
        [DisplayName("Detalle")]
        public string DesContenido { get; set; }
        [DataMember]
        [DisplayName("Detalles Adicionales")]
        public string DesAdicional { get; set; }

    }
    
}
