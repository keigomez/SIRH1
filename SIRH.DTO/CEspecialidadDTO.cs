using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CEspecialidadDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Especialidad")]
        public string DesEspecialidad { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public int IndEstEspecialidad { get; set; }

    }
}
