using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CActividadDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Descripción de Actividad")]
        public string DesActividad { get; set; }
    }
}
