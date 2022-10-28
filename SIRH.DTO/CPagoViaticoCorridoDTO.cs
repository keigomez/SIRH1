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
    public class CPagoViaticoCorridoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("[FK_ViaticoCorrido]")]
        public CViaticoCorridoDTO ViaticoCorridoDTO { get; set; }

        [DataMember]
        [DisplayName("Estado")]
        public short IndEstado { get; set; }

        [DataMember]
        [DisplayName("Fecha Pago")]
        public DateTime FecPago { get; set; }

        [DataMember]
        [DisplayName("Monto Pago")]
        public decimal MonPago { get; set; }

        [DataMember]
        [DisplayName("Hoja Individualizada")]
        public string HojaIndividualizada { get; set; }

        [DataMember]
        [DisplayName("Número Boleta")]
        public string NumBoleta { get; set; }

        [DataMember]
        [DisplayName("Reserva Recurso")]
        public string ReservaRecurso { get; set; }

        [DataMember]
        [DisplayName("Fecha Registro")]
        public DateTime FecRegistro { get; set; }

        [DataMember]
        [DisplayName("Seleccionar")]
        public bool IndSeleccionar { get; set; }


        [DataMember]
        public List<CDetallePagoViaticoCorridoDTO> Detalles { get; set; }

        [DataMember]
        public List<CPagoViaticoRetroactivoDTO> PagosRetroactivos { get; set; }
    }

    [DataContract]
    public class CPagoViaticoRetroactivoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("[FK_PagoViaticoCorrido]")]
        public CPagoViaticoCorridoDTO PagoDTO { get; set; }

        [DataMember]
        [DisplayName("Fecha Pago")]
        public DateTime FecPago { get; set; }

        [DataMember]
        [DisplayName("Monto Pago")]
        public decimal MonPago { get; set; }

        [DataMember]
        public List<CDetallePagoViaticoCorridoDTO> Detalles { get; set; }

    }

    [DataContract]
    public class CDetallePagoViaticoRetroactivooDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("[FK_PagoViaticoCorrido]")]
        public CPagoViaticoRetroactivoDTO PagoDTO { get; set; }

        [DataMember]
        [DisplayName("[FK_TipoDetallePago]")]
        public CTipoDetallePagoViaticoDTO TipoDetalleDTO { get; set; }

        [DataMember]
        [DisplayName("Día Pago")]
        public string FecDiaPago { get; set; }

        [DataMember]
        [DisplayName("Monto Pago")]
        public decimal MonPago { get; set; }

        [DataMember]
        [DisplayName("Cod Entidad")]
        public int CodEntidad { get; set; }
    }
}
