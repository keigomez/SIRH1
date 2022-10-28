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
    public class CPartidaDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Código de la Partida")]
        public string CodPartida { get; set; }
        [DataMember]
        [DisplayName("Descripción de la Partida")]
        public string DesPartida { get; set; }
    }
}
