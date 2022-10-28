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
    public class CDetallePagoGastoTrasporteDTO : CBaseDTO
    {
        /// <summary>
        /// Objeto que representa un Pago de gasto de transporte. 
        /// Representa la llave foránea en la BD
        /// </summary>
        [DataMember]
        [DisplayName("[FK_PagoGastoTransporte]")]
        public CPagoGastoTransporteDTO PagoDTO { get; set; }

        /// <summary>
        /// Objeto que representa el tipo de este detalle de pago
        /// </summary>
        [DataMember]
        [DisplayName("[FK_TipoDetallePago]")]
        public CTipoDetallePagoGastoDTO TipoDetalleDTO { get; set; }

        /// <summary>
        /// Día de pago del gasto
        /// </summary>
        [DataMember]
        [DisplayName("Día de pago")]
        public string FecDiaPago { get; set; }

        /// <summary>
        /// Monto del pago
        /// </summary>
        [DataMember]
        [DisplayName("Monto de pago del gasto")]
        public decimal MonPago { get; set; }

        /// <summary>
        /// ...
        /// </summary>
        [DataMember]
        [DisplayName("Codigo de Entidad")]
        public int CodEntidad { get; set; }
    }
}
