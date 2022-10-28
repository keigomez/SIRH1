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
    public class CGastoTransporteDTO : CBaseDTO
    {
        /// <summary>
        /// Nombramiento del funcionario a quien se le gestiona un gasto de transporte
        /// </summary>
        [DataMember]
        public CNombramientoDTO NombramientoDTO { get; set; }

        /// <summary>
        /// Estado de gasto de transporte, son: válido, en espera, anulado o vencido
        /// </summary>
        [DataMember]
        [DisplayName("[FK_EstadoGastoTransporte]")]
        public CEstadoGastoTransporteDTO EstadoGastoTransporteDTO { get; set; }

        /// <summary>
        /// Fecha de inicio de vigencia del gasto de transporte
        /// </summary>
        [DataMember]
        [DisplayName("Fecha Inicio")]
        [Required(ErrorMessage = "La Fecha Inicio es obligatoria")]
        public DateTime FecInicioDTO { get; set; }

        /// <summary>
        /// Fecha final de vigencia del gasto de transporte
        /// </summary>
        [DataMember]
        [DisplayName("Fecha Fin")]
        [Required(ErrorMessage = "La Fecha Final es obligatoria")]
        public DateTime FecFinDTO { get; set; }

        /// <summary>
        /// Monto o valor del gasto de transporte
        /// </summary>
        [DataMember]
        [DisplayName("Monto Gasto Transporte")]
        public string MontGastoTransporteDTO { get; set; }

        [DataMember]
        [DisplayName("Monto Actual")]
        public decimal MonActual { get; set; }
        /// <summary>
        /// Comentarios sobre el gasto de transporte
        /// </summary>
        [DataMember]
        [DisplayName("Observacion Gasto Transporte")]
        public string ObsGastoTransporteDTO { get; set; }

        /// <summary>
        /// Codigo generado por el sistema con el formato 'yyyy-#' que se muestra al usuario
        /// </summary>
        [DataMember]
        [DisplayName("N° de registro del Gasto de transporte")]
        public string CodigoGastoTransporte { get; set; }

        [DataMember]
        [DisplayName("Fecha Pago")]
        public DateTime FecPagoDTO { get; set; }

        [DataMember]
        [DisplayName("Monto Pago")]
        public decimal MonPagoDTO { get; set; }

        /// <summary>
        /// Fecha en que se agrega el registro de GT en la BD
        /// </summary>
        [DataMember]
        public DateTime FecRegistroDTO { get; set; }
        
        [DataMember]
        [DisplayName]
        public CPresupuestoDTO PresupuestoDTO { get; set; }

        [DataMember]
        [DisplayName("Documentos Adjuntos")]
        public byte[] DocAdjunto { get; set; }

        [DataMember]
        [DisplayName("Lugar Contrato")]
        public CDistritoDTO DistritoContrato { get; set; }

        [DataMember]
        [DisplayName("Lugar Trabajo")]
        public CDistritoDTO DistritoTrabajo { get; set; }

        /// <summary>
        /// Lista de pagos del gasto de transporte si los tiene
        /// </summary>
        [DataMember]
        public List<CPagoGastoTransporteDTO> Pagos { get; set; }
        /// <summary>
        /// Lista de movimientos realizados para el gasto de transporte
        /// </summary>
        [DataMember]
        public List<CMovimientoGastoTransporteDTO> Movimientos { get; set; }

        [DataMember]
        public List<CGastoTransporteReintegroDTO> Reintegros { get; set; }


        [DataMember]
        public List<CGastoTransporteRutasDTO> Rutas { get; set; }
    }
}
