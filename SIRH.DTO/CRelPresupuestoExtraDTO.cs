using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CRelPresupuestoExtraDTO : CBaseDTO
    {
        [DataMember]
        public CRegistroTiempoExtraDTO RegistroTiempoExtra { get; set; }
        [DataMember]
        public CTipoExtraDTO RegTipExtra { get; set; }
        [DataMember]
        public CPresupuestoDTO Presupuesto { get; set; }
    }
}
