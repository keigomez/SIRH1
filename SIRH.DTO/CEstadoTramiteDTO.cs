using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CEstadoTramiteDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Descripción")]
        public string DescripcionEstado { get; set; }
    }
}
