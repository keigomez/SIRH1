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
    public class CUsuarioDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Nombre de Usuario")]
        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        public string NombreUsuario { get; set; }
        [DataMember]
        [DisplayName("Número Telefónico")]
        public string TelefonoOficial { get; set; }
        [DataMember]
        [DisplayName("E-mail oficial")]
        public string EmailOficial { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public int IndEstUsuario { get; set; }
    }
}
