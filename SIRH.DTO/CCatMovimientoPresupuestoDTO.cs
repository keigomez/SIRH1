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
    public class CCatMovimientoPresupuestoDTO : CBaseDTO
    {
                
            [DataMember]
            [DisplayName("Movimiento Presupuesto")]
            public string DesMovimientoPresupuesto { get; set; }
        
    }
}
