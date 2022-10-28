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
    public class CRegistroVacacionesDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Fecha rige")]
        public DateTime FechaRige { get; set; }
        [DataMember]
        [DisplayName("Fecha vence")]
        public DateTime FechaVence { get; set; }
        [DisplayName("Fecha actualización")]
        public DateTime FechaActualizacion { get; set; }
        [DataMember]
        [DisplayName("Estado solicitud")]
        public int Estado { get; set; }
        [DataMember]
        [DisplayName("Tipo de transacción")]
        public int TipoTransaccion { get; set; }

        [DataMember]
        [DisplayName("Cantidad de dias")]
        public decimal Dias { get; set; }

        [DataMember]
        [DisplayName("Fuente")]
        public string Fuente { get; set; }

        [DataMember]
        [DisplayName("Número de transacción")]
        public string NumeroTransaccion { get; set; }

        [DataMember]
        public CPeriodoVacacionesDTO Periodo { get; set; }
    }
}
