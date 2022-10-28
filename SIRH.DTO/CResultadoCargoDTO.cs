using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.DTO
{
    [DataContract]
    public class CResultadoCargoDTO : CBaseDTO
    {
        [DisplayName("Descripción del resultado")]
        [DataMember]
        public string ResultadoCargo { get; set; }

        [DataMember]
        public CCargoDTO Cargo { get; set; }

        [DataMember]
        public List<CActividadResultadoCargoDTO> ActividadesResultado { get; set; }

    }

    [DataContract]
    public class CActividadResultadoCargoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Descripción actividad")]
        public string ActividadResultadoCargo { get; set; }

        [DataMember]
        public CResultadoCargoDTO ResultadoCargo { get; set; }

    }
}
