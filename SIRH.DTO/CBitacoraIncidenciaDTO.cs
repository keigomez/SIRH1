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
    public class CBitacoraIncidenciaDTO : CBaseDTO
    {
        [DataMember]
        public CIncidenciaDTO Incidencia { get; set; }
        [DataMember]
        public CFuncionarioDTO Funcionario { get; set; }
        [DataMember]
        public int EstSolicitud { get; set; }
        [DataMember]
        public DateTime FecEjecucion { get; set; }
    }
}