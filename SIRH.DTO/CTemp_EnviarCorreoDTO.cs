using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.DTO
{
    [DataContract]
    public class CTemp_EnviarCorreoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Cédula")]
        public string Cedula { get; set; }

        [DataMember]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [DataMember]
        [DisplayName("Correo")]
        public string Correo { get; set; }

        [DataMember]
        [DisplayName("Tipo motivo")]
        public int IdMotivo { get; set; }

        [DataMember]
        [DisplayName("Detalle motivo")]
        public string DetalleMotivo { get; set; }

        [DataMember]
        [DisplayName("Fecha envío")]
        public DateTime FechaEnvio { get; set; }
    }
}
