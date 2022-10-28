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
    public class CDetalleAsignacionGastoTransporteDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("FK_GastoTransporteRutas")]
        public CGastoTransporteRutasDTO Ruta { get; set; }

        [DataMember]
        [DisplayName("NomRutaDTO")]
        [Required(ErrorMessage ="En Nombre de Ruta DTO")]
        public string NomRutaDTO { get; set; }
        [DataMember]
        [DisplayName("NomFraccionamientoDTO")]
        [Required(ErrorMessage = "En Fraccionamiento DTO")]
        public string NomFraccionamientoDTO { get; set; }
        [DataMember]
        [DisplayName("Monto Tarifa:")]
        [Required(ErrorMessage = "En MontoTarifa DTO")]
        public decimal MontTarifa { get; set; }

        [DataMember]
        [DisplayName("NumGaceta")]
        public string NumGaceta { get; set; }
    }
}
