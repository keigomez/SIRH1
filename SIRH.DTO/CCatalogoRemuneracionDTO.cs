using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
    public class CCatalogoRemuneracionDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Descripción")]
        public string DescripcionRemuneracion { get; set; }
        [DataMember]
        [DisplayName("Porcentaje")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Required(ErrorMessage = "Debe digitar el porcentaje")]
        public decimal PorcentajeRemuneracion { get; set; }
    }
}

