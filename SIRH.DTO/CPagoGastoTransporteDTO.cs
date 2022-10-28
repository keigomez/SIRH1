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
    public class CPagoGastoTransporteDTO : CBaseDTO
    {
        /// <summary>
        /// Objeto que representa el gasto de transporte del que 
        /// se trata el pago. Representa la llave foránea en la BD
        /// </summary>
        [DataMember]
        [DisplayName("[FK_GastoTransporte]")]
        public CGastoTransporteDTO GastoTransporteDTO { get; set; }

        /// <summary>
        /// Indicador del estado de pago del gasto de transporte
        /// </summary>
        [DataMember]
        [DisplayName("Estado de pago del Gasto de Transporte")]
        public short IndEstado { get; set; }

        /// <summary>
        /// Fecha de pago del gasto de transporte
        /// </summary>
        [DataMember]
        [DisplayName("Fecha de pago")]
        public DateTime FecPago { get; set; }

        [DataMember]
        [DisplayName("Monto del Contrato")]
        public decimal MonContrato { get; set; }

        /// <summary>
        /// Monto total del pago del gasto
        /// </summary>
        [DataMember]
        [DisplayName("Monto del pago")]
        public decimal MonPago { get; set; }

        [DataMember]
        [DisplayName("Hoja Individualizada")]
        public string HojaIndividualizada { get; set; }

        /// <summary>
        /// Numero de boleta para el pago del gasto
        /// </summary>
        [DataMember]
        [DisplayName("Número de Boleta")]
        public string NumBoleta { get; set; }

        /// <summary>
        /// ...
        /// </summary>
        [DataMember]
        [DisplayName("Reserva Recurso")]
        public string ReservaRecurso { get; set; }

        [DataMember]
        [DisplayName("Fecha Registro")]
        public DateTime FecRegistro { get; set; }

        [DataMember]
        [DisplayName("Seleccionar")]
        public bool IndSeleccionar { get; set; }
        /// <summary>
        /// Lista con los detalles de pago del gasto de transporte
        /// </summary>
        [DataMember]
        public List<CDetallePagoGastoTrasporteDTO> Detalles { get; set; }


    }
}
