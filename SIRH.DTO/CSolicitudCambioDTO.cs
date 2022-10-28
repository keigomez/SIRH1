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
    public class CSolicitudCambioDTO : CBaseDTO
    {
        [DataMember]
        public CUsuarioDTO UsuarioRemite { get; set; }

        [DataMember]
        public CUsuarioDTO UsuarioRecibe { get; set; }

        [DataMember]
        public CFuncionarioDTO Funcionario { get; set; }

        [DataMember]
        [DisplayName("Núm. Solicitud")]
        public string NumSolicitud { get; set; }

        [DataMember]
        public int Estado { get; set; }

        [DataMember]
        [DisplayName("Fecha Solicitud")]
        public DateTime FecSolicitud { get; set; }

        [DataMember]
        [DisplayName("Fecha Atencion")]
        public DateTime? FecAtencion { get; set; }

        [DataMember]
        [DisplayName("Observaciones Remite")]
        public string DesObsRemite { get; set; }

        [DataMember]
        [DisplayName("Observaciones Recibe")]
        public string DesObsRecibe { get; set; }

        [DataMember]
        public List<CDetalleSolicitudCambioDTO> Detalle { get; set; }
    }
}
