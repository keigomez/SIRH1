using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SIRH.DTO
{
    [DataContract]
    public class CPeriodoEscalaSalarialDTO : CBaseDTO
    {
        [DataMember]
        public DateTime FechaInicial { get; set; }
        [DataMember]
        public DateTime FechaCierre { get; set; }
        [DataMember]
        public string NumeroResolucion { get; set; }
        [DataMember]
        public decimal MontoPuntoCarrera { get; set; }
    }
}
