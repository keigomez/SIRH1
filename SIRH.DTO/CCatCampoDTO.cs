using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CCatCampoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Tabla")]
        public string DesCampo { get; set; }

        [DataMember]
        [DisplayName("Tabla")]
        public string DesTabla { get; set; }

        [DataMember]
        [DisplayName("Columna")]
        public string DesColumna { get; set; }
        
        [DataMember]
        [DisplayName("Pantalla")]
        public string DesPantalla { get; set; }
    }
}
