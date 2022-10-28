using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CMetaIndividualEvidenciaDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Meta")]
        public CMetaIndividualCalificacionDTO Meta { get; set; }

        [DataMember]
        [DisplayName("Comentario")]
        public string DesEvidencia { get; set; }

        [DataMember]
        [DisplayName("Estado")]
        public int IndEstado { get; set; }

        [DataMember]
        [DisplayName("Fecha Registro")]
        public DateTime FecRegistro { get; set; }

        [DataMember]
        [DisplayName("Dirección del archivo")]
        public string DesEnlace { get; set; }

        [DataMember]
        [DisplayName("Documento")]
        public byte[] DocAdjunto { get; set; }

        [DataMember]
        [DisplayName("Observaciones")]
        public string Observaciones { get; set; }

        [DataMember]
        [DisplayName("Archivo")]
        public string DesArchivo { get; set; }
    }
}