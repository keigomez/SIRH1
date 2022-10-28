using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
    [DataContract(IsReference = true)]
    public class CDispositivoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        [DataMember]
        [DisplayName("Ubicación")]
        public string Ubicacion { get; set; }

    }
}
