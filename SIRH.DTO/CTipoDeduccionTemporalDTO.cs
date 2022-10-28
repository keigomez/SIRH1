using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
    [DataContract(IsReference = true)]
    public class CTipoDeduccionTemporalDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Tipo de deducción")]
        public string DetalleTipoDeduccionTemporal { get; set; }

        [DataMember]
        [DisplayName("Estado")]
        public int IndEstado { get; set; }

        [DataMember]
        [DisplayName("Con Salario")]
        public int IndConSalario { get; set; }
    }
}
