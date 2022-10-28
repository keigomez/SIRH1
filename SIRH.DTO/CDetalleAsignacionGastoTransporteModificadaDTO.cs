using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
	[DataContract]
    public class CDetalleAsignacionGastoTransporteModificadaDTO :CBaseDTO
    {
		[DataMember]
        [DisplayName("FK_GastoTransporte")]
        public CGastoTransporteDTO GastoTransporteDTO { get; set; }
        
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
        public string MontTarifa { get; set; }

        [DataMember]
        [DisplayName("Número Gaceta:")]
        public string NumGaceta { get; set; }
    }
}
