using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
    [DataContract(IsReference = true)]
    public class CBorradorAccionPersonalDTO : CBaseDTO
    {

        [DataMember]
        public CEstadoBorradorDTO EstadoBorrador { get; set; }

        [DataMember]
        [DisplayName("Número Oficio")]
        public string NumOficio { get; set; }
       
        [DataMember]
        [DisplayName("Usuario Asignado")]
        public string UsuarioAsignado { get; set; }
        
        [DataMember]
        [DisplayName("Justificación")]
        public string ObsJustificacion { get; set; }
    }
}