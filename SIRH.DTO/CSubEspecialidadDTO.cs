using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CSubEspecialidadDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Sub Especialidad")]
        public string DesSubEspecialidad { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public int IndEstSubEspecialidad { get; set; }
    }
}
