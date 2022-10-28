using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CDecretoDTO : CBaseDTO
    {
        [DataMember]
        [Required(ErrorMessage = "Debe agregar el número de decreto")]
        [DisplayName("Numero de decreto")]
        public string NumeroDecreto { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Debe ingresar el título")]
        [DisplayName("Titulo de decreto")]
        public string TituloDecreto { get; set; }

        [DataMember]
        [DisplayName("Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaDecreto { get; set; }

        [DataMember]
        [DisplayName("Observacion")]
        public string ObservacionDecreto { get; set; }



    }
}
