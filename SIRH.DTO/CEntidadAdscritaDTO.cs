using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CEntidadAdscritaDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Nombre de Entidad")]
        public string EntidadAdscrita { get; set; }
        [DataMember]
        [DisplayName("Tipo de Entidad")]
        public int TipoEntidad { get; set; }
    }
}
