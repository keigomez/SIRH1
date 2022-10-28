using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.DTO
{
    public class CActividadFuncionarioDTO : CBaseDTO
    {
        [DataMember]
        public CFuncionarioDTO Funcionario { get; set; }
        public DateTime FechaDesde { get; set; }
        [DataMember]
        public DateTime FechaHasta { get; set; }
        [DataMember]
        public DateTime FechaRegistro { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public int IndEstado { get; set; }
        [DataMember]
        public string Evidencia { get; set; }
        [DataMember]
        public string Observaciones { get; set; }
        [DataMember]
        public int IndTeletrabajo { get; set; }
    }
}
