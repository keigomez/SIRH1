using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.DTO
{
    [DataContract]
    public class CNotificacionUsuarioDTO : CBaseDTO
    {
        //comentario para actualizar
        [DataMember]
        [DisplayName("Destinatario")]
        public string Destinatario { get; set; }

        [DataMember]
        [DisplayName("Usuario destino")]
        public CUsuarioDTO UsuarioDestino { get; set; }

        [DataMember]
        [Required]
        [DisplayName("Asunto")]
        public string Asunto { get; set; }

        [DataMember]
        [Required]
        [DisplayName("Contenido")]
        public string Contenido { get; set; }

        [DataMember]
        [DisplayName("Fecha envío")]
        public DateTime FechaEnvio { get; set; }

        [DataMember]
        [DisplayName("Módulo")]
        public int Modulo { get; set; }

        [DataMember]
        [DisplayName("Referencia funcionario")]
        public string CedulaReferencia { get; set; }

        [DataMember]
        [DisplayName("Remitente")]
        public CUsuarioDTO Usuario { get; set; }
    }
}
