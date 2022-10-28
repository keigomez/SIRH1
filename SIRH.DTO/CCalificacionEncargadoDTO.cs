using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.DTO
{
    public class CCalificacionEncargadoDTO : CBaseDTO
    {
        [DataMember]
        public CFuncionarioDTO Funcionario { get; set; }
        [DataMember]
        public CSeccionDTO Seccion { get; set; }

        [DataMember]
        public int IndEstado { get; set; }
    }
}
