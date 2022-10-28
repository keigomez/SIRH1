using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIRH.Web.ViewModels
{
    public class FormularioRegistroVacacionesVM
    {
        public CRegistroVacacionesDTO RegistroVacaciones { get; set; }
        public CPeriodoVacacionesDTO PeriodoVacaciones { get; set; }
        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CNombramientoDTO Nombramiento { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        //public SelectList PeriodosActivos { get; set; }
        //public string PeriodoSeleccion { get; set; }
        public SelectList tipoDocumento { get; set; }
        public string tipoDocumentoSeleccion { get; set; }
        public string UltimoPeriodo { get; set; }
    }
}