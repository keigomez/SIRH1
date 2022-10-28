using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
    public enum EstadoExtraEnum { Activo = 1, Anulado = 2, Cerrado = 3, Aprobado = 4, Rechazado = 5 }
    [DataContract]
    public class CRegistroTiempoExtraDTO : CBaseDTO
    {
        [DataMember]
        public List<CDetalleTiempoExtraDTO> Detalles { get; set; }

        [DataMember]
        public CClaseDTO Clase { get; set; }

        [DataMember]
        public CPresupuestoDTO Presupuesto { get; set; }

        [DataMember]
        public CDesgloseSalarialDTO QuincenaA { get; set; }

        [DataMember]
        public CDesgloseSalarialDTO QuincenaB { get; set; }

        [DataMember]
        [DisplayName("Fecha Emisión")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaEmision { get; set; }
        [DataMember]
        public DateTime FecRegistroDetalles { get; set; }
        [DataMember]
        [DisplayName("Fecha de Pago")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecPago { get; set; }

        [DataMember]
        [DisplayName("Monto por hora")]
        public decimal MontoDiurna { get; set; }

        [DataMember]
        [DisplayName("Monto por hora mixta")]
        public decimal? MontoMixta { get; set; }

        [DataMember]
        [DisplayName("Monto por hora nocturna")]
        public decimal? MontoNocturna { get; set; }

        [DataMember]
        [DisplayName("Monto total")]
        public decimal? MontoTotal { get; set; }

        [DataMember]
        [DisplayName("Periodo")]
        public string Periodo { get; set; }

        [DataMember]
        [DisplayName("Justificación")]
        public string Justificacion { get; set; }

        [DataMember]
        [DisplayName("Observaciones estado")]
        public string ObservacionesEstado { get; set; }

        [DataMember]
        [DisplayName("Observaciones estado jornada doble")]
        public string ObservacionesEstadoDoble { get; set; }

        [DataMember]
        [DisplayName("Área")]
        public string Area { get; set; }

        [DataMember]
        [DisplayName("Actividad")]
        public string Actividad { get; set; }

        [DataMember]
        [DisplayName("Número de Ofic. Justificación")]
        public string OficJustificacion { get; set; }

        [DataMember]
        public byte[] Archivo { get; set; }

        [DataMember]
        public CSeccionDTO Seccion { get; set; }

        [DataMember]
        public CFuncionarioDTO Funcionario { get; set; }

        [DataMember]
        public EstadoExtraEnum Estado { get; set; }
        [DataMember]
        public EstadoDetalleExtraEnum? EstadoDetalles { get; set; }
        [DataMember]
        public DateTime FechVenceNombramiento { get; set; }
        [DataMember]
        public string Ocupacion { get; set; }
    }
}
