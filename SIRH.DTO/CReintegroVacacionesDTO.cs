using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.DTO
{
    [DataContract]
    public class CReintegroVacacionesDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Fecha rige")]
        public DateTime FechaRige { get; set; }
        [DataMember]
        [DisplayName("Fecha vence")]
        public DateTime FechaVence { get; set; }

        [DataMember]
        [DisplayName("Fecha actualización")]
        public DateTime FechaActualizacion { get; set; }
        [DataMember]
        [DisplayName("Motivo")]
        public int Motivo { get; set; }
        [DataMember]
        [DisplayName("Observaciones")]
        public string Observaciones { get; set; }
        [DataMember]
        [DisplayName("Número solicitud reintegro")]
        public string SolReintegro { get; set; }

        [DataMember]
        [DisplayName("Cantidad de días")]
        public decimal CantidadDias { get; set; }

        [DataMember]
        [DisplayName("Fuente")]
        public string Fuente { get; set; }

        [DataMember]
        public CRegistroVacacionesDTO RegistroVacaciones { get; set; }
    }
}
