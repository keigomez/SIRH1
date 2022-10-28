using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CUbicacionAdministrativaDTO : CBaseDTO
    {
        [DataMember]
        public CPresupuestoDTO Presupuesto { get; set; }
        [DataMember]
        public CSeccionDTO Seccion { get; set; }
        [DataMember]
        public CDepartamentoDTO Departamento { get; set; }
        [DataMember]
        public CDireccionGeneralDTO DireccionGeneral { get; set; }
        [DataMember]
        public CDivisionDTO Division { get; set; }
        [DataMember]
        [DisplayName("Observaciones")]
        public string DesObservaciones { get; set; }
    }
}
