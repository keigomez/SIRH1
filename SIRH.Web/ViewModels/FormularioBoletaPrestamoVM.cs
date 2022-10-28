using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SIRH.DTO;
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.Web.ViewModels
{
    public class FormularioBoletaPrestamoVM
    {
        public CBoletaPrestamoDTO BoletaPrestamo { get; set; }
        public CUsuarioDTO Usuario { get; set; }
        public CFuncionarioDTO Funcionario { get; set; }
        public CExpedienteFuncionarioDTO ExpedienteAsignado { get; set; }
        public CErrorDTO Error { get; set; }

        [DisplayName("¿Usuario Externo?")]
        public bool UsuarioExterno { get; set; }

        [DisplayName("¿Solicitante es Funcionario?")]
        public bool SolicitanteEsFuncionario { get; set; }

        public string Mensaje { get; set; }

    }
}