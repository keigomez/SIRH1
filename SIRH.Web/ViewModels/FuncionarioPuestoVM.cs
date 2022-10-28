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
using SIRH.DTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SIRH.Web.ViewModels
{
    public class FuncionarioPuestoVM
    {
        public List<CFuncionarioDTO> Funcionario { get; set; }
        public int TotalFuncionarios { get; set; }
        public List<CPuestoDTO> Puestos { get; set; }
        public int TotalPuestos { get; set; }
        public int TotalPaginas { get; set; }
        public int PaginaActual { get; set; }
        public string CodPuestoSearch { get; set; }
        public string CodClaseSearch { get; set; }
        public string CodNivelSearch { get; set; }
        public string CodEspecialidadSearch { get; set; }
        public string CodOcupacionRealSearch { get; set; }

        public SelectList estadosPuestos { get; set; }
        public string CodEstadoPuestoSearch { get; set; }
        public SelectList listaConfianza { get; set; }
        public string CodConfianzaSearch { get; set; }
    }
}
