using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CDetallePrestacionDTO : CBaseDTO
    {
        [DataMember]
        public CPrestacionLegalDTO Prestacion { get; set; }
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
        public CUsuarioDTO Usuario { get; set; }

        [DataMember]
        public CAccionPersonalDTO AccionIngreso { get; set; }
        [DataMember]
        public CAccionPersonalDTO AccionCese { get; set; }

        [DataMember]
        [DisplayName("Jefe RRHH")]
        public string NomJefe { get; set; }

        [DataMember]
        [DisplayName("Puesto")]
        public string DesPuesto { get; set; }
    }
}