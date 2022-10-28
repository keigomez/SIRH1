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
    public class CFolioDTO : CBaseDTO
    {

        [DataMember]
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        [DataMember]
        public int IdTomo { get; set; }

        [DataMember]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Fecha Creación")]
        public DateTime FechaCreacion { get; set; }

        [DataMember]
        [DisplayName("Número de Folio")]
        public int NumeroFolio { get; set; }

    }
}
