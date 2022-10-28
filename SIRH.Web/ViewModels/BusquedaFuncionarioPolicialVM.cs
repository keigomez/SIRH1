using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class BusquedaFuncionarioPolicialVM
    {
        public CFuncionarioDTO Funcionario { get; set; }

        public CPuestoDTO Puesto { get; set; }

        public CDetallePuestoDTO DetallePuesto { get; set; }

        public CDetalleContratacionDTO DetalleContratacion { get; set; }

        [DisplayName("Desde (Fecha de ingreso al régimen)")]
        public DateTime FechaDesde { get; set; }

        [DisplayName("Hasta (Fecha de ingreso al régimen)")]
        public DateTime FechaHasta { get; set; }      

        public CErrorDTO Error { get; set; }
    }
}