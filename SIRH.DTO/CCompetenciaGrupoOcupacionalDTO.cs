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
    public class CCompetenciaGrupoOcupacionalDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Nivel de dominio")]
        public int? Nivel { get; set; }

        [DataMember]
        [DisplayName("Grupo ocupacional")]
        public string TipoGrupoOcupacional { get; set; }

        [DataMember]
        public List<CComportamientoGrupoOcupacionalDTO> ComportamientosGrupo { get; set; }

        [DataMember]
        public CCargoDTO Cargo { get; set; }

    }

    [DataContract]
    public class CComportamientoGrupoOcupacionalDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Descripción comportamiento")]
        public string Comportamiento { get; set; }

        [DataMember]
        public List<CEvidenciaGrupoOcupacionalDTO> EvidenciasGrupo { get; set; }

        [DataMember]
        public CCompetenciaGrupoOcupacionalDTO CompetenciaGrupoOcupacional { get; set; }

    }

    [DataContract]
    public class CEvidenciaGrupoOcupacionalDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Descripción evidencia")]
        public string Evidencia { get; set; }

        [DataMember]
        public CComportamientoGrupoOcupacionalDTO ComportamientoGrupoOcupacional { get; set; }

    }
}
