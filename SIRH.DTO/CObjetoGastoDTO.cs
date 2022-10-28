using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CObjetoGastoDTO : CBaseDTO
    {

        [DataMember]
        public CSubPartidaDTO SubPartida { get; set; }

        [DataMember]
        [DisplayName("Código del Objeto de Gasto")]
        public string CodObjGasto { get; set; }
        [DataMember]
        [DisplayName("Descripción del Objeto de Gasto")]
        public string DesObjGasto { get; set; }
    }
}
