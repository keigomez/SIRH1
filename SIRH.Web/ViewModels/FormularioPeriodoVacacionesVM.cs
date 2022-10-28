using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRH.Web.ViewModels
{
    public class FormularioPeriodoVacacionesVM
    {
        public CPeriodoVacacionesDTO PeriodoVacaciones { get; set; }
        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CNombramientoDTO Nombramiento { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
    }
}