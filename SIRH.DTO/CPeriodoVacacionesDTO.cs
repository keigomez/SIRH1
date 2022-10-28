using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CPeriodoVacacionesDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Días a derecho")]
        public decimal DiasDerecho { get; set; }
        [DataMember]
        [DisplayName("Fecha de carga")]
        public DateTime FechaCarga { get; set; }
        [DataMember]
        [DisplayName("Periodo de vacaciones")]
        public string Periodo { get; set; }
        [DataMember]
        [DisplayName("Estado del periodo")]
        public int Estado { get; set; }
        [DataMember]
        [DisplayName("Saldos del periodo")]
        public double Saldo { get; set; }

        [DataMember]
        [DisplayName("Proporción a la fecha")]
        public double Proporcion { get; set; }

        [DataMember]
        [DisplayName("Proporción mensual")]
        public double ProporcionMes { get; set; }

        [DataMember]
        [DisplayName("Solicitudes del periodo")]
        public int CantidadSolicitudes { get; set; }
        public CNombramientoDTO Nombramiento { get; set; }
    }
}
