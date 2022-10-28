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
    public class CRelPuestoUbicacionDTO : CBaseDTO
    {
        [DataMember]
        public CUbicacionPuestoDTO UbicacionPuesto { get; set; }
        [DataMember]
        public CPuestoDTO IdPuesto { get; set; }
        [DataMember]
        [DisplayName("Fecha de creación")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecCreacion { get; set; }
    }

}
