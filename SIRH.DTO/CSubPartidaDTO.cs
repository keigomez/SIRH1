using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CSubPartidaDTO : CBaseDTO
    {
        [DataMember]
        public CPartidaDTO Partida { get; set; }

        [DataMember]
        [DisplayName("Código de la SubPartida")]
        public string CodSubPartida { get; set; }
        [DataMember]
        [DisplayName("Descripción de la SubPartida")]
        public string DesSubPartida { get;  set;}
    }
}


    
