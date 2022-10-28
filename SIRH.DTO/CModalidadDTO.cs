using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CModalidadDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Modalidad")]
        public string Descripcion { get; set; }
        [DataMember]
        [DisplayName("Tope de puntos")]
        public int TopePuntos { get; set; }
        
    }
}
   
    