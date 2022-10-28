using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    public class CPlantillaEmpleadoDTO : CBaseDTO
    {
        [DataMember]
        public CEmpleadoDTO Empleado { get; set; }

        [DataMember]
        public string Plantilla { get; set; }
    }
}
