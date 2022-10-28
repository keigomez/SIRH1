using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class FuncionarioNombramientoVM
    {
        //Dato para subir
        public CFuncionarioDTO Funcionario { get; set; }
        public CNombramientoDTO Nombramiento { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CEstadoNombramientoDTO EstadoNombramiento { get; set; }
    }
}
