using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CEntidadFinancieraDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Entidad Financiera")]
        public string NomEntidad { get; set; }
        [DataMember]
        [DisplayName("Codigo Entidad")]
        public string CodEntidad { get; set; }
    }
}
