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
using System.ComponentModel;
using System.Web.Mvc;
using SIRH.DTO;
using System.Collections.Generic;

namespace SIRH.Web.ViewModels
{
    public class BusquedaAguinaldoVM
    {
        public CFuncionarioDTO Funcionario { get; set; }

        [DisplayName("Número de cédula")]
        public string Cedula { get; set; }

        [DisplayName("Periodo")]
        public DateTime? FechaDesde { get; set; }

        public List<CAguinaldoDTO> Registros { get; set; }
        public int TotalAguinaldos { get; set; }

    }
}