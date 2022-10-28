using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;
using System.ComponentModel;
using System.Web.Mvc;

namespace SIRH.Web.ViewModels
{
    public class NotificacionUsuarioVM
    {
        public CNotificacionUsuarioDTO Notificacion { get; set; }

        public CFuncionarioDTO Funcionario { get; set; }

        public CPuestoDTO Puesto { get; set; }

        public CDetallePuestoDTO DetallePuesto { get; set; }

        public CExpedienteFuncionarioDTO Expediente { get; set; }

        public CDetalleContratacionDTO DetalleContratacion { get; set; }

        public CErrorDTO Error { get; set; }

        [DisplayName("Fecha de emisión desde")]
        public DateTime FechaEmisionDesde { get; set; }

        [DisplayName("Fecha de emisión hasta")]
        public DateTime FechaEmisionHasta { get; set; }

        public SelectList Asuntos { get; set; }
        [DisplayName("Asunto")]
        public string AsuntoSeleccionado { get; set; }
    }
}