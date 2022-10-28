using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CEstadoBorradorDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Estado de Borrador")]
        public string DesEstadoBorrador { get; set; }
    }
}