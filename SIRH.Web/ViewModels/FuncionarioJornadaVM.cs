using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class FuncionarioJornadaVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CTipoJornadaDTO JornadaLaboral { get; set; }
    }
}
