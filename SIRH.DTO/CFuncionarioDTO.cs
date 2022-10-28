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
    public class CFuncionarioDTO : CBaseDTO
    {
        [DataMember]
        public CEstadoFuncionarioDTO EstadoFuncionario { get; set; }
        [DataMember]
        [DisplayName("Cédula")]
        [Required(ErrorMessage = "La cédula es requerida")]
        [RegularExpression(@"\d{10}", ErrorMessage="El número cédula debe ser de al menos 10 dígitos")]
        public string Cedula { get; set; }
        [DataMember]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [DataMember]
        [DisplayName("Primer Apellido")]
        public string PrimerApellido { get; set; }
        [DataMember]
        [DisplayName("Segundo Apellido")]
        public string SegundoApellido { get; set; }
        [DataMember]
        [DisplayName("Fecha de Nacimiento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaNacimiento { get; set; }
        [DataMember (IsRequired=false)]
        [DisplayName("Género")]
        public GeneroEnum Sexo { get; set; }
    }

    [DataContract]
    public enum GeneroEnum
    {
        [EnumMember]
        Masculino = 1,
        [EnumMember]
        Femenino = 2,
        [EnumMember]
        Indefinido = 3
    }

}
