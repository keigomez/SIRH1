using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CCatPermisoDTO : CBaseDTO
   {
        [DataMember]
        [DisplayName("Nombre Permiso")]
        public string NomPermiso { get; set; }
        [DataMember]
        [DisplayName("Descripción Permiso")]
        public string DesPermiso { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public int IndEstCatPermiso { get; set; }

        [DataMember]
        [DisplayName("Perfil")]
        public string Perfil { get; set; }
    }
}
