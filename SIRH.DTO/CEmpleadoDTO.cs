using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{

   public class CEmpleadoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Cédula")]
        public string CodigoEmpleado { get; set; }

        [DataMember]
        [DisplayName("Nombre")]
        public string PrimerNombre { get; set; }

        [DataMember]
        [DisplayName("Segundo Nombre")]
        public string SegundoNombre { get; set; }

        [DataMember]
        [DisplayName("Primer Apellido")]
        public string  ApellidoPaterno { get; set; }

        [DataMember]
        [DisplayName("Segundo Apellido")]
        public string ApellidoMaterno { get; set; }

        [DataMember]
        [DisplayName("Estado de contratación")]
        public int Estado { get; set; }

        [DataMember]
        [DisplayName("Motivo de baja")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaBaja { get; set; }

        [DataMember]
        [DisplayName("Código de acceso")]
        public string Digitos { get; set; }

    }



}
