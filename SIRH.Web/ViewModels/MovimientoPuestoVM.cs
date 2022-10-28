using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class MovimientoPuestoVM
    {
        public CMovimientoPuestoDTO MovimientoPuesto { get; set; }
        public CMotivoMovimientoDTO MotivoMovimiento { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CFuncionarioDTO Funcionario { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CErrorDTO Error { get; set; }
        /*---------------------------------------------------------*/
        public List<CMovimientoPuestoDTO> MovimientosPuesto { get; set; }
        public string MotivoSeleccionado { get; set; }
        public DateTime? FechaMovimiento { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public SelectList MotivosMovimiento { get; set; }
        public string CodPuesto { get; set; }
        public string CodCedula { get; set; }
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
        public int TotalMovimientos { get; set; }
        public int TotalPaginas { get; set; }
        public int PaginaActual { get; set; }
        public bool PermisoVacantes { get; set; }
    }
}
