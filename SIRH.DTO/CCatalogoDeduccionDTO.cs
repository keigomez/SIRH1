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
   public class CCatalogoDeduccionDTO : CBaseDTO
    {
        [DataMember]
        public CTipoDeduccionDTO TipoDeduccion { get; set; }
        [DataMember]
        [DisplayName("Código de deducción")]
        public int CodigoDeduccion { get; set; }
        [DataMember]
        [DisplayName("Descripción")]
        public string DescripcionDeduccion { get; set; }
        [DataMember]
        [DisplayName("Porcentaje")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal PorcentajeDeduccion { get; set; }
    }
}
