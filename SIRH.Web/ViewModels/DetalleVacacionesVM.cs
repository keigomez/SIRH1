using SIRH.DTO;
using System;
using System.Collections.Generic;


namespace SIRH.Web.ViewModels
{
    public class DetalleVacacionesVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CPeriodoVacacionesDTO Periodo { get; set; }
        public CDetalleContratacionDTO DetalleContratacion { get; set; }
        public List<CPeriodoVacacionesDTO> PeriodosActivos { get; set; }
        public List<CPeriodoVacacionesDTO> PeriodosNoActivos { get; set; }
        public List<CPeriodoVacacionesDTO> PeriodosEmulacion { get; set; }
        public List<CRegistroVacacionesDTO> RegistroVacaciones { get; set; }
        public List<CReintegroVacacionesDTO> ReintegroVacaciones { get; set; }

        public CErrorDTO Error { get; set; }

        public int codigoDetalle = 0;

        public int detalle { get; set; }

        public String estadoPeriodo = "Valido";

        public decimal TotalRegistros { get; set; }
        public decimal TotalReintegros { get; set; }
        public string Alerta { get; set; }
    }
}