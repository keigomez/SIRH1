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
    public class CCatalogoPreguntaDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("DesPreguntaDTO")]
        public string DesPreguntaDTO { get; set; }

        [DataMember]
        [DisplayName("IndTipoFormularioDTO")]
        public int IndTipoFormularioDTO { get; set; }

        [DataMember]
        [DisplayName("IndEstadoDTO")]
        public int IndEstadoDTO { get; set; }

        [DataMember]
        [DisplayName("DesTituloPDTO")]
        public string DesTituloPDTO { get; set; }


    }
}
