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
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SIRH.Web.Models
{
    public class FuncionarioModel
    {
        public List<CFuncionarioDTO> Funcionario { get; set; }
        public List<CEMUExfuncionarioDTO> Exfuncionarios { get; set; }
        public int TotalFuncionarios { get; set; }
        public int TotalPaginas { get; set; }
        public int PaginaActual { get; set; }
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "La cédula sólo puede contener números")]
        public string CedulaSearch { get; set; }
        public string NombreSearch { get; set; }
        public string PrimerApellidoSearch { get; set; }
        public string SegundoApellidoSearch { get; set; }
        public string EstadoSearch { get; set; }
        public string CodPuestoSearch { get; set; }
        public string CodClaseSearch { get; set; }
        public string CodEspecialidadSearch { get; set; }
        public string CodNivelSearch { get; set; }
        public string CodDivisionSearch { get; set; }
        public string CodDireccionSearch { get; set; }
        public string CodDepartamentoSearch { get; set; }
        public string CodSeccionSearch { get; set; }
        public string CodPresupuestoSearch { get; set; }
        public string CodCostosSearch { get; set; }

        public string Grupo { get; set; }
        public List<bool> Parametros { get; set; }
    }
}
