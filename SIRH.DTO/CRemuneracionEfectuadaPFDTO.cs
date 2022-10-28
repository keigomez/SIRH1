using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
    public class CRemuneracionEfectuadaPFDTO : CBaseDTO
    {
        [DataMember]
        public CCatalogoRemuneracionDTO CatalogoRemuneracion { get; set; }
        [DataMember]
        public CPagoFeriadoDTO PagoFeriado { get; set; }
        [DataMember]
        [DisplayName("Porcentaje efectuado")]
        //[DisplayFormat(DataFormatString = "{0:P4}")]
        public decimal PorcentajeEfectuado { get; set; }
    }
}