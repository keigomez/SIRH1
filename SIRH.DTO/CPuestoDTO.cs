using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CPuestoDTO : CBaseDTO
    {
        [DataMember]
        public CUbicacionAdministrativaDTO UbicacionAdministrativa { get; set; }
        [DataMember]
        public CEstadoPuestoDTO EstadoPuesto { get; set; }
        [DataMember]
        [DisplayName("Número de Puesto")]
        public string CodPuesto { get; set; }
        [DataMember]
        [DisplayName("Puesto de confianza")]
        public bool PuestoConfianza { get; set; }

        [DataMember]
        [DisplayName("NivelOcupacional")]
        public int NivelOcupacional { get; set; }
        [DataMember]
        [DisplayName("Observaciones del puesto")]
        public string ObservacionesPuesto { get; set; }
        [DataMember]        
        public CEstudioPuestoDTO EstudioPuesto { get; set; }
        [DataMember]
        public CPedimentoPuestoDTO PedimentoPuesto { get; set; }
        [DataMember]
        public CDetallePuestoDTO DetallePuesto { get; set; }

        [DataMember]
        public CNombramientoDTO Nombramiento { get; set; }

        [DataMember]
        public CFamiliaPuestoDTO FamiliaPuesto { get; set; }
    }
}
