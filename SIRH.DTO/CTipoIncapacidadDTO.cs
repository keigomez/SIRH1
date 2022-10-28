using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CTipoIncapacidadDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Tipo de Incapacidad")]
        public string DescripcionTipoIncapacidad { get; set; }

        [DataMember]
        public CEntidadMedicaDTO EntidadMedica { get; set; }
    }
}
