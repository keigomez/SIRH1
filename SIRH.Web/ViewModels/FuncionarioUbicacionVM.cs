using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class FuncionarioUbicacionVM
    {
        public List<CFuncionarioDTO> Funcionario { get; set; }
        public int TotalFuncionarios { get; set; }
        public int TotalPaginas { get; set; }
        public int PaginaActual { get; set; }
        public string DivisionSearch { get; set; }
        public string DireccionSearch { get; set; }
        public string DepartamentoSearch { get; set; }
        public string SeccionSearch { get; set; }
        public string PresupuestoSearch { get; set; }
    }
}
