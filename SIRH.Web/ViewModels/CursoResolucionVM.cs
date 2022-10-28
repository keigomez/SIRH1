using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class CursoResolucionVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CDetalleContratacionDTO Detalle { get; set; }
        public CCursoCapacitacionDTO Curso { get; set; }

        //public decimal PuntosActuales { get; set; }
        //public decimal PuntosAdicionales { get; set; }
        //public decimal PuntosTotales { get; set; }
        //public decimal MontoPagar { get; set; }
        //public decimal MontoPunto { get; set; }
        //public decimal SalarioBase { get; set; }

    }
}