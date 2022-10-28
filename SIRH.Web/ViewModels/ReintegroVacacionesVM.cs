
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class ReintegroVacacionesVM
    {
        public CRegistroVacacionesDTO RegistroVacaciones { get; set; }
        public CPeriodoVacacionesDTO PeriodoVacaciones { get; set; }
        public CReintegroVacacionesDTO ReintegroVacaciones { get; set; }
        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CNombramientoDTO Nombramiento { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }


    }
}