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
    public class CPagoExtraordinarioDTO : CBaseDTO
    {
        [DataMember]
        public CFuncionarioDTO funcionario { get; set; }
        [DataMember]
        [DisplayName("Fecha del trámite")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaTramite { get; set; }
    }
}