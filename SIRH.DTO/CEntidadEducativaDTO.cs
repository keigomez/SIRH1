using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CEntidadEducativaDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Nombre de Entidad")]
        public string DescripcionEntidad { get; set; }
        [DataMember]
        [DisplayName("Tipo de Entidad")]
        public int TipoEntidad { get; set; }
        [DataMember]
        public int Estado { get; set; }
        [DataMember]
        public string NombreTipo { get; set; }

    }
}
   