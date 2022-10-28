using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
    [DataContract]
    public class CEmpleadoDispositivoDTO : CBaseDTO
    {
        [DataMember]
        public string Empleado { get; set; }

        [DataMember]
        public string  Dispositivo { get; set; }

        [DataMember]
        public string Digitos { get; set; }
    }
}
