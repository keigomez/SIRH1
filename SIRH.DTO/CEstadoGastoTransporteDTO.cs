using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
    [DataContract]
    public class CEstadoGastoTransporteDTO : CBaseDTO
    {

        [DataMember]
        [DisplayName("Estado del Gasto de Transporte")]
        [Required(ErrorMessage = "Obligatorio")]
        public string NomEstadoDTO { set; get; }
    }
}
