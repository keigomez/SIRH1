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
    public class CDetalleSolicitudCambioDTO : CBaseDTO
    {
        [DataMember]
        public CSolicitudCambioDTO Solicitud { get; set; }

        [DataMember]
        public CCatCampoDTO Campo { get; set; }

        [DataMember]
        [DisplayName("Dato Original")]
        public string DatoOriginal { get; set; }

        [DataMember]
        [DisplayName("Dato Modificar")]
        public string DatoModificar { get; set; }

        [DataMember]
        [DisplayName("Valor PK/FK/Id")]
        public string ValorModificar { get; set; }
        
        [DataMember]
        public int Estado { get; set; }

        [DataMember]
        public bool Aplica { get; set; }

        [DataMember]
        [DisplayName("Observaciones")]
        public string DesObservaciones { get; set; }
    }
}