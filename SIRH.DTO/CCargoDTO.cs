using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.DTO
{
    [DataContract]
    public class CCargoDTO : CBaseDTO
    {
        [DataMember]
        public CClaseDTO ClaseCargo { get; set; }

        [DataMember]
        public CEspecialidadDTO EspecialidadCargo { get; set; }

        [DataMember]
        public CSubEspecialidadDTO SubespecialidadCargo { get; set; }

        [DataMember]
        public CSeccionDTO SeccionCargo { get; set; }

        [DataMember]
        [DisplayName("Proceso de trabajo")]
        [Required]
        public string ProcesoTrabajo { get; set; }

        [DataMember]
        [DisplayName("Nombre del cargo")]
        [Required]
        public string NombreCargo { get; set; }

        [DataMember]
        [DisplayName("Jefatura Inmediata")]
        [Required]
        public string JefaturaInmediata { get; set; }

        [DataMember]
        [DisplayName("Jefatura superior de la inmediata")]
        [Required]
        public string JefaturaSuperiorInmediata { get; set; }

        [DataMember]
        [DisplayName("Propósito")]
        [Required]
        public string Proposito { get; set; }

        [DataMember]
        [DisplayName("Estrato")]
        [Required]
        public string Estrato { get; set; }

        [DataMember]
        [DisplayName("Ubicación Organizacional")]
        [Required]
        public string UbicacionOrganizacional { get; set; }
    }
}
