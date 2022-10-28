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
    public class CDeduccionEfectuadaDTO : CBaseDTO
    {
        [DataMember]
        public CCatalogoDeduccionDTO CatalogoDeduccion { get; set; }
        [DataMember]
        public CPagoFeriadoDTO PagoFeriado { get; set; }
        [DataMember]
        [DisplayName("Porcentaje efectuado")]
        public decimal PorcentajeEfectuado { get; set; }
        [DataMember]
        [DisplayName("Monto de deducción")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal MontoDeduccion { get; set; }
    }
}
