using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CDetalleAccesoDTO : CBaseDTO
    {
        [DataMember] 
        public CUsuarioDTO Usuario { get; set; }
        [DataMember] 
        public CFuncionarioDTO Funcionario { get; set; }
        [DataMember] 
        [DisplayName("Fecha de Creación")]
        public DateTime FecCreacion { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public int IndEstDetalleAcceso { get; set; }
    }
}
