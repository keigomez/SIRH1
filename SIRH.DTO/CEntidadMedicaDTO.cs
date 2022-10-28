using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CEntidadMedicaDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Nombre de Entidad")]
        public string DescripcionEntidadMedica { get; set; }
    }
}