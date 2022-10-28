using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CPerfilDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Nombre Perfil")]
        public string NomPerfil { get; set; }
        [DataMember]
        [DisplayName("Descripción Perfil")]
        public string DesPerfil { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public int IndEstPerfil { get; set; }

    }
}
