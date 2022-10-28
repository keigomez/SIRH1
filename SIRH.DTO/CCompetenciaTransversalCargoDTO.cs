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
    public class CCompetenciaTransversalCargoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Competencia")]
        public int? TipoCompetencia { get; set; }

        [DataMember]
        [DisplayName("Nivel de dominio")]
        public int? NivelDominio { get; set; }

        [DataMember]
        public CCargoDTO Cargo { get; set; }

        [DataMember]
        public List<CComportamientoTransversalDTO> ComportamientosTransversales { get; set; }

    }

    [DataContract]
    public class CComportamientoTransversalDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Descripción comportamiento")]
        public string ComportamientoTransversal { get; set; }

        [DataMember]
        public CCompetenciaTransversalCargoDTO CompetenciaTransversalCargo { get; set; }

        [DataMember]
        public List<CEvidenciaComportamientoTransversalDTO> EvidenciasComportamientoTransversal { get; set; }

    }

    [DataContract]
    public class CEvidenciaComportamientoTransversalDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Descripción evidencia")]
        public string Evidencia { get; set; }

        [DataMember]
        public CComportamientoTransversalDTO ComportamientoTransversal { get; set; }

    }
}
