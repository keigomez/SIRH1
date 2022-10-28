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
    public class CDetalleDeduccionViaticoCorridoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("FK_MovimientoViaticoCorrido")]
        public CMovimientoViaticoCorridoDTO MovimientoViaticoCorridoDTO { get; set; }
        [DataMember]
        [DisplayName("DesMotivo")]
        public string DesMotivoDTO { get; set; }
        [DataMember]
        [DisplayName("FechaRige")]
        public string FecRigeDTO { get; set; }
        [DataMember]
        [DisplayName("FechaVence")]
        public string FecVenceDTO { get; set; }
        [DataMember]
        [DisplayName("numNoDia")]
        public int NumNoDiaDTO { get; set; }
        [DataMember]
        [DisplayName("Monto a Bajar")]
        public string MontMontoBajarDTO { get; set; }
        [DataMember]
        [DisplayName("Monto a Rebajar")]
        public string MontMontoRebajarDTO { get; set; }
        [DataMember]
        [DisplayName("Total a Rebajar")]
        public string TotRebajarDTO { get; set; }
        [DataMember]
        [DisplayName("NumSolicitudAccionP")]
        public string NumSolicitudAccionPDTO { get; set; }

        [DataMember]
        [DisplayName("Reintegrar")]
        public bool IndReintegroDTO { get; set; }
    }
}

